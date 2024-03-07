import React from "react";
import RoomForm from "../components/room/RoomForm";
import { NavBar } from "../components/Common/NavBar";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

const DashboardRoomEditPage = () => {
  return (
    <div className="dashboard-room-edit-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <RoomForm />
      </div>
    </div>
  );
};
export default DashboardRoomEditPage;
