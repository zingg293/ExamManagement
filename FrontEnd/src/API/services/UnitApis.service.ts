import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { UnitDTO } from "@models/unitDto";

export const UnitApisService = createApi({
  reducerPath: "UnitApisService",
  tagTypes: ["UnitApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListUnit: builder.query<ListResponse<UnitDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/Unit/getListUnit?pageNumber=${pageNumber}&pageSize=${pageSize}`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "UnitApisService" as const, id })),
            {
              type: "UnitApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "UnitApisService", id: "LIST" }];
      }
    }),
    GetUnitById: builder.query<ListResponse<UnitDTO>, { id: string }>({
      query: ({ id }): any => ({
        url: `/Unit/getUnitById?idUnit=${id}`
      }),
      providesTags: (result, error, arg) => [{ type: "UnitApisService", id: arg.id }]
    }),
    GetListUnitAvailable: builder.query<ListResponse<UnitDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/Unit/getListUnitAvailable?pageNumber=${pageNumber}&pageSize=${pageSize}`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "UnitApisService" as const, id })),
            {
              type: "UnitApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "UnitApisService", id: "LIST" }];
      }
    }),
    InsertUnit: builder.mutation<payloadResult, { unit: Partial<UnitDTO> }>({
      query: ({ unit }) => ({
        url: `/Unit/insertUnit`,
        method: "POST",
        data: unit
      }),
      invalidatesTags: (result) => (result ? [{ type: "UnitApisService", id: "LIST" }] : [])
    }),
    UpdateUnit: builder.mutation<payloadResult, { unit: Partial<UnitDTO> }>({
      query: ({ unit }) => ({
        url: `/Unit/updateUnit`,
        method: "PUT",
        data: unit
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "UnitApisService",
                id: arg.unit.id
              },
              { type: "UnitApisService", id: "L" }
            ]
          : []
    }),
    DeleteUnit: builder.mutation<payloadResult, { idUnit: string[] }>({
      query: ({ idUnit }) => ({
        url: `/Unit/deleteUnit`,
        method: "DELETE",
        data: [...idUnit]
      }),
      invalidatesTags: (result) => (result ? [{ type: "UnitApisService", id: "LIST" }] : [])
    }),
    HideUnit: builder.mutation<payloadResult, { idUnit: string[]; isHide: boolean }>({
      query: ({ idUnit, isHide }) => ({
        url: `/Unit/hideUnit?isHide=${isHide}`,
        method: "PUT",
        data: [...idUnit]
      }),
      invalidatesTags: (result) => (result ? [{ type: "UnitApisService", id: "LIST" }] : [])
    }),
    getListUnitByIdParent: builder.query<ListResponse<UnitDTO>, { idParent: string }>({
      query: ({ idParent }): any => ({
        url: `/Unit/getListUnitByIdParent`,
        method: "GET",
        params: { idParent }
      }),
      providesTags: () => [{ type: "UnitApisService", id: "L" }]
    })
  })
});
export const {
  useGetListUnitQuery,
  useGetUnitByIdQuery,
  useGetListUnitAvailableQuery,
  useInsertUnitMutation,
  useUpdateUnitMutation,
  useDeleteUnitMutation,
  useHideUnitMutation,
  useGetListUnitByIdParentQuery
} = UnitApisService;
