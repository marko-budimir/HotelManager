import React from "react";
import RoomForm from "../components/room/RoomForm";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

const DashBoardRoomPage = () => {
  return (
    <div className="dashboard-add-room-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <h2>Add room</h2>
        <RoomForm />
      </div>
    </div>
  );
};
export default DashBoardRoomPage;
