import React, { useState } from 'react';

const PriceRange = () => {
  const [minValue, setMinValue] = useState(0);
  const [maxValue, setMaxValue] = useState(100);

  const handleMinChange = (event) => {
    const value = parseInt(event.target.value);
    setMinValue(value);
  };

  const handleMaxChange = (event) => {
    const value = parseInt(event.target.value);
    setMaxValue(value);
  };

  return (
    <div className="range-price">
      <div className="input-container">
        <label htmlFor="min">Min:</label>
        <input
          type="number"
          id="min"
          value={minValue}
          onChange={handleMinChange}
        />
      </div>
      <div className="input-container">
        <label htmlFor="max">Max:</label>
        <input
          type="number"
          id="max"
          value={maxValue}
          onChange={handleMaxChange}
        />
      </div>
    </div>
  );
};

export default PriceRange;