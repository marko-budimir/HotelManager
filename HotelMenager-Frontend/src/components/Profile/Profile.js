import { useState, useEffect } from "react";
import { ProfileData } from "./ProfileData";
import { EditProfile } from "./EditProfile";
import { EditPassword } from "./EditPassword";

import { getUser, updatePassword, updateUser } from "../../services/api_user";

export const Profile = () => {
  const [user, setUser] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
  });

  const headers = {
    Authorization: "",
  };

  const fetchData = async () => {
    fetchToken();
    try {
      const userData = await getUser(headers);
      setUser(userData.data);
    } catch (error) {
      console.error("Error fetching user:", error);
    }
  };

  const fetchToken = () => {
    const token = localStorage.getItem("token");
    if (!token) {
      return;
    }
    headers.Authorization = `Bearer ${token}`;
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleProfileEdit = async (userData) => {
    fetchToken();
    console.log(userData, headers);
    try {
      await updateUser(userData, headers);
      fetchData();
    } catch (error) {
      console.error(error);
    }
  };

  const handlePasswordEdit = async (passwordData) => {
    fetchToken();
    try {
      await updatePassword(passwordData, headers);
      fetchData();
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="profile">
      <ProfileData user={user} />
      <div className="profile-forms">
        <EditProfile handleEdit={handleProfileEdit} />
        <EditPassword handleEdit={handlePasswordEdit} />
      </div>
    </div>
  );
};
