import React from "react";
import { Link } from "react-router-dom";
import DashboardNavigation from "./DashboardNavigation";

export const AddReviewNavbar = () => {
  return (
    <div className="navbar">
      <Link to="/my-reservations">Go back</Link>
    </div>
  );
};
