import { get, post, put, remove} from "./api_base";

const URL_PATH = "/api/Discount/";

const getAllDiscounts = async () => {
  return await get(URL_PATH);
};

const getByIdDiscount = async (discountId) => {
  return await get(URL_PATH+discountId)
};

const createDiscount = async (discount) => {
  return await post(`${URL_PATH}`, discount)
};

const updateDiscount = async (discountId, discount) => {
  return await put(`${URL_PATH}${discountId}`, discount)
};

const deleteDiscount = async (discountId) => {
  return await remove(`${URL_PATH}${discountId}`)
};

export {
  getAllDiscounts,
  getByIdDiscount,
  createDiscount,
  updateDiscount,
  deleteDiscount
};
