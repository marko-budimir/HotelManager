import React from "react";
import { RoomList } from "../components/room/RoomList";
import { NavBar } from "../components/Common/NavBar";

export const RoomsPage = () => {
  return (
    <div className="rooms-page page">
      <NavBar />
      <div className="container">
        <RoomList />
      </div>
    </div>
  );
};
