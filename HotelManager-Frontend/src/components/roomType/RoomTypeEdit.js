import React, { useState, useEffect } from "react";
import { getByIdRoomType, updateRoomType } from "../../services/api_room_type";
import { useNavigate } from "react-router-dom";

const RoomTypeEdit = ({ roomId }) => {
  const [roomType, setRoomType] = useState({});
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchRoomType = async () => {
      try {
        const response = await getByIdRoomType(roomId);
        setRoomType(response.data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching room type:", error);
      }
    };

    fetchRoomType();
  }, [roomId]);

  const handleChange = (e) => {
    setRoomType({ ...roomType, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const confirmed = window.confirm("Are you sure you want to update this room type?");
    if (confirmed) {
      try 
      {
        await updateRoomType(roomId, roomType);
        console.log("Room type updated successfully!");
        navigate(`/dashboard-roomtype`);
      } catch (error) 
      {
        console.error("Error updating room type:", error);
      }
    } else 
    {
      console.log("Update cancelled");
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
        <button type="submit">Update Room Type</button>
      </form>
    </div>
  );
};

export default RoomTypeEdit;
