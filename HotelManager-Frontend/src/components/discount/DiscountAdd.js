import React, { useState } from 'react';
import { createDiscount } from '../../services/api_discount';

const DiscountAdd = () => {
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
    try {
      await createDiscount(formData);
      console.log("Discount added successfully!");
    } catch (error) {
      console.error("Error adding discount:", error);
    }
  };

  return (
    <div>
      <h2>Add New Discount</h2>
      <form onSubmit={handleSubmit}>
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
