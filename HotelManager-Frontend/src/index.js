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
import MyReservationsPage from "./pages/MyReservationsPage";
import "./style/style.css";
import { RoomsPage } from "./pages/RoomsPage";
import { RoomDetailsPage } from "./pages/RoomDetailsPage";
import { AddReviewPage } from "./pages/AddReviewPage";
import DashBoardRoomPage from "./pages/DashBoardRoomPage.js";
import DashBoardAddRoomPage from "./pages/DashBoardAddRoomPage.js";
import { AddRoomTypePage } from "./pages/AddRoomTypePage";
import { EditRoomTypePage } from "./pages/EditRoomTypePage";
import DashboardRoomTypePage from "./pages/DashboardRoomTypePage.js";
import { AddDiscountPage } from "./pages/AddDiscountPage.js";
import { EditDiscountPage } from "./pages/EditDiscountPage.js";
import DashboardDiscountPage from "./pages/DashboardDiscountPage.js";
import DashboardReceiptPage from "./pages/DashboardReceiptPage.js";
import DashboardReceiptEditPage from "./pages/DashboardReceiptEditPage.js";
import { DashboardServicesPage } from "./pages/DashboardServicesPage";
import { EditServicePage } from "./pages/EditServicePage";
import { AddServicePage } from "./pages/AddServicePage";
import DashBoardReservationsPage from "./pages/DashBoardReservationPage.js";

const router = createBrowserRouter(
  createRoutesFromElements(
    <>
      <Route path="/" element={<HomePage />} errorElement={<Error />}>
        <Route path="/" element={<RoomsPage />}></Route>
        <Route path="my-profile" element={<ProfilePage />}></Route>
        <Route path="/my-reservations" element={<MyReservationsPage />}></Route>
        <Route path="/room/:id" element={<RoomDetailsPage />}></Route>
        <Route path="/addreview/:roomId" element={<AddReviewPage />}></Route>

        <Route
          path="/dashBoardRoom/:id"
          element={<DashBoardRoomPage />}
        ></Route>
        <Route
          path="/dashBoardRoom/add"
          element={<DashBoardAddRoomPage />}
        ></Route>
        <Route path="/addroomtype" element={<AddRoomTypePage />}></Route>
        <Route
          path="/editroomtype/:roomId"
          element={<EditRoomTypePage />}
        ></Route>

        <Route
          path="/dashboard-roomtype/"
          element={<DashboardRoomTypePage />}
        ></Route>
        <Route
          path="/dashboard-roomtype/add"
          element={<AddRoomTypePage />}
        ></Route>
        <Route
          path="/dashboard-roomtype/:roomTypeId"
          element={<EditRoomTypePage />}
        ></Route>

        <Route
          path="/dashboard-discount/"
          element={<DashboardDiscountPage />}
        ></Route>
        <Route
          path="/dashboard-discount/add"
          element={<AddDiscountPage />}
        ></Route>
        <Route
          path="/dashboard-discount/:discountId"
          element={<EditDiscountPage />}
        ></Route>
        <Route
          path="/dashboardReceipt"
          element={<DashboardReceiptPage />}
        ></Route>
        <Route
          path="/dashboardReceipt/edit/:receiptId"
          element={<DashboardReceiptEditPage />}
        ></Route>
        <Route path="/addreview/:roomId" element={<AddReviewPage />}></Route>
        <Route
          path="/dashboardServices"
          element={<DashboardServicesPage />}
        ></Route>
        <Route
          path="/edit-service/:serviceId"
          element={<EditServicePage />}
        ></Route>
        <Route path="/add-service" element={<AddServicePage />}></Route>
        <Route path="/dashboard-discount/" element={<DashboardDiscountPage/>}></Route>
        <Route path="/dashboard-discount/add" element={<AddDiscountPage/>}></Route>
        <Route path="/dashboard-discount/:discountId" element={<EditDiscountPage/>}></Route>
        <Route path="/dashboardReceipt" element={<DashboardReceiptPage />}></Route>
        <Route path="/dashboardReceipt/edit/:receiptId" element={<DashboardReceiptEditPage />}></Route>
        <Route path="/dashboard-reservation/" element={<DashBoardReservationsPage/>}></Route>
        <Route path="/dashboard-reservation/view" element={<DashBoardReservationsPage/>}></Route>


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
