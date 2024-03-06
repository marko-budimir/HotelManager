import React from "react";
import RoomForm from "../components/room/RoomForm";
import { NavBar } from "../components/Common/NavBar";

const DashBoardRoomPage = () => {
  return (
    <div className="dashboard-room-page page">
      <NavBar />
      <div className="container">
        <RoomForm />
      </div>
    </div>
  );
};
export default DashBoardRoomPage;
