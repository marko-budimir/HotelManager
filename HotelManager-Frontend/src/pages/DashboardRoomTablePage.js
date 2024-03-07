import { NavBar } from "../components/Common/NavBar";
import { DashboardRoomNavbar } from "../components/navigation/DashboardRoomNavbar";
import { RoomList } from "../components/room/RoomList";

const DashboardRoomTablePage = () => {
  return (
    <div className="dashboard-room-table-page page">
      <DashboardRoomNavbar />
      <div className="container">
        <RoomList />
      </div>
    </div>
  );
};

export default DashboardRoomTablePage;
