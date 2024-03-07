import React, { useEffect, useState } from "react";
import { useLocation, useParams } from "react-router-dom";
import { getByIdRoom } from "../services/api_room";
import { getAllDiscounts } from "../services/api_discount";
import { formatReservationDate } from "../common/HelperFunctions";
import apiReservation from '../services/api_reservation';
import Reviews from "../components/review/Reviews";

export const RoomDetailsPage = () => {
  const { id } = useParams();
  const [room, setRoom] = useState(null);
  const [discount, setDiscount] = useState(['']);
  const [query, setQuery] = useState({
    filter: {
      code: ''
    },
    currentPage: 1,
    pageSize: 1,
    sortBy: "DateCreated",
    sortOrder: "ASC",
  });
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const startDate = queryParams.get("startDate");
  const endDate = queryParams.get("endDate");
/*
  console.log(startDate,"   ",endDate);
  console.log(formatReservationDate(startDate,13));
  console.log(formatReservationDate(endDate,10));
*/
  useEffect(() => {
    const fetchRoom = async () => {
      try {
        const roomData = await getByIdRoom(id);
        setRoom(roomData.data);
      } catch (error) {
        console.error("Error fetching room:", error);
      }
    };
  
    fetchRoom();
  }, [id]);
  

  useEffect(() => {
    

    
  }, [query]);

  const fetchDiscounts = async () => {
    try {
      const [discountsData, totalPages] = await getAllDiscounts(query);
      setDiscount(discountsData);
    } 
    catch (error) {
      console.error("Error fetching discounts:", error);
      setDiscount([]);
    }

  };

  const handleQueryChange = (discountCode) => {
    setQuery({...query, filter: { code: discountCode }})
  };

  const handleReserve = () => {
    console.log('Discount:', discount); 
    const appliedDiscount = discount[0]; 
    if (appliedDiscount) {
        console.log('Discount Code:', appliedDiscount.code);
    } else {
        console.log('No discounts found.');
    }
    
    const reservationData = {
        discountId: appliedDiscount ? appliedDiscount.id : null,
        roomId: room.id,
        pricePerNight: room.price,
        checkInDate: formatReservationDate(startDate,13),
        checkOutDate: formatReservationDate(endDate,10)
    };

    console.log(reservationData);
    apiReservation.createReservation(reservationData);
};

  if (!room) {
    return <div>Loading...</div>;
  }

  return (
    <div className="roomDetail">
      <img src={room.imageUrl} alt="room" />
      <p>Room number: {room.number}</p>
      <p>Price per day: {room.price}€</p>
      <p>Number of beds: {room.bedCount}🛏️</p>
      <p>Room type: {room.typeName}</p>
      
      <div className="discount-reservation-form">
        
      <p>Reservaton start date: {startDate}</p>
      <p>Reservation end date: {endDate}</p>
        
      <input
        type="text"
        placeholder="Enter discount code"
        value={query.filter.code}
        onChange={(e) => handleQueryChange(e.target.value)}
      />
      <button onClick={fetchDiscounts}>Apply</button>
      <br />
      <button onClick={handleReserve}>Reserve</button>
      </div>
      <Reviews id={id} />
    </div>
  );
};
