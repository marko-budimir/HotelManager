import React from "react";
import { NavBar } from "../components/Common/NavBar";
import DiscountAdd from "../components/discount/DiscountAdd";

export const AddDiscountPage = () => {
  return (
    <div className="discount-page page">
      <NavBar />
      <div className="container"></div>
      <DiscountAdd />
    </div>
  );
};