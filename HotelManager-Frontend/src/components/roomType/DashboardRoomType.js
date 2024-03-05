import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import DataTable from "../Common/DataTable";
import { deleteRoomType, getAllRoomType } from "../../services/api_room_type";

export const DashboardRoomType = () => {
  const [roomTypes, setRoomTypes] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await getAllRoomType();
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
        const confirmed = window.confirm("Are you sure you want to delete this room type?");
        if (confirmed) 
        {
          deleteRoomType(row.id)
            .then(() => {
              console.log("Delete successful");
              window.location.reload();
            })
            .catch((error) => {
              console.error("Error deleting room type:", error);
            });
        } else 
        {
          console.log("Delete cancelled");
        }
      },
    },
    {
      label: "Edit",
      onClick: (row) => {
        navigate(`/dashboard-roomtype/${row.id}`);
      },
    },
  ];

  return (
    <div className="service-list">
      <DataTable data={roomTypes} columns={columns} handle={handle} />
    </div>
  );
};
