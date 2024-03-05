import { get } from "./api_base";
import { buildQueryString } from "../common/HelperFunctions";

const BASE_URL = "/api/reservation";

const getAllReservations = async (query) => {
  const queryString = buildQueryString(query);
  try {
    const response = await get(`${BASE_URL}${queryString}`);
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

const getByIdReservation = () => { };

const createReservation = () => { };

const updateReservation = () => { };

const deleteReservation = () => { };



export default {
  getAllReservations,
  getByIdReservation,
  createReservation,
  updateReservation,
  deleteReservation,
};
