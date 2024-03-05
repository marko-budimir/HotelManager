import DataTable from "../components/Common/DataTable";
import { NavBar } from "../components/Common/NavBar";
import { useState, useEffect } from "react";
import api_reservation from "../services/api_reservation";
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
            onClick: (item) => deleteReservation(item.id)
        },
        {
            label: "View",
            onClick: (item) => navigate(`/dashboard-reservation/view/${item.id}`)
        }
    ];
    
    const deleteReservation = (reservationId) => {
        api_reservation.deleteReservation(reservationId).then(() => {
            setReservations(prevReservations => prevReservations.filter(reservation => reservation.id !== reservationId));
        }).catch(error => {
            console.error("Failed to delete reservation:", error);
        });
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