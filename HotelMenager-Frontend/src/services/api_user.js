import axios from "axios";

const BASE_URL = "https://localhost:44327";

const getUser = () => {};

const createUser = () => {};

const updateUser = () => {};

const updatePassword = () => {};

const loginUser = ({email, password}) => {
  try {
    axios.post(`https://localhost:44327/login`, loginData).then((response) => {
      return response.data;
    });
  } catch (error) {
    throw error;
  }
}

export default {
  getUser,
  createUser,
  updateUser,
  updatePassword,
  loginUser
};
