import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import { format } from "date-fns";
import DataTable from "../Common/DataTable";
import { deleteDiscount, getAllDiscounts } from "../../services/api_discount";

export const DashboardDiscount = () => {
  const [discounts, setDiscounts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await getAllDiscounts();
        if (response.data && response.data.items && Array.isArray(response.data.items)) {
          const formattedDiscounts = response.data.items.map(discount => ({
            ...discount,
            validFrom: format(new Date(discount.validFrom), 'dd-MM-yyyy'),
            validTo: format(new Date(discount.validTo), 'dd-MM-yyyy')
          }));
          setDiscounts(formattedDiscounts);
        } else {
          console.error("Unexpected discount data format:", response.data);
        }
      } catch (error) {
        console.error("Error fetching room types:", error);
      }
    };
  
    fetchData();
  }, []);
  
  const columns = [
    { key: "code", label: "Code" },
    { key: "percent", label: "Percentage" },
    { key: "validFrom", label: "Valid From" },
    { key: "validTo", label: "Valid To" },
  ];

  const handle = [
    {
      label: "Delete",
      onClick: (row) => {
        const confirmed = window.confirm("Are you sure you want to delete this discount?");
        if (confirmed) 
        {
          deleteDiscount(row.id)
            .then(() => {
              console.log("Delete successful");
              window.location.reload();
            })
            .catch((error) => {
              console.error("Error deleting discount:", error);
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
        navigate(`/dashboard-discount/${row.id}`);
      },
    },
  ];

  return (
    <div className="discount-list">
      <DataTable data={discounts} columns={columns} handle={handle} />
    </div>
  );
};
