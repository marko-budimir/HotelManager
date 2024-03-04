import React from "react";
import ReactDOM from "react-dom/client";
import { HomePage } from "./pages/HomePage";
import Error from "./components/Common/Error";
import { ProfilePage } from "./pages/ProfilePage";
import {
  Route,
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements,
} from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";

import "./style/style.css";
import { RoomsPage } from "./pages/RoomsPage";
import { RoomDetailsPage } from "./pages/RoomDetailsPage";
import Review from "./components/review/Review";
import Reviews from "./components/review/Reviews";
import ReviewAdd from "./components/review/ReviewAdd";

const router = createBrowserRouter(
  createRoutesFromElements(
    <>
      <Route path="/" element={<HomePage />} errorElement={<Error />}>
        <Route path="/" element={<RoomsPage />}></Route>
        <Route path="my-profile" element={<ProfilePage />}></Route>
        <Route path="/room/:id" element={<RoomDetailsPage />}></Route>
        <Route path="/review" element={<Review rating="3" comment="beautiful" userFullName="toni" dateCreated="2.1.2023." id="43434"/>}></Route>
        <Route path="/reviews" element={<Reviews/>}></Route>
        <Route path="/addreview" element={<ReviewAdd/>}></Route>
      </Route>
      <Route
        path="/login"
        element={<LoginPage />}
        errorElement={<Error />}
      ></Route>
      <Route
        path="/register"
        element={<RegisterPage />}
        errorElement={<Error />}
      ></Route>
    </>
  )
);

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
