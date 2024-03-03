import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getByIdRoom } from "../services/api_room";

export const RoomDetailsPage = () => {
  const { id } = useParams();
  const [room, setRoom] = useState(null);

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
    </div>
  );
};
