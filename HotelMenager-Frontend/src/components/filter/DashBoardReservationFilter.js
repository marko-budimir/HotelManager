import React from "react";
import DateSelectUser from './FilterDateSelectUser.js';
import PriceRangeSlider from './FilterPriceRange.js';
import SearchQuery from './FilterSearchQuery.js'

const DashBoardReservationFilter = () => {

    return(
        <div className="DashBoardReservationFilter">
        <DateSelectUser/>
        <PriceRangeSlider minValue={0} maxValue={100} />
        <SearchQuery/>
        </div>
    );
}

export default DashBoardReservationFilter;
