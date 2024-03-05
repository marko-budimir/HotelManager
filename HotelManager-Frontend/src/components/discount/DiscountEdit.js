import React, { useState, useEffect } from "react";
import { getDiscountById, updateDiscount } from "../../services/api_discount";

const DiscountEdit = ({ discountId }) => {
  const [discount, setDiscount] = useState({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchDiscount = async () => {
      try {
        const response = await getDiscountById(discountId);
        setDiscount(response.data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching discount:", error);
      }
    };

    fetchDiscount();
  }, [discountId]);

  const handleChange = (e) => {
    setDiscount({ ...discount, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await updateDiscount(discountId, discount);
      console.log("Discount updated successfully!");
    } catch (error) {
      console.error("Error updating discount:", error);
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h2>Edit Discount</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="code">Code:</label>
          <input
            type="text"
            id="code"
            name="code"
            value={discount.code || ""}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="percent">Percentage:</label>
          <input
            type="number"
            id="percent"
            name="percent"
            value={discount.percent || ""}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="validFrom">Valid from:</label>
          <input
            type="date"
            id="validFrom"
            name="validFrom"
            value={discount.validFrom || ""}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="validTo">Valid to:</label>
          <input
            type="date"
            id="validTo"
            name="validTo"
            value={discount.validTo || ""}
            onChange={handleChange}
          />
        </div>
        <button type="submit">Update Discount</button>
      </form>
    </div>
  );
};

export default DiscountEdit;
