import type { AxiosError, AxiosRequestConfig } from "axios";
import axios from "axios";
import { BaseQueryFn } from "@reduxjs/toolkit/dist/query";
import { globalVariable } from "~/globalVariable";
import { getCookie } from "~/units";

export const axiosBaseQuery =
  (
    { baseUrl }: { baseUrl: string } = { baseUrl: `${globalVariable.urlServerApi}/api/v1` }
  ): BaseQueryFn<
    {
      url: string;
      method: AxiosRequestConfig["method"];
      data?: AxiosRequestConfig["data"];
      params?: AxiosRequestConfig["params"];
    },
    unknown,
    unknown
  > =>
  async ({ url, method, data, params }) => {
    try {
      const result = await axios({
        url: baseUrl + url,
        method,
        data,
        params,
        headers: {
          Authorization: `Bearer ${getCookie("jwt")}`
        }
      });
      return { data: result.data };
    } catch (error) {
      const axiosError = error as AxiosError;
      let serializedError = {};
      if (axiosError.config) {
        serializedError = {
          message: axiosError?.message,
          url: axiosError?.config?.url,
          method: axiosError?.config?.method,
          Authorization: axiosError?.config?.headers?.Authorization,
          data: axiosError?.config?.data
        };
      }
      console.warn("axiosError", axiosError);
      return {
        error: {
          data: axiosError.response?.data || axiosError.message,
          error: serializedError,
          status: axiosError.response?.status
        }
      };
    }
  };
