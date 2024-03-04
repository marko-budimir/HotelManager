import React, { useState } from "react";
import api_room_type from '../../services/api_room_type';

const RoomTypeAdd = () => {
  const [roomType, setRoomType] = useState({});

  const handleChange = (e) => {
    setRoomType({...roomType,[e.target.name]: e.target.value});
    console.log(roomType);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
        console.log(roomType);
      await api_room_type.createRoomType(roomType);
      // Handle success, maybe redirect or show a success message
      console.log("Room type added successfully!");
    } catch (error) {
      // Handle error, maybe show an error message
      console.error("Error adding room type:", error);
    }
  };

  return (
    <div>
      <h2>Add New Room Type</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">Name:</label>
          <input
            type="text"
            id="name"
            name="name"
            value={roomType.name}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="description">Description:</label>
          <textarea
            id="description"
            name="description"
            value={roomType.description}
            onChange={handleChange}
          />
        </div>
        {/* Add inputs for other room type properties here */}
        <button type="submit">Add Room Type</button>
      </form>
    </div>
  );
};

export default RoomTypeAdd;
