import React, { useState } from "react";

const FilterNumberOfBeds = ({ isDisabled = false }) => {
  const [numberOfBeds, setNumberOfBeds] = useState("");

  const handleChange = (event) => {
    setNumberOfBeds(event.target.value);
  };

  return (
    <label className="number-of-beds-row filter-label">
      <span>Number of beds:</span>
      <input
        className="filter-input number-of-beds-input"
        type="number"
        value={numberOfBeds}
        disabled={isDisabled}
        onChange={handleChange}
      />
    </label>
  );
};

export default FilterNumberOfBeds;
