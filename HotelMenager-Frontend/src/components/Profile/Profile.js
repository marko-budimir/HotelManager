import { useState } from "react";
import { ProfileData } from "./ProfileData";

export const Profile = () => {
  const [user, setUser] = useState({
    id: "73dd2485-b158-420a-86ca-599c3abba0aa",
    firstName: "Luka",
    lastName: "Hat",
    email: "john.doe@example.com",
    phone: "123456789",
  });
  return <ProfileData user={user} />;
};
