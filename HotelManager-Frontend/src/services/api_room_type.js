import { get, post, put, remove} from "./api_base";

const URL_PATH = "/api/RoomType/";

const getAllRoomType = async () => {
  return await get(URL_PATH)
};

const getByIdRoomType = async (roomyTypeId) => {
  return await get(URL_PATH+roomyTypeId)
};

const createRoomType = async (roomType) => {
  return await post(`${URL_PATH}`,roomType)
};

const updateRoomType = async (roomyTypeId, roomType) => {
  return await put(`${URL_PATH}${roomyTypeId}`, roomType)
};

const deleteRoomType = async (roomyTypeId) => {
  return await remove(`${URL_PATH}${roomyTypeId}`)
}

export  {
  getAllRoomType,
  getByIdRoomType,
  createRoomType,
  updateRoomType,
  deleteRoomType
};
