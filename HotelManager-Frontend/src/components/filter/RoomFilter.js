import React from "react";
import DateSelectUser from './FilterDateSelectUser.js';
import PriceRange from './FilterPriceRange.js';
import NumberOfBeds from './FilterNumberOfBeds.js';
import RoomTypes from './FilterRoomTypes.js';

const RoomFilter = () => {

    return(
        <div className="RoomFilter">
        <DateSelectUser/>
        <PriceRange minValue={0} maxValue={100} />
        <NumberOfBeds/>
        <RoomTypes/>
        </div>
    );
}

export default RoomFilter;