import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { CompanyInformationDTO } from "@models/companyInformationDTO";
import { globalVariable } from "~/globalVariable";

export const CompanyInformationApisService = createApi({
  reducerPath: "CompanyInformationApisService",
  tagTypes: ["CompanyInformationApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCompanyInformation: builder.query<ListResponse<CompanyInformationDTO>, pagination>({
      query: (pagination) => ({
        url: `/companyInformation/getListCompanyInformation`,
        params: pagination,
        method: "GET"
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CompanyInformationApisService" as const, id })),
            {
              type: "CompanyInformationApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CompanyInformationApisService", id: "LIST" }];
      }
    }),

    GetCompanyInformationById: builder.query<ListResponse<CompanyInformationDTO>, { idCompanyInformation: string }>({
      query: ({ idCompanyInformation }) => ({
        url: `/companyInformation/getCompanyInformationById`,
        params: { idCompanyInformation },
        method: "GET"
      }),
      providesTags: (result, error, arg) => [{ type: "CompanyInformationApisService", id: arg.idCompanyInformation }]
    }),

    UpdateCompanyInformation: builder.mutation<payloadResult, CompanyInformationDTO>({
      query: (companyInformation) => ({
        url: `/companyInformation/updateCompanyInformation`,
        method: "PUT",
        data: companyInformation
      }),
      invalidatesTags: (result) => (result ? [{ type: "CompanyInformationApisService", id: "LIST" }] : [])
    }),

    InsertCompanyInformation: builder.mutation<payloadResult, CompanyInformationDTO>({
      query: (companyInformation) => ({
        url: `/companyInformation/insertCompanyInformation`,
        method: "POST",
        data: companyInformation
      }),
      invalidatesTags: (result) => (result ? [{ type: "CompanyInformationApisService", id: "LIST" }] : [])
    })
  })
});

export const getFileImageCompanyInformation = (fileNameId: string) => {
  return `${globalVariable.urlServerApi}/api/v1/companyInformation/getFileImage?fileNameId=${fileNameId}`;
};
export const {
  useGetListCompanyInformationQuery,
  useGetCompanyInformationByIdQuery,
  useUpdateCompanyInformationMutation,
  useInsertCompanyInformationMutation
} = CompanyInformationApisService;
