import React from "react";
import { Radio, RadioGroup, FormControlLabel } from "@mui/material";

const FilterRoomType = ({ onChange }) => {
    const handleTypeChange = (event) => {
        onChange(event.target.value);
    };

    return (
        <div className="FilterRoomType">
            <label>Room Type:</label>
            <RadioGroup onChange={handleTypeChange}>
                <FormControlLabel value="single" control={<Radio />} label="Single" />
                <FormControlLabel value="double" control={<Radio />} label="Double" />
                <FormControlLabel value="suite" control={<Radio />} label="Suite" />
            </RadioGroup>
        </div>
    );
};

export default FilterRoomType;
