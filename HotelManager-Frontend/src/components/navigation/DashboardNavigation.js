import { Link } from "react-router-dom";

const DashboardNavigation = () => {
  return (
    <div className="dashboard-navigation">
      <h2>Dashboard</h2>
      <nav className="navigation">
        <Link to="/dashboardRoom">Rooms</Link>
        <Link to="/dashboardReceipt">Receipts</Link>
        <Link to="/dashboard-reservation">Reservations</Link>
        <Link to="/dashboardServices">Services</Link>
        <Link to="/dashboard-discount">Discounts</Link>
        <Link to="/dashboard-roomtype">Room types</Link>
      </nav>
    </div>
  );
};

export default DashboardNavigation;
