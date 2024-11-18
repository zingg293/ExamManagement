import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { TrainingSystemDTO } from "@models/TrainingSystemDTO";

export const TrainingSystemApisService = createApi({
  reducerPath: "TrainingSystemApisService",
  tagTypes: ["TrainingSystemApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListTrainingSystem: builder.query<ListResponse<TrainingSystemDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/TrainingSystem/getListTrainingSystem`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "TrainingSystemApisService" as const, id })),
            {
              type: "TrainingSystemApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "TrainingSystemApisService", id: "LIST" }];
      }
    }),
    UpdateTrainingSystem: builder.mutation<
      payloadResult,
      { TrainingSystem: Partial<TrainingSystemDTO> }
    >({
      query: ({ TrainingSystem }) => ({
        url: `/TrainingSystem/updateTrainingSystem`,
        method: "PUT",
        data: TrainingSystem
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "TrainingSystemApisService", id: arg.TrainingSystem.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.TrainingSystem && arg.TrainingSystem.id) {
          return [{ type: "TrainingSystemApisService", id: arg.TrainingSystem.id }];
        }
        return [];
      }
    }),
    GetTrainingSystemById: builder.query<ListResponse<TrainingSystemDTO>, { idTrainingSystem: string }>({
      query: ({ idTrainingSystem }): any => ({
        url: `/TrainingSystem/getTrainingSystemById`,
        params: { idTrainingSystem }
      }),
      providesTags: (result, error, arg) => [{ type: "TrainingSystemApisService", id: arg.idTrainingSystem }]
    }),
    InsertTrainingSystem: builder.mutation<
      payloadResult,
      { TrainingSystem: Partial<TrainingSystemDTO> }
    >({
      query: ({ TrainingSystem }) => ({
        url: `/TrainingSystem/insertTrainingSystem`,
        method: "POST",
        data: TrainingSystem
      }),
      invalidatesTags: (result) => (result ? [{ type: "TrainingSystemApisService", id: "LIST" }] : [])
    }),
    DeleteTrainingSystem: builder.mutation<payloadResult, { idTrainingSystem: string[] }>({
      query: ({ idTrainingSystem }) => ({
        url: `/TrainingSystem/DeleteTrainingSystem`,
        method: "DELETE",
        data: idTrainingSystem
      }),
      invalidatesTags: (result) => (result ? [{ type: "TrainingSystemApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListTrainingSystemQuery,
  useUpdateTrainingSystemMutation,
  useGetTrainingSystemByIdQuery,
  useInsertTrainingSystemMutation,
  useDeleteTrainingSystemMutation
} = TrainingSystemApisService;
