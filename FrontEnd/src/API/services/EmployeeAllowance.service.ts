import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";

export const EmployeeAllowanceApisService = createApi({
  reducerPath: "EmployeeAllowanceApisService",
  tagTypes: ["EmployeeAllowanceApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: () => ({})
});
