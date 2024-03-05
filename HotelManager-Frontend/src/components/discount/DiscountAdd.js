import React, { useState } from 'react';
import { createDiscount } from '../../services/api_discount';
import { useNavigate } from "react-router-dom";

const DiscountAdd = () => {
    const navigate = useNavigate();

  const [formData, setFormData] = useState({
    code: '',
    percent: 0,
    validFrom: '',
    validTo: ''
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (window.confirm("Are you sure you want to create this discount?")) {
      try {
        await createDiscount(formData);
        console.log("Discount added successfully!");
        navigate(`/dashboard-discount/`);
      } catch (error) {
        console.error("Error adding discount:", error);
      }
    }
  };

  return (
    <div>
      <h2>Add New Discount</h2>
      <form className='discount-add-form' onSubmit={handleSubmit}>
        <div>
          <label htmlFor="code">Code:</label>
          <input
            type="text"
            id="code"
            name="code"
            value={formData.code}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="percent">Percent:</label>
          <input
            type="number"
            id="percent"
            name="percent"
            value={formData.percent}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="validFrom">Valid From:</label>
          <input
            type="date"
            id="validFrom"
            name="validFrom"
            value={formData.validFrom}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="validTo">Valid To:</label>
          <input
            type="date"
            id="validTo"
            name="validTo"
            value={formData.validTo}
            onChange={handleChange}
          />
        </div>
        <button type="submit">Add Discount</button>
      </form>
    </div>
  );
};

export default DiscountAdd;
