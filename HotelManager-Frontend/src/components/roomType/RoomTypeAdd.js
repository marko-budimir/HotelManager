import React, { useState } from "react";
import { createRoomType } from "../../services/api_room_type";
import { useNavigate } from "react-router-dom";

const RoomTypeAdd = () => {
  const [roomType, setRoomType] = useState({});
  const navigate = useNavigate();

  const handleChange = (e) => {
    setRoomType({ ...roomType, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const confirmed = window.confirm("Are you sure you want to add this room type?");
    if (confirmed) {
      try {
        await createRoomType(roomType);
        console.log("Room type added successfully!");
        navigate("/dashboard-roomtype");
      } catch (error) {
        console.error("Error adding room type:", error);
      }
    } else {
      console.log("Addition cancelled");
    }
  };

  return (
    <div>
      <h2>Add New Room Type</h2>
      <form className="roomtype-add-form" onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">Name:</label>
          <input
            type="text"
            id="name"
            name="name"
            value={roomType.name || ""}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="description">Description:</label>
          <textarea
            id="description"
            name="description"
            value={roomType.description || ""}
            onChange={handleChange}
          />
        </div>
        <button type="submit">Add Room Type</button>
      </form>
    </div>
  );
};

export default RoomTypeAdd;