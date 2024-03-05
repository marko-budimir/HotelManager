import React from "react";

export const ServiceEditForm = () => {
  return (
    <div className="service-edit">
      <h2 className="service-edit-heading">Edit Service</h2>
      <form className="service-edit-form">
        <label className="service-edit-form-label">Name:</label>
        <input
          className="service-edit-form-input"
          placeholder="Enter service name"
          type="text"
        />
        <label className="service-edit-form-label">Description:</label>
        <textarea
          className="service-edit-form-input"
          placeholder="Enter service description"
        />
        <label className="service-edit-form-label">Price:</label>
        <input
          className="service-edit-form-input"
          placeholder="Enter service price"
          type="number"
        />
      </form>
    </div>
  );
};
