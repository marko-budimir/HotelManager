import React from "react";

export const ServiceAddForm = () => {
  return (
    <div className="service-add">
      <h2 className="service-add-heading">Add Service</h2>
      <form className="service-add-form">
        <label className="service-add-form-label">Name:</label>
        <input
          className="service-add-form-input"
          placeholder="Enter service name"
          type="text"
        />
        <label className="service-add-form-label">Description:</label>
        <textarea
          className="service-add-form-input"
          placeholder="Enter service description"
        />
        <label className="service-add-form-label">Price:</label>
        <input
          className="service-add-form-input"
          placeholder="Enter service price"
          type="number"
        />
      </form>
    </div>
  );
};
