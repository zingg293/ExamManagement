import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse } from "~/models/common";
import { PositionEmployeeDTO } from "@models/positionEmployeeDTO";

export const PositionEmployeeApisService = createApi({
  reducerPath: "PositionEmployeeApisService",
  tagTypes: ["PositionEmployeeApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListPositionEmployeeByIdEmployee: builder.query<ListResponse<PositionEmployeeDTO>, { idEmployee: string }>({
      query: ({ idEmployee }) => ({
        url: `/positionEmployee/getListPositionEmployeeByIdEmployee`,
        method: "GET",
        params: { idEmployee }
      }),
      providesTags: (result, err, arg) => [{ type: "PositionEmployeeApisService", id: arg?.idEmployee }]
    }),
    insertPositionEmployeeByList: builder.mutation<any, { listPositionEmployee: PositionEmployeeDTO[] }>({
      query: ({ listPositionEmployee }) => ({
        url: `/positionEmployee/insertPositionEmployeeByList`,
        method: "POST",
        data: listPositionEmployee
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "PositionEmployeeApisService",
                id: arg?.listPositionEmployee[0].idEmployee
              }
            ]
          : []
    }),
    updatePositionEmployeeByList: builder.mutation<any, { listPositionEmployee: PositionEmployeeDTO[] }>({
      query: ({ listPositionEmployee }) => ({
        url: `/positionEmployee/updatePositionEmployeeByList`,
        method: "PUT",
        data: listPositionEmployee
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "PositionEmployeeApisService",
                id: arg?.listPositionEmployee[0].idEmployee
              }
            ]
          : []
    })
  })
});

export const {
  useGetListPositionEmployeeByIdEmployeeQuery,
  useLazyGetListPositionEmployeeByIdEmployeeQuery,
  useInsertPositionEmployeeByListMutation,
  useUpdatePositionEmployeeByListMutation
} = PositionEmployeeApisService;
