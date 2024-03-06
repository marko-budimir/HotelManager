import React from "react";
import { NavBar } from "../components/Common/NavBar";
import { DashboardDiscount } from "../components/discount/DashboardDiscount";

const DashboardDiscountPage = () => {
  return (
    <div className="dashboard-discount-page page">
      <NavBar />
      <div className="container">
        <DashboardDiscount />
      </div>
    </div>
  );
};
export default DashboardDiscountPage;
