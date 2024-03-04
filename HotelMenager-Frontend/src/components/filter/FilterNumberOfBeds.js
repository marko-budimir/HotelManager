import React, { useState } from "react";

const FilterNumberOfBeds = ({isDisabled=false}) => {
    const [numberOfBeds, setNumberOfBeds] = useState('');

    const handleChange = (event) => {
        setNumberOfBeds(event.target.value);    }

    return(
        <label className="NumberOfBedsRow">
            <span>Number of beds:</span>
            <br/>
            <input
                type="text"
                value={numberOfBeds}
                disabled={isDisabled}
                onChange={handleChange}
            />
            <br/><br/>
        </label>
    );
}

export default FilterNumberOfBeds;
