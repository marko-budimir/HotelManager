import React from "react";
import PriceRange from "../filter/FilterPriceRange";
import { Link } from "react-router-dom";
import DashboardNavigation from "./DashboardNavigation";

export const DashboardHomeDiscountsNavbar = () => {
  return (
    <div className="navbar">
      <DashboardNavigation />
      <PriceRange />
      <Link to="/dashboard-discount/add">Add discount</Link>
    </div>
  );
};
