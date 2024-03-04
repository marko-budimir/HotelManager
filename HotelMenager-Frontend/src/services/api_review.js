import { get, post } from "./api_base";

const URL_PATH = "/api/review/";

const getReviewForRoom = async (roomId) => {
  return await get(URL_PATH+roomId)
};

const getReviewForRoomPaging = async (roomId, pageNumber) => {
  return await get(`${URL_PATH}${roomId}?pageNumber=${pageNumber}`);
};

const createReviewForRoom = () => {};

export default {
  getReviewForRoom,
  createReviewForRoom,
  getReviewForRoomPaging,
};
