import axios from "axios";

const BASE_URL = "https://localhost:44327";

const getUser = () => { };

const createUser = async (registerData) => {

  try {
    const response = await axios.post(`${BASE_URL}/api/user`, registerData);
    return response.status;
  } catch (error) {
    throw error;
  }
};

const updateUser = () => { };

const updatePassword = () => { };

export const loginUser = async ({ email, password }) => {
  const loginData = new URLSearchParams();
  loginData.append('username', email);
  loginData.append('password', password);
  loginData.append('grant_type', 'password')
  try {
    const response = await axios.post(`${BASE_URL}/login`, loginData);
    return response;
  } catch (error) {
    throw error;
  }
}

export {
  getUser,
  createUser,
  updateUser,
  updatePassword
};
