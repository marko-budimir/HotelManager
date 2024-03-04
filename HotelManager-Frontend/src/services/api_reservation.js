import { get } from "./api_base";

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

const buildQueryString = ({ filter, currentPage, pageSize, sortBy, sortOrder }) => {
  let queryString = '';
  queryString += `?pageNumber=${currentPage}&pageSize=${pageSize}`;
  if (sortBy) {
    queryString += `&sortBy=${sortBy}&sortOrder=${sortOrder}`;
  }
  for (const key in filter) {
    if (filter[key]) {
      queryString += `&${key}=${filter[key]}`;
    }
  }
  return queryString;
}

export default {
  getAllReservations,
  getByIdReservation,
  createReservation,
  updateReservation,
  deleteReservation,
};
