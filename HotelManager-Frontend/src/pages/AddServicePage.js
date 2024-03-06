import React from "react";
import { ServiceAddForm } from "../components/services/ServiceAddForm";
import { NavBar } from "../components/Common/NavBar";

export const AddServicePage = () => {
  return (
    <div className="add-service-page page">
      <NavBar />
      <div className="container">
        <ServiceAddForm />
      </div>
    </div>
  );
};
