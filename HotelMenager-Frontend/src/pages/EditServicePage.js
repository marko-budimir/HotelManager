import React from "react";
import { NavBar } from "../components/Common/NavBar";
import { ServiceEditForm } from "../components/services/ServiceEditForm";

export const EditServicePage = () => {
  return (
    <div className="edit-service-page">
      <NavBar />
      <ServiceEditForm />
    </div>
  );
};
