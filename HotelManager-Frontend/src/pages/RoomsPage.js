import React from "react";
import { RoomList } from "../components/room/RoomList";
import { NavBar } from "../components/Common/NavBar";

export const RoomsPage = () => {
  return (
    <div className="rooms-page">
      <NavBar />
      <RoomList />
    </div>
  );
};
