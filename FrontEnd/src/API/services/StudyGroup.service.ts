import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { StudyGroupDTO } from "@models/StudyGroupDTO";

export const StudyGroupApisService = createApi({
  reducerPath: "StudyGroupApisService",
  tagTypes: ["StudyGroupApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListStudyGroup: builder.query<ListResponse<StudyGroupDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/StudyGroup/getListStudyGroup`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "StudyGroupApisService" as const, id })),
            {
              type: "StudyGroupApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "StudyGroupApisService", id: "LIST" }];
      }
    }),
    UpdateStudyGroup: builder.mutation<
      payloadResult,
      { StudyGroup: Partial<StudyGroupDTO> }
    >({
      query: ({ StudyGroup }) => ({
        url: `/StudyGroup/updateStudyGroup`,
        method: "PUT",
        data: StudyGroup
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "StudyGroupApisService", id: arg.StudyGroup.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.StudyGroup && arg.StudyGroup.id) {
          return [{ type: "StudyGroupApisService", id: arg.StudyGroup.id }];
        }
        return [];
      }
    }),
    GetStudyGroupById: builder.query<ListResponse<StudyGroupDTO>, { idStudyGroup: string }>({
      query: ({ idStudyGroup }): any => ({
        url: `/StudyGroup/getStudyGroupById`,
        params: { idStudyGroup }
      }),
      providesTags: (result, error, arg) => [{ type: "StudyGroupApisService", id: arg.idStudyGroup }]
    }),
    InsertStudyGroup: builder.mutation<
      payloadResult,
      { StudyGroup: Partial<StudyGroupDTO> }
    >({
      query: ({ StudyGroup }) => ({
        url: `/StudyGroup/insertStudyGroup`,
        method: "POST",
        data: StudyGroup
      }),
      invalidatesTags: (result) => (result ? [{ type: "StudyGroupApisService", id: "LIST" }] : [])
    }),
    DeleteStudyGroup: builder.mutation<payloadResult, { idStudyGroup: string[] }>({
      query: ({ idStudyGroup }) => ({
        url: `/StudyGroup/DeleteStudyGroup`,
        method: "DELETE",
        data: idStudyGroup
      }),
      invalidatesTags: (result) => (result ? [{ type: "StudyGroupApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListStudyGroupQuery,
  useUpdateStudyGroupMutation,
  useGetStudyGroupByIdQuery,
  useInsertStudyGroupMutation,
  useDeleteStudyGroupMutation
} = StudyGroupApisService;
