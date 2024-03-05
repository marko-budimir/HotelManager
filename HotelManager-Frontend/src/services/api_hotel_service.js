import { get, post, put, remove } from "./api_base";

const URL_PATH = "/api/hotelService";

const getAllServices = async () => {
  return await get(`${URL_PATH}`);
};

const getByIdService = () => {};

const createService = () => {};

const updateService = () => {};

const deleteService = () => {};

export default {
  getAllServices,
  getByIdService,
  createService,
  updateService,
  deleteService,
};
