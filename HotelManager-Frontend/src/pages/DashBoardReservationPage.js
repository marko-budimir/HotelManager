import DataTable from "../components/Common/DataTable";
import { NavBar } from "../components/Common/NavBar";
import { useState, useEffect } from "react";
import api_reservation from "../services/api_reservation";
import api_receipt from "../services/api_dashboard_invoice";
import { useNavigate } from "react-router";
import Paging from "../components/Common/Paging";

const DashBoardReservationsPage = () => {
    const navigate = useNavigate();
    const [query, setQuery] = useState({
        filter: {
        },
        currentPage: 1,
        pageSize: 10,
        totalPages: 1,
        sortBy: "CheckInDate",
        sortOrder: "ASC"
    });

    const [queryReceipt, setQueryReceipt] = useState({
        filter: {
            ReservationId : ""
        },
    });

    const [reservations, setReservations] = useState([]);

    const formatDate = (dateString) => {
        const options = { day: 'numeric', month: 'numeric', year: 'numeric' };
        const date = new Date(dateString);
        return date.toLocaleDateString('en-GB', options);
    };

    useEffect(() => {
        fetch();
    }, [query.currentPage, query.sortBy, query.sortOrder]);

  

    const fetch = () => {
        api_reservation.getAllReservations(query).then((response) => {
            console.log(response);
            const [data, totalPages] = response;
            setReservations(data.map(reservation => ({
                ...reservation,
                checkInDate: formatDate(reservation.checkInDate),
                checkOutDate: formatDate(reservation.checkOutDate)
            })));
            setQuery({
                ...query,
                totalPages
            });
        });
    }

    const handlePageChange = (newPage) => {
        if (newPage >= 1) {
            setQuery({
                ...query,
                currentPage: newPage
            });
        }
    }


    const columns = [
        { key: "reservationNumber", label: "Reservation Number" },
        { key: "checkInDate", label: "Start Date" },
        { key: "checkOutDate", label: "End Date" },
        { key: "pricePerNight", label: "Price per night" },
        { key: "userEmail", label: "Email" }

    ]

    const handle = [
        {
            label: "Delete",
            onClick: (item) => handleDeleteReservation(item.id)
        },
        {
            label: "View",
            onClick: (item) => navigate(`/dashboard-reservation/view/${item.id}`)
        }
    ];

    useEffect(() => {
        console.log(queryReceipt);
    }, [queryReceipt]);

    const handleDeleteReservation = async (reservationId) => {
        try {
            const invoicesResponse = await api_receipt.getAllDashboardInvoice({
                filter: {
                    ReservationId: reservationId
                }
            });
            const invoices = invoicesResponse[0]; // Access the first element of the array
            console.log("Invoices:", invoices);
            console.log(reservationId);
            // Check if invoices array is not empty
            if (invoices.length > 0) {
                const invoiceId = invoices[0].id; // Assuming you only need the first invoice
                console.log("Invoice ID:", invoiceId);
    
                // Proceed with deleting the reservation
                const deleteResponse = await api_reservation.deleteReservation(reservationId, invoiceId);
                if (deleteResponse.status === 200) {
                    console.log("Reservation deleted successfully.");
                    return true;
                } else {
                    console.error("Error deleting reservation:", deleteResponse.statusText);
                    return false;
                }
            } else {
                console.error("No invoices found for reservationId:", reservationId);
                return false;
            }
        } catch (error) {
            console.error("Error deleting reservation:", error);
            return false;
        }
    };
    
    
    
    
    


    return (
        <div className="dashboard-reservations-page">
            <NavBar />
            <DataTable data={reservations} columns={columns} handle={handle} />
            <Paging totalPages={query.totalPages} currentPage={query.currentPage} onPageChange={handlePageChange} />
        </div>
    );
}

export default DashBoardReservationsPage;