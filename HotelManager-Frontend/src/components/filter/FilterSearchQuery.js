import React, { useState } from 'react';

const SearchInput = ({ onSearch }) => {
  const [searchQuery, setSearchQuery] = useState('');

  const handleChange = (event) => {
    console.log(event.target.value);
    setSearchQuery(event.target.value);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    alert("Searching \t"+ searchQuery);
    //onSearch(searchQuery);
  };

  return (
    <form className="FilterSearchQuery" onSubmit={handleSubmit}>
      <input
        type="text"
        value={searchQuery}
        onChange={handleChange}
        placeholder="Search by email..."
      />
      <button type="submit">Search</button>
    </form>
  );
};

export default SearchInput;
