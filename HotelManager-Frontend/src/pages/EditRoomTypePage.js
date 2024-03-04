import React from "react";
import { NavBar } from "../components/Common/NavBar";
import { useParams } from "react-router-dom";
import RoomTypeEdit from "../components/roomType/RoomTypeEdit";

export const EditRoomTypePage = () =>{
    const { roomId } = useParams();
    return(
        <div>
            <NavBar/>
            <RoomTypeEdit roomId={roomId}/>
        </div>
    )
}