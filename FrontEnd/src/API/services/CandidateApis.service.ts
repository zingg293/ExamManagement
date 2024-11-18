import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination } from "~/models/common";
import { CandidateDTO } from "@models/candidateDTO";
import { globalVariable } from "~/globalVariable";

export const CandidateApisService = createApi({
  reducerPath: "CandidateApisService",
  tagTypes: ["CandidateApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCandidate: builder.query<ListResponse<CandidateDTO>, pagination>({
      query: (pagination) => ({
        url: `/candidate/getListCandidate`,
        params: pagination,
        method: "GET"
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CandidateApisService" as const, id })),
            {
              type: "CandidateApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CandidateApisService", id: "LIST" }];
      }
    }),

    GetCandidateById: builder.query<ListResponse<CandidateDTO>, { idCandidate: string }>({
      query: ({ idCandidate }) => ({
        url: `/candidate/getCandidateById`,
        params: { idCandidate },
        method: "GET"
      }),
      providesTags: (result, error, arg) => [{ type: "CandidateApisService", id: arg.idCandidate }]
    }),

    InsertCandidate: builder.mutation<void, CandidateDTO>({
      query: (candidate) => ({
        url: `/candidate/insertCandidate`,
        method: "POST",
        data: candidate
      }),
      invalidatesTags: (result) => (result ? [{ type: "CandidateApisService", id: "LIST" }] : [])
    })
  })
});

export const getFileCandidate = (fileNameId: string) => {
  return `${globalVariable.urlServerApi}/api/v1/candidate/getFileCandidate?fileNameId=${fileNameId}`;
};
export const { useGetListCandidateQuery, useGetCandidateByIdQuery, useInsertCandidateMutation } = CandidateApisService;
