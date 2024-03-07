import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getAllRooms } from "../../services/api_room";

export const RoomList = () => {
  const [rooms, setRooms] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const fetchData = async (pageNumber) => {
    try {
      const roomData = await getAllRooms(pageNumber);
      const roomsData = roomData.data.items;
      const paging = roomData.data;
      console.log("room data:", roomData.data);
      setRooms([...roomsData]);
      setTotalPages(paging.totalPages);
    } catch (error) {
      console.error("Error fetching room:", error);
    }
  };

  useEffect(() => {
    fetchData(currentPage);
  }, [currentPage]);

  return (
    <div className="room-list">
      {rooms.map((room) => (
        <div key={room.id}>
          <Link to={`/room/${room.id}`} className="room-link">
            <img src={room.imageUrl} alt="room" className="room-link-image" />
            <div className="room-link-info">
              <p className="room-link-number">Room {room.number}</p>
              <p className="room-link-price">{room.price}€/per night</p>
            </div>
          </Link>
        </div>
      ))}
      <div className="pagination">
        {Array.from({ length: totalPages }).map((_, index) => (
          <button key={index} onClick={() => setCurrentPage(index + 1)}>
            {index + 1}
          </button>
        ))}
      </div>
    </div>
  );
};
