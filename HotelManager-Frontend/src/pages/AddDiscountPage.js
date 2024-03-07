import React from "react";
import { NavBar } from "../components/Common/NavBar";
import DiscountAdd from "../components/discount/DiscountAdd";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export const AddDiscountPage = () => {
  return (
    <div className="discount-page page">
      <DashboardEditViewNavbar />
      <div className="container"></div>
      <DiscountAdd />
    </div>
  );
};
