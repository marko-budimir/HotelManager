import { get } from "./api_base";

const URL_PATH = "/api/RoomType";

const getAllRoomType = () => {
  return get(`${URL_PATH}/`);
};

const getByIdRoomType = () => {};

const createRoomType = () => {};

const updateRoomType = () => {};

export  {
  getAllRoomType,
  getByIdRoomType,
  createRoomType,
  updateRoomType,
};
