import React from "react";
import DateSelectAdmin from './FilterDateSelectAdmin.js';
import PriceRange from './FilterPriceRange.js';
import NumberOfBeds from './FilterNumberOfBeds.js';
import RoomTypes from './FilterRoomTypes.js';


const DashBoardRoomFilter = () => {

    return(
        <div className="DashBoardRoomFilter">
        <DateSelectAdmin/>
        <PriceRange minValue={0} maxValue={100} />
        <NumberOfBeds/>
        <RoomTypes/>
        </div>
    );
}

export default DashBoardRoomFilter;