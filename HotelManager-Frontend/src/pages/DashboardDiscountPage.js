import React from "react";
import { NavBar } from "../components/Common/NavBar";
import { DashboardDiscount } from "../components/discount/DashboardDiscount";
import { DashboardDiscountsNavbar } from "../components/navigation/DashboardDiscountsNavbar";

const DashboardDiscountPage = () => {
  return (
    <div className="dashboard-discount-page page">
      <DashboardDiscountsNavbar />
      <div className="container">
        <DashboardDiscount />
      </div>
    </div>
  );
};
export default DashboardDiscountPage;
