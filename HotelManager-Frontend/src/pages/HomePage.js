import React from "react";
import { Outlet } from "react-router-dom";
import { Header } from "../components/Common/Header";
import { NavBar } from "../components/Common/NavBar";

export const HomePage = () => {
  return (
    <>
      <header className="header">
        <Header />
      </header>
      <main className="main">
        <Outlet />
      </main>
    </>
  );
};
