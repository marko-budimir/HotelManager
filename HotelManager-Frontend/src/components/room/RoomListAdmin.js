import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getAllRooms } from "../../services/api_room";

export const RoomListAdmin = () => {
  const [rooms, setRooms] = useState([]);

  const fetchData = async () => {
    try {
      const roomData = await getAllRooms();
      const roomsData = roomData.data.items;
      setRooms([...roomsData]);
    } catch (error) {
      console.error("Error fetching room:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <div className="room-list">
      {rooms.map((room) => (
        <div key={room.id}>
          <Link to={`/dashBoardRoom/${room.id}`} className="room-link">
            <img src={room.imageUrl} alt="room" className="room-link-image" />
            <div className="room-link-info">
              <p className="room-link-number">Room {room.number}</p>
              <p className="room-link-price">{room.price}€/per night</p>
            </div>
          </Link>
        </div>
      ))}
    </div>
  );
};
