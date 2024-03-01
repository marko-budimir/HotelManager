import React from "react";
import { Outlet } from "react-router-dom";
import { Header } from "../components/Header";

export const HomePage = () => {
  return (
    <>
      <header>
        <Header />
      </header>
      <main>
        <Outlet />
      </main>
    </>
  );
};
