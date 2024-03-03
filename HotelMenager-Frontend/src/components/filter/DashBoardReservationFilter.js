import React from "react";
import DateSelectUser from './FilterDateSelectUser.js';
import PriceRange from './FilterPriceRange.js';
import SearchQuery from './FilterSearchQuery.js'

const DashBoardReservationFilter = () => {

    return(
        <div className="DashBoardReservationFilter">
        <DateSelectUser/>
        <PriceRange minValue={0} maxValue={100} />
        <SearchQuery/>
        </div>
    );
}

export default DashBoardReservationFilter;
