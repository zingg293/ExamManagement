import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse } from "~/models/common";
import { EmployeeDTO } from "@models/employeeDTO";
import { OnLeaveDTO } from "@models/onLeaveDTO";
import { OvertimeDTO } from "@models/overtimeDTO";
import axios from "axios";
import { globalVariable } from "~/globalVariable";

export const MarkWorkPointsApisService = createApi({
  reducerPath: "MarkWorkPointsApisService",
  tagTypes: ["MarkWorkPointsApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListMarkWorkPoints: builder.query<
      ListResponse<{
        employee: EmployeeDTO;
        onLeaves: OnLeaveDTO[];
        overtimes: OvertimeDTO[];
      }>,
      { fromDate: string; toDate: string; idEmployee: string; pageNumber: number; pageSize: number }
    >({
      query: (filter): any => ({
        url: `/markWorkPoints/getListMarkWorkPoints`,
        method: "POST",
        data: filter
      })
    })
  })
});
export const getFileExcelMarkWorkPoints = async (filter: { fromDate: string; toDate: string; idEmployee: string }) => {
  const url = `${globalVariable.urlServerApi}/api/v1/markWorkPoints/getFileExcelMarkWorkPoints`;
  return await axios.post(url, filter, {
    responseType: "blob"
  });
};
export const { useLazyGetListMarkWorkPointsQuery } = MarkWorkPointsApisService;
