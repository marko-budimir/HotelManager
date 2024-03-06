import React from "react";
import { DashboardRoomType } from "../components/roomType/DashboardRoomType";
import { NavBar } from "../components/Common/NavBar";

const DashboardRoomTypePage = () => {
  return (
    <div className="dashboard-room-type-page page">
      <NavBar />
      <div className="container">
        <DashboardRoomType />
      </div>
    </div>
  );
};
export default DashboardRoomTypePage;
