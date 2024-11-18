import axios from "axios";
import { globalVariable } from "~/globalVariable";

export const axiosClient = axios.create({
  baseURL: `${globalVariable.urlServerApi}/api/v1/`
});

axiosClient.interceptors.request.use(async (config) => {
  // Handle token here ...
  return config;
});
axiosClient.interceptors.response.use(
  (response) => {
    if (response && response.data) {
      return response.data;
    }
    return response;
  },
  (error) => {
    // Handle errors
    throw error;
  }
);
