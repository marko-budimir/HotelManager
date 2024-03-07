import React, { useState } from 'react';
import DatePicker from 'react-datepicker';
import "react-datepicker/dist/react-datepicker.css";
import {formatDate} from '../../common/HelperFunctions';

const DatePickerUser = () => {
  const [startDate, setStartDate] = useState(new Date());
  const [endDate, setEndDate] = useState(null);

  const today = new Date();


  const onChange = (dates) => {
    const [start, end] = dates;
    setStartDate(start);
    setEndDate(end);
    console.log(formatDate(start),"  ",formatDate(end));
  };

  return (
    <DatePicker
    selected={startDate}
    onChange={onChange}
    startDate={startDate}
    minDate={today}
    endDate={endDate}
    selectsRange
    selectsDisabledDaysInRange
    inline
    />
  );
};

export default DatePickerUser;
