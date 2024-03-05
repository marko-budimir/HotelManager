import { get, post } from "./api_base";
import { buildQueryString } from "../common/HelperFunctions";

const BASE_URL = "/api/ServiceInvoice";

const getByInvoiceId = async (query) => {
  const queryString = buildQueryString(query);
  try {
    const response = await get(`${BASE_URL}${queryString}`);
    if (response.status === 200) {
      return response.data.items;
    }
    return [];
  }
  catch (error) {
    console.error(error);
    return [];
  }
};

const getAllServiceInvoice = () => { };

const deleteServiceInvoice = () => { };

const createServiceInvoice = (data) => {
  console.log( "test", data);
  return post(BASE_URL, data);
};

export default {
  getByInvoiceId,
  getAllServiceInvoice,
  deleteServiceInvoice,
  createServiceInvoice,
};
