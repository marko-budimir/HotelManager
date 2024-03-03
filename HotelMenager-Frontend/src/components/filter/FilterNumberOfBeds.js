import React from "react";

const FilterNumberOfBeds = () => {

    const handleChange = (event) => {
        console.log(event.target.value);
    }

    return(
        <label className="NumberOfBedsRow">
                <span>Number of beds:</span>
        <div className="FilterNumberOfBeds">
            <select onChange={handleChange}>
                <option value="1">1</option>
                <option value="2">2</option>
                <option value="3">3</option>
                <option value="4">4</option>
                <option value="5">5</option>
            </select>
        </div>
        </label>
    );
}

export default FilterNumberOfBeds;
