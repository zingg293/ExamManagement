import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { TestScheduleDTO } from "@models/TestScheduleDTO";

export const TestScheduleApisService = createApi({
  reducerPath: "TestScheduleApisService",
  tagTypes: ["TestScheduleApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListTestSchedule: builder.query<ListResponse<TestScheduleDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/TestSchedule/getListTestSchedule`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "TestScheduleApisService" as const, id })),
            {
              type: "TestScheduleApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "TestScheduleApisService", id: "LIST" }];
      }
    }),
    UpdateTestSchedule: builder.mutation<
      payloadResult,
      { TestSchedule: Partial<TestScheduleDTO> }
    >({
      query: ({ TestSchedule }) => ({
        url: `/TestSchedule/updateTestSchedule`,
        method: "PUT",
        data: TestSchedule
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "TestScheduleApisService", id: arg.TestSchedule.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.TestSchedule && arg.TestSchedule.id) {
          return [{ type: "TestScheduleApisService", id: arg.TestSchedule.id }];
        }
        return [];
      }
    }),
    GetTestScheduleById: builder.query<ListResponse<TestScheduleDTO>, { idTestSchedule: string }>({
      query: ({ idTestSchedule }): any => ({
        url: `/TestSchedule/getTestScheduleById`,
        params: { idTestSchedule }
      }),
      providesTags: (result, error, arg) => [{ type: "TestScheduleApisService", id: arg.idTestSchedule }]
    }),
    InsertTestSchedule: builder.mutation<
      payloadResult,
      { TestSchedule: Partial<TestScheduleDTO> }
    >({
      query: ({ TestSchedule }) => ({
        url: `/TestSchedule/insertTestSchedule`,
        method: "POST",
        data: TestSchedule
      }),
      invalidatesTags: (result) => (result ? [{ type: "TestScheduleApisService", id: "LIST" }] : [])
    }),
    DeleteTestSchedule: builder.mutation<payloadResult, { idTestSchedule: string[] }>({
      query: ({ idTestSchedule }) => ({
        url: `/TestSchedule/DeleteTestSchedule`,
        method: "DELETE",
        data: idTestSchedule
      }),
      invalidatesTags: (result) => (result ? [{ type: "TestScheduleApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListTestScheduleQuery,
  useUpdateTestScheduleMutation,
  useGetTestScheduleByIdQuery,
  useInsertTestScheduleMutation,
  useDeleteTestScheduleMutation
} = TestScheduleApisService;
