import DataTable from "../components/Common/DataTable";
import { NavBar } from "../components/Common/NavBar";
import { useState, useEffect } from "react";
import api_reservation from "../services/api_reservation";
import { getUser } from "../services/api_user";
import { useNavigate } from "react-router";

const MyReservationsPage = () => {
    const navigate = useNavigate();
    const [query, setQuery] = useState({
        filter: {
            userId: ""
        },
        currentPage: 1,
        pageSize: 10,
        sortBy: "CheckInDate",
        sortOrder: "ASC"
    });

    const [reservations, setReservations] = useState([]);

    useEffect(() => {
        try {
            getUser().then((user) => {
                setQuery({
                    ...query,
                    filter: {
                        userId: user.data.id
                    }
                });
            });
        } catch (error) {
            console.error(error);
        }
    }, []);

    const formatDate = (dateString) => {
        const options = { day: 'numeric', month: 'numeric', year: 'numeric' };
        const date = new Date(dateString);
        return date.toLocaleDateString('en-GB', options);
    };

    useEffect(() => {
        if (!query.filter.userId) return;
        api_reservation.getAllReservations(query).then((data) => {
            console.log("data", data);
            setReservations(data.map(reservation => ({
                ...reservation,
                checkInDate: formatDate(reservation.checkInDate),
                checkOutDate: formatDate(reservation.checkOutDate)
            })));
        });
    }, [query]);


    const columns = [
        { key: "roomNumber", label: "Room Number" },
        { key: "checkInDate", label: "Start Date" },
        { key: "checkOutDate", label: "End Date" },
        { key: "pricePerNight", label: "Price per night" }
    ]

    const handle = [
        {
            label: "Review it!",
            onClick: (item) => navigate(`/addreview/${item.id}`)
        }
    ]

    return (
        <div className="my-reservations-page">
            <NavBar />
            <DataTable data={reservations} columns={columns} handle={handle} />
        </div>
    );
}

export default MyReservationsPage;