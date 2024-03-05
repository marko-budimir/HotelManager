import DataTable from "../components/Common/DataTable";
import Paging from "../components/Common/Paging";
import { NavBar } from "../components/Common/NavBar";
import { useState, useEffect } from "react";
import api_receipt from "../services/api_dashboard_invoice";
import { useNavigate } from "react-router";
import { formatDate } from "../common/HelperFunctions";

const DashboardReceiptPage = () => {
    const navigate = useNavigate();
    const [receipts, setReceipts] = useState([]);
    const [query, setQuery] = useState({
        filter: {
            minPrice: 0,
            maxPrice: null,
            isPaid: null,
            userEmailQuery: ""
        },
        currentPage: 1,
        pageSize: 10,
        totalPages: 1,
        sortBy: "DateCreated",
        sortOrder: "ASC"
    });

    useEffect(() => {
        api_receipt.getAllDashboardInvoice(query).then((response) => {
            const [data, totalPages] = response;
            setReceipts(data.map(receipt => ({
                ...receipt,
                dateCreated: formatDate(receipt.dateCreated)
            })));
            setQuery({
                ...query,
                totalPages
            });
        });
    }, [query.currentPage, query.sortBy, query.sortOrder, query.filter]);

    const handlePageChange = (pageNumber) => {
        setQuery({
            ...query,
            currentPage: pageNumber
        });
    };

    const columns = [
        { key: "invoiceNumber", label: "Receipt number" },
        { key: "dateCreated", label: "Date of creation" },
        { key: "totalPrice", label: "Total price" },
        { key: "email", label: "User email" },
        { key: "isPaid", label: "Is paid" }
    ]

    const handle = [
        {
            label: "Edit",
            onClick: (receipt) => {
                navigate(`/dashboardReceipt/edit/${receipt.id}`);
            }
        },
        {
            label: "View",
            onClick: (receipt) => {
                navigate(`/dashboardReceipt/view/${receipt.id}`);
            }
        }
    ]

    return (
        <>
            <NavBar />
            <DataTable
                data={receipts}
                columns={columns}
                handle={handle}
            />
            <Paging
                totalPages={query.totalPages}
                currentPage={query.currentPage}
                onPageChange={handlePageChange}
            />
        </>
    );
}

export default DashboardReceiptPage;