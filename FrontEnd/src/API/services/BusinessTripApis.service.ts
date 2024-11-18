import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { BusinessTripDTO } from "@models/businessTripDTO";
import { globalVariable } from "~/globalVariable";

export const BusinessTripApisService = createApi({
  reducerPath: "BusinessTripApisService",
  tagTypes: ["BusinessTripApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListBusinessTrip: builder.query<ListResponse<BusinessTripDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/businessTrip/getListBusinessTrip`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "BusinessTripApisService" as const, id })),
            {
              type: "BusinessTripApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "BusinessTripApisService", id: "LIST" }];
      }
    }),
    GetBusinessTripById: builder.query<ListResponse<BusinessTripDTO>, { idBusinessTrip: string }>({
      query: ({ idBusinessTrip }): any => ({
        url: `/businessTrip/getBusinessTripById`,
        params: { idBusinessTrip }
      }),
      providesTags: (result, error, arg) => [{ type: "BusinessTripApisService", id: arg.idBusinessTrip }]
    }),
    InsertBusinessTrip: builder.mutation<payloadResult, { businessTrip: Partial<BusinessTripDTO> }>({
      query: ({ businessTrip }) => ({
        url: `/businessTrip/insertBusinessTrip`,
        method: "POST",
        data: businessTrip
      }),
      invalidatesTags: (result) => (result ? [{ type: "BusinessTripApisService", id: "LIST" }] : [])
    }),
    UpdateBusinessTrip: builder.mutation<payloadResult, { businessTrip: Partial<BusinessTripDTO> }>({
      query: ({ businessTrip }) => ({
        url: `/businessTrip/updateBusinessTrip`,
        method: "PUT",
        data: businessTrip
      }),
      invalidatesTags: (result, error, arg) =>
        result ? [{ type: "BusinessTripApisService", id: arg.businessTrip.id }] : []
    }),
    DeleteBusinessTrip: builder.mutation<payloadResult, { listId: string[] }>({
      query: ({ listId }) => ({
        url: `/businessTrip/deleteBusinessTrip`,
        method: "DELETE",
        data: listId
      }),
      invalidatesTags: (result) => (result ? [{ type: "BusinessTripApisService", id: "LIST" }] : [])
    }),
    GetListBusinessTripByRole: builder.query<ListResponse<BusinessTripDTO>, pagination>({
      query: (pagination): any => ({
        url: `/businessTrip/getListBusinessTripByRole`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "BusinessTripApisService" as const, id })),
            {
              type: "BusinessTripApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "BusinessTripApisService", id: "LIST" }];
      }
    }),
    GetListBusinessTripByHistory: builder.query<ListResponse<BusinessTripDTO>, pagination>({
      query: (pagination): any => ({
        url: `/businessTrip/getListBusinessTripByHistory`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "BusinessTripApisService" as const, id })),
            {
              type: "BusinessTripApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "BusinessTripApisService", id: "LIST" }];
      }
    }),
    filterListBusinessTrip: builder.query<
      ListResponse<BusinessTripDTO>,
      { idUnit: string; startDate: string; endDate: string }
    >({
      query: (filter): any => ({
        url: `/businessTrip/filterListBusinessTrip`,
        method: "GET",
        params: filter
      })
    })
  })
});

export const getFileBusinessTrip = (fileNameId: string) => {
  return `${globalVariable.urlServerApi}/api/v1/businessTrip/getFileBusinessTrip?fileNameId=${fileNameId}`;
};
export const {
  useGetListBusinessTripQuery,
  useGetBusinessTripByIdQuery,
  useInsertBusinessTripMutation,
  useUpdateBusinessTripMutation,
  useDeleteBusinessTripMutation,
  useGetListBusinessTripByRoleQuery,
  useGetListBusinessTripByHistoryQuery,
  useLazyFilterListBusinessTripQuery
} = BusinessTripApisService;
