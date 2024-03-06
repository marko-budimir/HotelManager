import React from "react";
import RoomForm from "../components/room/RoomForm";
import { NavBar } from "../components/Common/NavBar";

const DashboardRoomEditPage = () => {
  return (
    <div className="dashboard-room-edit-page page">
      <NavBar />
      <div className="container">
        <RoomForm />
      </div>
    </div>
  );
};
export default DashboardRoomEditPage;
