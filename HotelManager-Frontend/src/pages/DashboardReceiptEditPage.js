import DataTable from "../components/Common/DataTable";
import { NavBar } from "../components/Common/NavBar";
import { useState, useEffect } from "react";
import api_service_invoice from "../services/api_service_invoice";
import api_hotel_service from "../services/api_hotel_service";
import { useParams } from "react-router-dom";
import { formatDate } from "../common/HelperFunctions";

const DashboardReceiptEditPage = () => {
    const { receiptId } = useParams();
    const [services, setServices] = useState([]);
    const [serviceId, setServiceId] = useState("");
    const [quantity, setQuantity] = useState(0);
    const [invoiceServices, setinvoiceServices] = useState([]);

    const servicesQuery = {
        filter: {},
        sortBy: "DateCreated",
        sortOrder: "ASC"
    };
    const invoiceServicesQuery = {
        filter: {
            id: receiptId
        },
        sortBy: "DateCreated",
        sortOrder: "ASC"
    };

    useEffect(() => {
        api_hotel_service.getAllServices(servicesQuery).then((response) => {
            const data = response.data.items;
            setServices(data);
        });
    }, []);

    useEffect(() => {
        api_service_invoice.getByInvoiceId(invoiceServicesQuery).then((data) => {
            setinvoiceServices(data.map(service => ({
                ...service,
                dateCreated: formatDate(service.dateCreated)
            })));
        });
    }, [serviceId]);

    const columns = [
        { key: "serviceName", label: "Service name" },
        { key: "quantity", label: "Quantity" },
        { key: "dateCreated", label: "Date" }
    ]

    const handleAddService = () => {
        if (!serviceId) {
            alert("Service must be selected");
            return;
        }
        else if (quantity <= 0) {
            alert("Quantity must be greater than 0");
            return;
        }
        const data = {
            serviceId,
            quantity,
            invoiceId: receiptId
        };
        api_service_invoice.createServiceInvoice(data).then(() => {
            setQuantity(0);
            setServiceId("");
        });
    }

    return (
        <div>
            <NavBar />
            <h2 className="dashboard-receipt-edit-title">Edit receipt</h2>
            <div className="dashboard-receipt-edit-input-service">
                <label className="dashboard-receipt-edit-input-service-label" htmlFor="service">Service:</label>
                <select
                    className="dashboard-receipt-edit-input-service-select"
                    id="service"
                    name="service"
                    value={serviceId}
                    onChange={(e) => setServiceId(e.target.value)}
                >
                    <option value="">Select a service</option>
                    {services.map((service) => (
                        <option key={service.id} value={service.id}>
                            {service.name}
                        </option>
                    ))}
                </select>
                <label className="dashboard-receipt-edit-input-quantity-lable" htmlFor="quantity">Quantity:</label>
                <input
                    className="dashboard-receipt-edit-input-quantity-input"
                    type="number"
                    id="quantity"
                    name="quantity"
                    value={quantity}
                    onChange={(e) => setQuantity(e.target.value)}
                />
                <button className="dashboard-receipt-edit-input-add-button" onClick={handleAddService}>Add</button>
            </div>
            <div className="dashboard-receipt-edit-display-invoice-services">
                <h3>Service history</h3>
                <DataTable
                    data={invoiceServices}
                    columns={columns}
                />
            </div>
        </div>
    );
}

export default DashboardReceiptEditPage;