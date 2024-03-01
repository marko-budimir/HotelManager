import React from "react";
import { Outlet } from "react-router-dom";
import { Header } from "../components/Header";
import DashBoardReservationFilter from '../components/filter/DashBoardReservationFilter.js'

export const HomePage = () => {
  return (
    <>
      <header>
        <Header />
      </header>
      <main>
        <DashBoardReservationFilter/>
        <Outlet />
      </main>
    </>
  );
};
