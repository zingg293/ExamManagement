import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { ExaminationTypeDTO } from "@models/ExaminationTypeDTO";

export const ExaminationTypeApisService = createApi({
  reducerPath: "ExaminationTypeApisService",
  tagTypes: ["ExaminationTypeApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListExaminationType: builder.query<ListResponse<ExaminationTypeDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/ExaminationType/getListExaminationType`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ExaminationTypeApisService" as const, id })),
            {
              type: "ExaminationTypeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ExaminationTypeApisService", id: "LIST" }];
      }
    }),
    UpdateExaminationType: builder.mutation<payloadResult, { ExaminationType: Partial<ExaminationTypeDTO> }>({
      query: ({ ExaminationType }) => ({
        url: `/ExaminationType/updateExaminationType`,
        method: "PUT",
        data: ExaminationType
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "ExaminationTypeApisService", id: arg.ExaminationType.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.ExaminationType && arg.ExaminationType.id) {
          return [{ type: "ExaminationTypeApisService", id: arg.ExaminationType.id }];
        }
        return [];
      }
    }),
    GetExaminationTypeById: builder.query<ListResponse<ExaminationTypeDTO>, { idExaminationType: string }>({
      query: ({ idExaminationType }): any => ({
        url: `/ExaminationType/getExaminationTypeById`,
        params: { idExaminationType }
      }),
      providesTags: (result, error, arg) => [{ type: "ExaminationTypeApisService", id: arg.idExaminationType }]
    }),
    InsertExaminationType: builder.mutation<payloadResult, { ExaminationType: Partial<ExaminationTypeDTO> }>({
      query: ({ ExaminationType }) => ({
        url: `/ExaminationType/insertExaminationType`,
        method: "POST",
        data: ExaminationType
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExaminationTypeApisService", id: "LIST" }] : [])
    }),
    DeleteExaminationType: builder.mutation<payloadResult, { idExaminationType: string[] }>({
      query: ({ idExaminationType }) => ({
        url: `/ExaminationType/DeleteExaminationType`,
        method: "DELETE",
        data: idExaminationType
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExaminationTypeApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListExaminationTypeQuery,
  useUpdateExaminationTypeMutation,
  useGetExaminationTypeByIdQuery,
  useInsertExaminationTypeMutation,
  useDeleteExaminationTypeMutation
} = ExaminationTypeApisService;
