import React from "react";
import RoomTypeAdd from "../components/roomType/RoomTypeAdd";
import { NavBar } from "../components/Common/NavBar";

export const AddRoomTypePage = () =>{
    return(
        <div>
            <NavBar/>
            <RoomTypeAdd/>
        </div>
    )
}