import React from "react";
import PriceRange from "./FilterPriceRange.js";
import IsPaidReceipt from "./FilterIsPaidReceipt.js";
import SearchQuery from "./FilterSearchQuery.js";

const DashBoardReceiptFilter = () => {
  return (
    <div className="DashBoardReceiptFilter">
      <PriceRange minValue={0} maxValue={100} />
      <IsPaidReceipt />
      <SearchQuery />
    </div>
  );
};

export default DashBoardReceiptFilter;
