import axios from "axios";

const BASE_URL = "https://localhost:44327/api/room";

const getAllRooms = async () => {
  return await axios.get(`${BASE_URL}`);
};

const getByIdRoom = async (id) => {
  return await axios.get(`${BASE_URL}/${id}`);
};

export { getAllRooms, getByIdRoom };
