import axios from "axios";

const BASE_URL = "https://localhost:44327";

const getUser = async (headers) => {
  return await axios.get(`${BASE_URL}/api/user`, { headers: headers });
};

const createUser = async (registerData) => {
  try {
    const response = await axios.post(`${BASE_URL}/api/user`, registerData);
    return response.status;
  } catch (error) {
    throw error;
  }
};
const updateUser = async (userData, headers) => {
  return await axios.put(`${BASE_URL}/api/user`, userData, { headers: headers });
};

const updatePassword = async (passwordData, headers) => {
  return await axios.put(
    `${BASE_URL}/api/user/updatePassword`,
    {
      passwordOld: passwordData.passwordOld,
      passwordNew: passwordData.passwordNew,
    },
    {
      headers: headers,
    }
  );
};

const loginUser = async ({ email, password }) => {
  const loginData = new URLSearchParams();
  loginData.append("username", email);
  loginData.append("password", password);
  loginData.append("grant_type", "password");
  try {
    const response = await axios.post(`${BASE_URL}/login`, loginData);
    return response;
  } catch (error) {
    throw error;
  }
};

export { getUser, createUser, updateUser, updatePassword, loginUser };
