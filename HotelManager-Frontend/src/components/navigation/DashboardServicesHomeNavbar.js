import React from "react";
import PriceRange from "../filter/FilterPriceRange";
import DashboardNavigation from "./DashboardNavigation";
import { Link } from "react-router-dom";
import { useServicesFilter } from "../../context/ServicesFilterContext";

export const DashboardServicesHomeNavbar = () => {
  const { filter, setFilter } = useServicesFilter();

  const handlePriceRangeChange = (e) => {
    const value = parseInt(e.target.value);
    setFilter((prev) => ({
      ...prev,
      [e.target.id]: value
    }))
  };


  return (
    <div className="navbar">
      <DashboardNavigation />
<<<<<<< HEAD
      <PriceRange minValue={filter.minPrice} maxValue={filter.maxPrice} onChange={handlePriceRangeChange} />
      <Link to="/add-service">Add service</Link>
=======
      <PriceRange />
      <Link to="/add-service" className="add-link">
        Add service
      </Link>
>>>>>>> 5c5f451 (add styling for most components and pages)
    </div>
  );
};
