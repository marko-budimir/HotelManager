import { get, post, put, remove } from "./api_base";

const URL_PATH = "/api/hotelService";

const getAllServices = async () => {
  return await get(`${URL_PATH}`);
};

const getByIdService = async (id) => {
  return await get(`${URL_PATH}/${id}`);
};

const createService = async (serviceData) => {
  return await post(`${URL_PATH}`, serviceData);
};

const updateService = async (id, serviceData) => {
  return await put(`${URL_PATH}/${id}`, serviceData);
};

const deleteService = async (id) => {
  return await remove(`${URL_PATH}/${id}`);
};

export {
  getAllServices,
  getByIdService,
  createService,
  updateService,
  deleteService,
};
