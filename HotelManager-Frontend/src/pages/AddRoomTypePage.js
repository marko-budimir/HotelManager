import React from "react";
import RoomTypeAdd from "../components/roomType/RoomTypeAdd";
import { NavBar } from "../components/Common/NavBar";

export const AddRoomTypePage = () => {
  return (
    <div className="add-room-type-page page">
      <NavBar />
      <div className="container">
        <RoomTypeAdd />
      </div>
    </div>
  );
};
