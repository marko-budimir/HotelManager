import React, { useState } from 'react';
import { DateRangePicker } from 'react-date-range';
import { addDays } from 'date-fns';
import 'react-date-range/dist/styles.css'; 
import 'react-date-range/dist/theme/default.css';

const DatePickerAdmin = ({ onChange, ...props }) => {
  const [state, setState] = useState([
    {
      startDate: new Date(),
      endDate: addDays(new Date(), 7),
      key: 'selection'
    }
  ]);

  const handleSelect = (item) => {
    setState([item.selection]);
    onChange && onChange(item);
  };

  return (
    <DateRangePicker
      onChange={handleSelect}
      ranges={state}
      {...props}
    />
  );
};

export default DatePickerAdmin;
