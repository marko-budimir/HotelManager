import React from "react";
import { DashboardRoomType } from "../components/roomType/DashboardRoomType";
import { NavBar } from "../components/Common/NavBar";
import { DashboardRoomTypeNavbar } from "../components/navigation/DashboardRoomTypeNavbar";

const DashboardRoomTypePage = () => {
  return (
    <div className="dashboard-room-type-page page">
      <DashboardRoomTypeNavbar />
      <div className="container">
        <DashboardRoomType />
      </div>
    </div>
  );
};
export default DashboardRoomTypePage;
