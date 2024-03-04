import React from "react";
import ReviewAdd from "../components/review/ReviewAdd";
import { useParams } from "react-router-dom";

export const AddReviewPage = () =>{
    const { roomId } = useParams();
    return(
        <div>
            <ReviewAdd roomId={roomId}/>
        </div>
    )
}