import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getByIdRoom } from "../services/api_room";
import { getAllDiscounts } from "../services/api_discount";
import Reviews from "../components/review/Reviews";

export const RoomDetailsPage = () => {
  const { id } = useParams();
  const [room, setRoom] = useState(null);
  const [discountCode, setDiscountCode] = useState('');
  const [discounts, setDiscounts] = useState([]);
  const [query, setQuery] = useState({
    filter: {
      code:''
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

  useEffect(() => {
    const fetchDiscounts = async () => {
      try {
        const [discountsData,totalPages] = await getAllDiscounts(query);
        console.log("DISCOUNT:",discountsData); 
        setDiscounts(discountsData);
        
      } catch (error) {
        console.error("Error fetching discounts:", error);
        setDiscounts([]);
      }
      console.log(discounts);
    };

    fetchDiscounts();
  }, [discountCode]);

  const handleApplyDiscount = () => {
    console.log('Discount code applied:', discountCode);
    setQuery({...query,
      filter:
      {
        code:discountCode
      }})
  };

  const handleReserve = () => {
    console.log('Room reserved');
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

      <input 
        type="text" 
        placeholder="Enter discount code" 
        value={discountCode} 
        onChange={(e) => setDiscountCode(e.target.value)}
      />
      <button onClick={handleApplyDiscount}>Apply</button>
      <br/>
      <button onClick={handleReserve}>Reserve</button>

      <Reviews id={id}/>
      
    </div>
  );
};
