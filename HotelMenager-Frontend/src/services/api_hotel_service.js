import { get, post, put } from "./api_base";

const URL_PATH = "/api/hotelService";

const getAllServices = async () => {
  return await get(`${URL_PATH}`);
};

const getByIdService = () => {};

const createService = () => {};

const updateService = () => {};

const deleteService = () => {};

export {
  getAllServices,
  getByIdService,
  createService,
  updateService,
  deleteService,
};
