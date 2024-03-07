import React from "react";
import PriceRange from "../filter/FilterPriceRange";
import DashboardNavigation from "./DashboardNavigation";
import { Link } from "react-router-dom";

export const DashboardServicesHomeNavbar = () => {
  return (
    <div className="navbar">
      <DashboardNavigation />
      <PriceRange />
      <Link to="/add-service">Add service</Link>
    </div>
  );
};
