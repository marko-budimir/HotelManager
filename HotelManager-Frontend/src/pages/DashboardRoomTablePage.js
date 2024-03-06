import { NavBar } from "../components/Common/NavBar";
import { RoomList } from "../components/room/RoomList";

const DashboardRoomTablePage = () => {
    return(
        <div className="dashboard-room-table-page page">
      <NavBar />
      <div className="container">
        <RoomList />
      </div>
    </div>
    );
}

export default DashboardRoomTablePage;