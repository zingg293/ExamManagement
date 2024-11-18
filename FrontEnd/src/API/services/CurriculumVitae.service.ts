import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination } from "~/models/common";
import CurriculumVitaeDTO from "@models/CurriculumVitaeDTO";

export const CurriculumVitaeApisService = createApi({
  reducerPath: "AllowanceApisService",
  tagTypes: ["AllowanceApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCurriculumVitae: builder.query<ListResponse<CurriculumVitaeDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/allowance/getListAllowance`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "AllowanceApisService" as const, id })),
            {
              type: "AllowanceApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "AllowanceApisService", id: "LIST" }];
      }
    }),
  })

});

 export const {
    useGetListCurriculumVitaeQuery,
  } = CurriculumVitaeApisService;