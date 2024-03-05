import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import DataTable from "../Common/DataTable";
import { getAllRoomType } from "../../services/api_room_type";

export const DashboardRoomType = () => {
  const [roomTypes, setRoomTypes] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await getAllRoomType();
        console.log("Room types data:", response.data); // Log the data property
        if (response.data && response.data.items && Array.isArray(response.data.items)) {
          setRoomTypes([...response.data.items]);
        } else {
          console.error("Unexpected room types data format:", response.data);
        }
      } catch (error) {
        console.error("Error fetching room types:", error);
      }
    };
  
    fetchData();
  }, []);
  
  

  const columns = [
    { key: "name", label: "Roomtype name" },
    { key: "description", label: "Description" },
  ];

  const handle = [
    {
      label: "Delete",
      onClick: (row) => {
        navigate("/delete-service");
        console.log("Delete clicked for service:", row);
      },
    },
    {
      label: "Edit",
      onClick: (row) => {
        navigate(`/dashboard-roomtype/${row.id}`); // Navigate to the edit page with roomTypeId
      },
    },
  ];

  return (
    <div className="service-list">
      <DataTable data={roomTypes} columns={columns} handle={handle} />
    </div>
  );
};
