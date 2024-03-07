import { NavBar } from "../components/Common/NavBar";
import { DashboardHomeRoomNavbar } from "../components/navigation/DashboardHomeRoomNavbar";
import { RoomList } from "../components/room/RoomList";

const DashboardRoomTablePage = () => {
  return (
    <div className="dashboard-room-table-page page">
      <DashboardHomeRoomNavbar />
      <div className="container">
        <RoomList />
      </div>
    </div>
  );
};

export default DashboardRoomTablePage;
