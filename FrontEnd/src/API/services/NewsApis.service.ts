import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { NewsDTO } from "@models/newsDTO";
import { globalVariable } from "~/globalVariable";

export const NewsApisService = createApi({
  reducerPath: "NewsApisService",
  tagTypes: ["NewsApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListNews: builder.query<ListResponse<NewsDTO>, pagination>({
      query: (pagination) => ({
        url: `/news/getListNews`,
        params: pagination,
        method: "GET"
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "NewsApisService" as const, id })),
            {
              type: "NewsApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "NewsApisService", id: "LIST" }];
      }
    }),
    GetNewsById: builder.query<ListResponse<NewsDTO>, { idNews: string }>({
      query: ({ idNews }) => ({
        url: `/news/getNewsById`,
        params: { idNews },
        method: "GET"
      }),
      providesTags: (result, error, arg) => [{ type: "NewsApisService", id: arg.idNews }]
    }),
    SaveNewsImage: builder.mutation<payloadResult, Blob>({
      query: (imageBlob) => ({
        url: `/news/saveNewsImage`,
        method: "POST",
        data: imageBlob
      })
    }),
    UpdateNews: builder.mutation<payloadResult, NewsDTO>({
      query: (news) => ({
        url: `/news/updateNews`,
        method: "PUT",
        data: news
      }),
      invalidatesTags: (result) => (result ? [{ type: "NewsApisService", id: "LIST" }] : [])
    }),
    GetListNewsApproved: builder.query<ListResponse<NewsDTO>, pagination>({
      query: (pagination) => ({
        url: `/news/getListNewsApproved`,
        params: pagination,
        method: "GET"
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "NewsApisService" as const, id })),
            {
              type: "NewsApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "NewsApisService", id: "LIST" }];
      }
    }),
    DeleteNews: builder.mutation<payloadResult, { listId: string[] }>({
      query: ({ listId }) => ({
        url: `/news/deleteNews`,
        method: "DELETE",
        data: listId
      }),
      invalidatesTags: (result) => (result ? [{ type: "NewsApisService", id: "LIST" }] : [])
    }),
    HideNews: builder.mutation<payloadResult, { isHide: boolean; listId: string[] }>({
      query: ({ isHide, listId }) => ({
        url: `/news/hideNews`,
        method: "PUT",
        data: listId,
        params: { isHide }
      }),
      invalidatesTags: (result) => (result ? [{ type: "NewsApisService", id: "LIST" }] : [])
    }),
    ApproveNews: builder.mutation<payloadResult, { isApprove: boolean; listId: string[] }>({
      query: ({ isApprove, listId }) => ({
        url: `/news/approveNews`,
        method: "PUT",
        data: listId,
        params: { isApprove }
      }),
      invalidatesTags: (result) => (result ? [{ type: "NewsApisService", id: "LIST" }] : [])
    }),

    InsertNews: builder.mutation<payloadResult, NewsDTO>({
      query: (news) => ({
        url: `/news/insertNews`,
        method: "POST",
        data: news
      }),
      invalidatesTags: (result) => (result ? [{ type: "NewsApisService", id: "LIST" }] : [])
    }),

    IncreaseNumberView: builder.mutation<payloadResult, { idNews: string }>({
      query: ({ idNews }) => ({
        url: `/news/increaseNumberView`,
        method: "PUT",
        data: { idNews }
      }),
      invalidatesTags: (result) => (result ? [{ type: "NewsApisService", id: "LIST" }] : [])
    }),

    IncreaseNumberLike: builder.mutation<payloadResult, { idNews: string }>({
      query: ({ idNews }) => ({
        url: `/news/increaseNumberLike`,
        method: "PUT",
        data: { idNews }
      }),
      invalidatesTags: ["NewsApisService"]
    }),
    filterListNews: builder.query<
      ListResponse<NewsDTO>,
      {
        title?: string;
        idCategoryNews?: string;
        createdDateDisplay?: string;
        isHide?: boolean;
        isApproved?: boolean;
        pageNumber?: number;
        pageSize?: number;
      }
    >({
      query: (filter) => ({
        url: `/news/filterListNews`,
        method: "POST",
        data: filter
      })
    })
  })
});
export const getFileImageNews = (fileNameId: string) => {
  return `${globalVariable.urlServerApi}/api/v1/news/getFileImageNews?fileNameId=${fileNameId}`;
};
export const {
  useGetListNewsQuery,
  useGetNewsByIdQuery,
  useSaveNewsImageMutation,
  useUpdateNewsMutation,
  useGetListNewsApprovedQuery,
  useDeleteNewsMutation,
  useHideNewsMutation,
  useApproveNewsMutation,
  useInsertNewsMutation,
  useIncreaseNumberViewMutation,
  useIncreaseNumberLikeMutation,
  useLazyFilterListNewsQuery
} = NewsApisService;
