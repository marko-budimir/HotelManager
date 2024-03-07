import React from "react";
import ReviewAdd from "../components/review/ReviewAdd";
import { useParams } from "react-router-dom";
import { NavBar } from "../components/Common/NavBar";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export const AddReviewPage = () => {
  const { roomId } = useParams();
  return (
    <div className="add-review-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <ReviewAdd roomId={roomId} />
      </div>
    </div>
  );
};
