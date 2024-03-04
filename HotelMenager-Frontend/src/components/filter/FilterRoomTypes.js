import React, { useState, useEffect } from 'react';
import { Radio, RadioGroup, FormControlLabel } from "@mui/material";
import { getAllRoomType } from '../../services/api_room_type';

const FilterRoomType = ({ isDisabled, onChangeHandle }) => {
    const [roomTypes, setRoomTypes] = useState([]);
    const [selectedRoomType, setSelectedRoomType] = useState('');

    useEffect(() => {
        getAllRoomType()
          .then(response => {
            setRoomTypes(response.data.items);
          })
          .catch(error => {
            console.error("Error fetching room types:", error);
          });
    }, []);

    const handleRoomTypeChange = (event) => {
        const selectedType = event.target.value;
        setSelectedRoomType(selectedType);
        onChangeHandle(selectedType);

    };

    return (
        <div className="FilterRoomType">
            <label>Room Type:</label>
            <RadioGroup value={selectedRoomType} onChange={handleRoomTypeChange}>
                {roomTypes.map((roomType, index) => (
                    <FormControlLabel key={index} value={roomType.id} control={<Radio disabled={isDisabled} />} label={roomType.name} />
                ))}
            </RadioGroup>
        </div>
    );
};

export default FilterRoomType;
