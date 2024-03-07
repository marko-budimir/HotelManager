import React from "react";
import { Profile } from "../components/Profile/Profile";
import { NavBar } from "../components/Common/NavBar";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export const ProfilePage = () => {
  return (
    <div className="profile-page page">
      <NavBar />
      <div className="container">
        <Profile />
      </div>
    </div>
  );
};
