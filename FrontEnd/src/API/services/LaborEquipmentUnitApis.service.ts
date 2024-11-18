import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { LaborEquipmentUnitDTO } from "@models/laborEquipmentUnitDTO";

export const LaborEquipmentUnitApisService = createApi({
  reducerPath: "LaborEquipmentUnitApisService",
  tagTypes: ["LaborEquipmentUnitApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListLaborEquipmentUnit: builder.query<ListResponse<LaborEquipmentUnitDTO>, pagination>({
      query: ({ pageSize, pageNumber }) => ({
        url: `/LaborEquipmentUnit/getListLaborEquipmentUnit`,
        method: "GET",
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "LaborEquipmentUnitApisService" as const, id })),
            {
              type: "LaborEquipmentUnitApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "LaborEquipmentUnitApisService", id: "LIST" }];
      }
    }),
    getListLaborEquipmentUnitByUnitAndEmployee: builder.query<
      ListResponse<LaborEquipmentUnitDTO>,
      { pageNumber: number; pageSize: number }
    >({
      query: ({ pageNumber, pageSize }) => ({
        url: `/LaborEquipmentUnit/getListLaborEquipmentUnitByUnitAndEmployee`,
        method: "GET",
        params: { pageNumber, pageSize }
      }),
      providesTags: (result) => (result ? [{ type: "LaborEquipmentUnitApisService", id: "LIST" }] : [])
    }),
    GetLaborEquipmentUnitById: builder.query<ListResponse<LaborEquipmentUnitDTO>, { idLaborEquipmentUnit: string }>({
      query: ({ idLaborEquipmentUnit }) => ({
        url: `/LaborEquipmentUnit/getLaborEquipmentUnitById`,
        method: "GET",
        params: { idLaborEquipmentUnit }
      }),
      providesTags: (result, error, arg) => [{ type: "LaborEquipmentUnitApisService", id: arg.idLaborEquipmentUnit }]
    }),
    getListLaborEquipmentUnitByIdLaborEquipment: builder.query<
      ListResponse<LaborEquipmentUnitDTO>,
      {
        idTicketLaborEquipment: string;
        pageSize: 0;
        pageNumber: 0;
      }
    >({
      query: ({ idTicketLaborEquipment, pageSize, pageNumber }) => ({
        url: `/LaborEquipmentUnit/getListLaborEquipmentUnitByIdLaborEquipment`,
        method: "GET",
        params: { idTicketLaborEquipment, pageSize, pageNumber }
      }),
      providesTags: (result) =>
        result
          ? [
              {
                type: "LaborEquipmentUnitApisService",
                id: "getListLaborEquipmentUnitByIdLaborEquipment"
              }
            ]
          : []
    }),
    insertLaborEquipmentUnit: builder.mutation<payloadResult, { idTicketLaborEquipment: string }>({
      query: ({ idTicketLaborEquipment }) => ({
        url: `/LaborEquipmentUnit/insertLaborEquipmentUnit`,
        method: "POST",
        params: { idTicketLaborEquipment }
      }),
      invalidatesTags: (result) =>
        result
          ? [
              {
                type: "LaborEquipmentUnitApisService",
                id: "getListLaborEquipmentUnitByIdLaborEquipment"
              }
            ]
          : []
    }),
    updateLaborEquipmentUnitByCodeAndStatus: builder.mutation<payloadResult, { EquipmentCode: string; status: number }>(
      {
        query: ({ EquipmentCode, status }) => ({
          url: `/LaborEquipmentUnit/updateLaborEquipmentUnitByCodeAndStatus`,
          method: "PUT",
          params: { EquipmentCode, status }
        })
      }
    ),
    getListLaborEquipmentUnitByListEquipmentCode: builder.query<
      ListResponse<LaborEquipmentUnitDTO>,
      {
        listEquipmentCode: any[];
        pageSize: number;
        pageNumber: number;
      }
    >({
      query: ({ listEquipmentCode, pageNumber, pageSize }) => ({
        url: `/LaborEquipmentUnit/getListLaborEquipmentUnitByListEquipmentCode`,
        method: "POST",
        params: { pageNumber, pageSize },
        data: listEquipmentCode
      })
    }),
    filterLaborEquipmentUnitModel: builder.query<
      ListResponse<LaborEquipmentUnitDTO>,
      {
        idEmployee: string;
        idUnit: string;
        pageNumber: number;
        status: number;
        pageSize: number;
      }
    >({
      query: (model) => ({
        url: `/LaborEquipmentUnit/filterLaborEquipmentUnitModel`,
        method: "POST",
        data: model
      })
    }),
    createLaborEquipmentUnit: builder.mutation<payloadResult, { model: LaborEquipmentUnitDTO }>({
      query: ({ model }) => ({
        url: `/LaborEquipmentUnit/createLaborEquipmentUnit`,
        method: "POST",
        data: model
      })
    })
  })
});

export const {
  useGetListLaborEquipmentUnitQuery,
  useGetLaborEquipmentUnitByIdQuery,
  useGetListLaborEquipmentUnitByUnitAndEmployeeQuery,
  useGetListLaborEquipmentUnitByIdLaborEquipmentQuery,
  useInsertLaborEquipmentUnitMutation,
  useUpdateLaborEquipmentUnitByCodeAndStatusMutation,
  useGetListLaborEquipmentUnitByListEquipmentCodeQuery,
  useLazyFilterLaborEquipmentUnitModelQuery,
  useCreateLaborEquipmentUnitMutation
} = LaborEquipmentUnitApisService;
