import { useState } from "react";
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
    Authorization:
      "Bearer tzJfrMe0NWQORRY3l_OQqZ7VylsWARJHZy4EBTY4F7AkVzCYVonxka8m9mtIsJEtbrxtBPHM8UYZKi35zo66lGfgXXtvOfsJ7Lw4afl3OKTmjjUYfSvqn9L56n52XIMjGv2Rf2MpMZQvWs37nO4l5wL7S5rxbEEYIBo1UwAG8tT-N0lL4mPSkewNl5aRbVcQlVR99Z2g3Nx6IbGiWRVBMTj-cuYtdO7w7jfGOYy7j6O1ADXEU4B-aOd1KoEBECD4fTWTMGitBaOAVCMKwbqD5CHtrh1VRL8mB-oK_hwAhZkUrrgcXcceIvcx3K9u_nQ4AWt9Mxw1lY65X9LGhpS4nspgbfskyEw_es4wicAWfThhVtZAQQp-N6rC9-g9WmIy",
  };

  const fetchData = async () => {
    try {
      const userData = await getUser(headers);
      setUser(userData.data);
    } catch (error) {
      console.error("Error fetching user:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleProfileEdit = async (userData) => {
    try {
      await updateUser(userData, headers);
      fetchData();
    } catch (error) {
      console.error(error);
    }
  };

  const handlePasswordEdit = async (passwordData) => {
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
