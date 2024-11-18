import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { CategoryNewsDTO } from "@models/categoryNewsDTO";
import { globalVariable } from "~/globalVariable";

export const CategoryNewsApisService = createApi({
  reducerPath: "CategoryNewsApisService",
  tagTypes: ["CategoryNewsApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryNews: builder.query<ListResponse<CategoryNewsDTO>, pagination>({
      query: (pagination) => ({
        url: `/categoryNews/getListCategoryNews`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryNewsApisService" as const, id })),
            {
              type: "CategoryNewsApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryNewsApisService", id: "LIST" }];
      }
    }),

    GetListCategoryNewsByIdParent: builder.query<ListResponse<CategoryNewsDTO>, { idParent: string }>({
      query: ({ idParent }) => ({
        url: `/categoryNews/getListCategoryNewsByIdParent`,
        method: "GET",
        params: { idParent }
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryNewsApisService", id: arg.idParent }]
    }),

    GetListCategoryNewsAvailable: builder.query<ListResponse<CategoryNewsDTO>, pagination>({
      query: (pagination) => ({
        url: `/categoryNews/getListCategoryNewsAvailable`,
        params: pagination,
        method: "GET"
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryNewsApisService" as const, id })),
            {
              type: "CategoryNewsApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryNewsApisService", id: "LIST" }];
      }
    }),

    GetCategoryNewsById: builder.query<ListResponse<CategoryNewsDTO>, { idCategoryNews: string }>({
      query: ({ idCategoryNews }) => ({
        url: `/categoryNews/getCategoryNewsById`,
        params: { idCategoryNews },
        method: "GET"
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryNewsApisService", id: arg.idCategoryNews }]
    }),

    UpdateCategoryNews: builder.mutation<payloadResult, CategoryNewsDTO>({
      query: (categoryNews) => ({
        url: `/categoryNews/updateCategoryNews`,
        method: "PUT",
        data: categoryNews
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "CategoryNewsApisService", id: arg.id }] : [])
    }),

    InsertCategoryNews: builder.mutation<payloadResult, CategoryNewsDTO>({
      query: (categoryNews) => ({
        url: `/categoryNews/insertCategoryNews`,
        method: "POST",
        data: categoryNews
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryNewsApisService", id: "LIST" }] : [])
    }),

    HideCategoryNews: builder.mutation<payloadResult, { isHide: boolean; listId: string[] }>({
      query: ({ isHide, listId }) => ({
        url: `/categoryNews/hideCategoryNews`,
        method: "PUT",
        data: listId,
        params: { isHide }
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryNewsApisService", id: "LIST" }] : [])
    }),

    ShowCategoryNews: builder.mutation<payloadResult, { listId: string[]; isShow: boolean }>({
      query: ({ listId, isShow }) => ({
        url: `/categoryNews/showCategoryNews`,
        method: "PUT",
        data: listId,
        params: { isShow }
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryNewsApisService", id: "LIST" }] : [])
    }),

    DeleteCategoryNews: builder.mutation<payloadResult, { listId: string[] }>({
      query: ({ listId }) => ({
        url: `/categoryNews/deleteCategoryNews`,
        method: "DELETE",
        data: listId
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryNewsApisService", id: "LIST" }] : [])
    })
  })
});

export const getFileImageCategoryNews = (fileNameId: string) => {
  return `${globalVariable.urlServerApi}/api/v1/categoryNews/getFileImage?fileNameId=${fileNameId}`;
};

export const {
  useGetListCategoryNewsQuery,
  useGetListCategoryNewsByIdParentQuery,
  useGetListCategoryNewsAvailableQuery,
  useGetCategoryNewsByIdQuery,
  useUpdateCategoryNewsMutation,
  useInsertCategoryNewsMutation,
  useHideCategoryNewsMutation,
  useShowCategoryNewsMutation,
  useDeleteCategoryNewsMutation
} = CategoryNewsApisService;
