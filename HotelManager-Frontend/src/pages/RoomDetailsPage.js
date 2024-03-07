import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getByIdRoom } from "../services/api_room";
import Reviews from "../components/review/Reviews";

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
      </div>
      <Reviews id={id} />
    </div>
  );
};
