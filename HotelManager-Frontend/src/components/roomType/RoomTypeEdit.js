import React, { useState, useEffect } from "react";
import api_room_type from '../../services/api_room_type';

const RoomTypeEdit = ({ roomId }) => {
  const [roomType, setRoomType] = useState({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchRoomType = async () => {
      try {
        const response = await api_room_type.getByIdRoomType(roomId);
        setRoomType(response.data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching room type:", error);
      }
    };

    fetchRoomType();
  }, [roomId]);

  const handleChange = (e) => {
    setRoomType({...roomType, [e.target.name]: e.target.value});
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await api_room_type.updateRoomType(roomId, roomType);
      console.log("Room type updated successfully!");
    } catch (error) {
      console.error("Error updating room type:", error);
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h2>Edit Room Type</h2>
      <form onSubmit={handleSubmit}>
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
        {/* Add inputs for other room type properties here */}
        <button type="submit">Update Room Type</button>
      </form>
    </div>
  );
};

export default RoomTypeEdit;
