import React from "react";
import { ServiceAddForm } from "../components/services/ServiceAddForm";
import { NavBar } from "../components/Common/NavBar";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export const AddServicePage = () => {
  return (
    <div className="add-service-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <ServiceAddForm />
      </div>
    </div>
  );
};
