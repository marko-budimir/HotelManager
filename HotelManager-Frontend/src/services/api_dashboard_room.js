import { get, post, put } from "./api_base";

const URL_PATH = "/api/DashBoardRoom";

const updateDashboardRoom = (roomId, roomData) => {
  return put(`${URL_PATH}/${roomId}`, roomData);
};

const getAllDashboardRooms = (filter) => {
  return get(URL_PATH,filter);
};

const getDashboardRoomUpdateById = (roomId) => {
  return get(`${URL_PATH}/${roomId}`);
};


const createDashboardRoom = (roomData) => {
  return post(URL_PATH, roomData);
};

export{
  createDashboardRoom,
  updateDashboardRoom,
  getAllDashboardRooms,
  getDashboardRoomUpdateById,
};
