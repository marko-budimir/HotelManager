import React from "react";
import PriceRange from "./FilterPriceRange.js";
import IsPaidReceipt from "./FilterIsPaidReceipt.js";
import SearchQuery from "./FilterSearchQuery.js";
import { useReceiptFilter } from "../../context/ReceiptFilterContext.js";

const DashBoardReceiptFilter = () => {
  const { filter, setFilter } = useReceiptFilter();

  const handlePriceRangeChange = (e) => {
    const value = parseInt(e.target.value);
    setFilter((prev) => ({
      ...prev,
      [e.target.id]: value,
    }));
  };

  const handleIsPaidChange = (value) => {
    setFilter((prev) => ({
      ...prev,
      isPaid: value,
    }));
  };

  const handleSearchQueryChange = (e) => {
    setFilter((prev) => ({
      ...prev,
      userEmailQuery: e.target.value,
    }));
  };

  return (
    <div className="DashBoardReceiptFilter filter">
      <PriceRange minValue={0} maxValue={100} />
      <IsPaidReceipt />
      <SearchQuery />
    </div>
  );
};

export default DashBoardReceiptFilter;
