import React from 'react';

const SearchInput = ({ searchQuery, onChange }) => {

  return (
    <form className="FilterSearchQuery">
      <input
        type="text"
        value={searchQuery}
        onChange={onChange}
        placeholder="Search by email..."
      />
    </form>
  );
};

export default SearchInput;
