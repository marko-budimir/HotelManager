import { get, post, put, remove} from "./api_base";

const URL_PATH = "/api/Discount/";

const getAllDiscounts = async (startingValue = 0, endValue = 100, code = "", pageNum = 1, pageSize = 10, sortOrder = "ASC", sortBy = "Percent", dateCreated = null, dateUpdated = null) => {
  try {
    const response = await get(URL_PATH, {
      startingValue,
      endValue
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching all discounts:", error);
    throw error;
  }
};

const getByIdDiscount = async (id) => {
  
};

const createDiscount = async (discount) => {
  return await post(`${URL_PATH}`, discount)
};

const updateDiscount = () => {};

const deleteDiscount = () => {};

export {
  getAllDiscounts,
  getByIdDiscount,
  createDiscount,
  updateDiscount,
  deleteDiscount
};
