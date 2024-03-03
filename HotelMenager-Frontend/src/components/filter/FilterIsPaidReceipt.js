import React from "react";
import { Radio, RadioGroup, FormControlLabel } from "@mui/material";

const FilterIsPaid = ({ onChange }) => {
    const handleTypeChange = (event) => {
        onChange(event.target.value);
    };

    return (
        <div className="FilterIsPaid">
            <label>Is paid:</label>
            <RadioGroup onChange={handleTypeChange}>
                <FormControlLabel value="Yes" control={<Radio />} label="Yes" />
                <FormControlLabel value="No" control={<Radio />} label="No" />
            </RadioGroup>
        </div>
    );
};

export default FilterIsPaid;