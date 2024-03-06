import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getByIdRoom } from "../services/api_room";
import { getAllDiscounts } from "../services/api_discount";
import apiReservation from '../services/api_reservation';
import Reviews from "../components/review/Reviews";

export const RoomDetailsPage = () => {
  const { id } = useParams();
  const [room, setRoom] = useState(null);
  const [discount, setDiscount] = useState(['']);
  const [discountCode,setDiscountCode] = useState('');
  const [query, setQuery] = useState({
    filter: {
      code: ''
    },
    currentPage: 1,
    pageSize: 1,
    sortBy: "DateCreated",
    sortOrder: "ASC",
  });

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
  


  const handleApplyDiscount = () => {
    console.log('Discount code applied:', discountCode);
    setQuery({
      ...query,
      filter:
      {
        code: discountCode
      }
    })
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
        checkInDate: "2025-03-07T09:00:06.2932106+01:00",
        checkOutDate: "2025-03-07T09:00:06.2932106+01:00"
    };

    console.log(reservationData);

    // Call the API to create the reservation
    // apiReservation.createReservation(reservationData);
};

  useEffect(() => {
    const fetchDiscounts = async () => {
      try {
        const [discountsData,totalPages] = await getAllDiscounts(query);
        console.log("DISCOUNT:",discountsData); 
        setDiscount(discountsData);
        
      } catch (error) {
        console.error("Error fetching discounts:", error);
        setDiscount([]);
      }
      console.log(discount);
    };

    fetchDiscounts();
  }, [discountCode]);



  if (!room) {
    return <div>Loading...</div>;
  }

  return (
    <div className="room-detail">
      <div className="room-info-section">
        <img src={room.imageUrl} alt="room" className="room-detail-image" />
        <div className="room-detail-info">
          <p className="room-detail-number">Room number: {room.number}</p>
          <p className="room-detail-number">Price per day: {room.price}€</p>
          <p className="room-detail-number">
            Number of beds: {room.bedCount}🛏️
          </p>
          <p className="room-detail-number">Room type: {room.typeName}</p>
        </div>
        <div className="discount-reservation-form">
        <input
            type="text"
            placeholder="Enter discount code"
            value={discountCode}
            onChange={(e) => setDiscountCode(e.target.value)}
          />
          <button onClick={handleApplyDiscount}>Apply</button>
          <br />
          <button onClick={handleReserve}>Reserve</button>
        </div>
      </div>
      <Reviews id={id} />
    </div>
  );
};
