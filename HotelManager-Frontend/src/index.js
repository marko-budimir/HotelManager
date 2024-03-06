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
import AdminRouteGuard from "./components/authentication/AdminRouteGuard.js";

import { RoomsPage } from "./pages/RoomsPage";
import { RoomDetailsPage } from "./pages/RoomDetailsPage";
import { AddReviewPage } from "./pages/AddReviewPage";
import DashboardRoomEditPage from "./pages/DashboardRoomEditPage.js";
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
import ViewEditReservationPage from './pages/DashBoardReservationViewEdit.js'
import DashboardRoomTablePage from "./pages/DashboardRoomTablePage.js";

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
          path="/dashboardRoom"
          element={<AdminRouteGuard><DashboardRoomTablePage/></AdminRouteGuard>}
        ></Route> 
        <Route
          path="/dashBoardRoom/:id"
          element={<AdminRouteGuard><DashboardRoomEditPage /></AdminRouteGuard>}
        ></Route>
        <Route
          path="/dashBoardRoom/add"
          element={<AdminRouteGuard><DashBoardAddRoomPage /></AdminRouteGuard>}
        ></Route>
        <Route path="/addroomtype" element={<AdminRouteGuard><AddRoomTypePage /></AdminRouteGuard>}></Route>
        <Route
          path="/editroomtype/:roomId"
          element={<AdminRouteGuard><EditRoomTypePage /></AdminRouteGuard>}
        ></Route>

        <Route
          path="/dashboard-roomtype/"
          element={<AdminRouteGuard><DashboardRoomTypePage /></AdminRouteGuard>}
        ></Route>

        <Route
          path="/dashboard-roomtype/add"
          element={<AdminRouteGuard><AddRoomTypePage /></AdminRouteGuard>}
        ></Route>
        <Route
          path="/dashboard-roomtype/:roomTypeId"
          element={<AdminRouteGuard><EditRoomTypePage /></AdminRouteGuard>}
        ></Route>

        <Route
          path="/dashboard-discount/"
          element={<AdminRouteGuard><DashboardDiscountPage /></AdminRouteGuard>}
        ></Route>
        <Route
          path="/dashboard-discount/add"
          element={<AdminRouteGuard><AddDiscountPage /></AdminRouteGuard>}
        ></Route>
        <Route
          path="/dashboard-discount/:discountId"
          element={<AdminRouteGuard><EditDiscountPage /></AdminRouteGuard>}
        ></Route>

        <Route
          path="/dashboardReceipt"
          element={<AdminRouteGuard><DashboardReceiptPage /></AdminRouteGuard>}
        ></Route>
        <Route
          path="/dashboardReceipt/edit/:receiptId"
          element={<AdminRouteGuard><DashboardReceiptEditPage /></AdminRouteGuard>}
        ></Route>
        <Route path="/addreview/:roomId" element={<AddReviewPage />}></Route>

        <Route
          path="/dashboardServices"
          element={<AdminRouteGuard><DashboardServicesPage /></AdminRouteGuard>}
        ></Route>
        <Route
          path="/edit-service/:serviceId"
          element={<AdminRouteGuard><EditServicePage /></AdminRouteGuard>}
        ></Route>
        <Route path="/add-service" element={<AddServicePage />}></Route>
        <Route path="/dashboard-discount/" element={<DashboardDiscountPage/>}></Route>
        <Route path="/dashboard-discount/add" element={<AddDiscountPage/>}></Route>
        <Route path="/dashboard-discount/:discountId" element={<EditDiscountPage/>}></Route>
        <Route path="/dashboardReceipt" element={<DashboardReceiptPage />}></Route>
        <Route path="/dashboardReceipt/edit/:receiptId" element={<DashboardReceiptEditPage />}></Route>
        <Route path="/dashboard-reservation/" element={<DashBoardReservationsPage/>}></Route>
        <Route path="/dashboard-reservation/view/:id" element={<ViewEditReservationPage/>}></Route>


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
