import React from "react";
import { NavBar } from "../components/Common/NavBar";
import { useParams } from "react-router-dom";
import RoomTypeEdit from "../components/roomType/RoomTypeEdit";

export const EditRoomTypePage = () => {
  const { roomTypeId } = useParams();
  return (
    <div className="edit-room-type-page page">
      <NavBar />
      <div className="container">
        <RoomTypeEdit roomId={roomTypeId} />
      </div>
    </div>
  );
};
