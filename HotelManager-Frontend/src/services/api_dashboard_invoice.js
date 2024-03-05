import { get } from './api_base';
import { buildQueryString } from '../common/HelperFunctions';

const URL_PATH = "/api/DashboardReceipt";

const getAllDashboardInvoice = async (query) => {
  const queryString = buildQueryString(query);
  try {
    const response = await get(`${URL_PATH}${queryString}`);
    if (response.status === 200) {
      return [response.data.items, response.data.totalPages];
    }
    return [];
  }
  catch (error) {
    console.error(error);
    return [];
  }
};

const getByIdDashboardInvoice = () => {};

const sendDashboardInvoice = () => {};

export default {
  getAllDashboardInvoice,
  getByIdDashboardInvoice,
  sendDashboardInvoice,
};
