import { getAllServices } from "../services/api_hotel_service";
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import { NavBar } from "../components/Common/NavBar";
import DataTable from "../components/Common/DataTable";
import { deleteService } from "../services/api_hotel_service";

export const DashboardServicesPage = () => {
  const [services, setServices] = useState([]);
  const navigate = useNavigate();

  const fetchData = async () => {
    try {
      const serviceData = await getAllServices();
      const servicesData = serviceData.data.items;
      [...servicesData].map((service) => {
        service.price = service.price + "€";
      });
      setServices([...servicesData]);
    } catch (error) {
      console.error("Error fetching service:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const columns = [
    { key: "name", label: "Service name" },
    { key: "description", label: "Description" },
    { key: "price", label: "Price" },
  ];

  const handle = [
    {
      label: "Delete",
      onClick: async (row) => {
        await deleteService(row.id);
        fetchData();
      },
    },
    {
      label: "Edit",
      onClick: (row) => {
        navigate(`/edit-service/${row.id}`);
      },
    },
  ];

  return (
    <div className="service-list">
      <NavBar />
      <DataTable data={services} columns={columns} handle={handle} />
    </div>
  );
};
