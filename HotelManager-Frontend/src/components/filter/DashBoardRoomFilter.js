import React from "react";
import DateSelectAdmin from "./FilterDateSelectAdmin.js";
import DatePickerUser from "./FilterDateSelectUser.js";
import PriceRange from "./FilterPriceRange.js";
import NumberOfBeds from "./FilterNumberOfBeds.js";
import RoomTypes from "./FilterRoomTypes.js";

const DashBoardRoomFilter = () => {
  return (
    <div className="DashBoardRoomFilter">
      <DatePickerUser />
      <PriceRange minValue={0} maxValue={100} />
      <NumberOfBeds />
      <RoomTypes />
    </div>
  );
};

export default DashBoardRoomFilter;
