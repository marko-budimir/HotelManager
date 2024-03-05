import { buildQueryString } from "../common/HelperFunctions";
import { get, remove } from "./api_base";

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

const deleteReservation = async (reservationId, invoiceId) => {
  try {
    console.log(`${BASE_URL}/${reservationId}?invoiceId=${invoiceId}`);
    const response = await remove(`${BASE_URL}/${reservationId}?invoiceId=${invoiceId}`);
    if (response.status === 200) {
      console.log("Reservation deleted successfully.");
      return true;

    }
    return false;
  } catch (error) {
    console.error("Error deleting reservation:", error);
    return false;
  }
};



export default {
  getAllReservations,
  getByIdReservation,
  createReservation,
  updateReservation,
  deleteReservation,
};
