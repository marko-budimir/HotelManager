import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  getByIdService,
  updateService,
} from "../../services/api_hotel_service";

export const ServiceEditForm = () => {
  const navigate = useNavigate();
  const { serviceId } = useParams();
  const [service, setService] = useState({
    name: "",
    description: "",
    price: 0,
  });

  const fetchData = async () => {
    const res = await getByIdService(serviceId);
    const data = res.data;
    setService(data);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleChange = (e) => {
    setService({ ...service, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      console.log(serviceId, service);
      await updateService(serviceId, service);

      navigate("/dashboardServices");
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="service-edit">
      <h2 className="service-edit-heading">Edit Service</h2>
      <form className="service-edit-form" onSubmit={handleSubmit}>
        <label className="service-edit-form-label">Name:</label>
        <input
          className="service-edit-form-input"
          type="text"
          id="name"
          name="name"
          value={service.name}
          onChange={handleChange}
        />
        <label className="service-edit-form-label">Description:</label>
        <textarea
          className="service-edit-form-input"
          id="description"
          name="description"
          value={service.description}
          onChange={handleChange}
        />
        <label className="service-edit-form-label">Price:</label>
        <input
          className="service-edit-form-input"
          type="number"
          id="price"
          name="price"
          value={service.price}
          onChange={handleChange}
        />
        <input type="submit" value="Apply" />
      </form>
    </div>
  );
};
