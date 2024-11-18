import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { RoleDto, RoleDTO } from "@models/roleDTO";

export const RoleApisService = createApi({
  reducerPath: "RoleApisService",
  tagTypes: ["RoleApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListRole: builder.query<ListResponse<RoleDto>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/role/getListRole?pageNumber=${pageNumber}&pageSize=${pageSize}`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "RoleApisService" as const, id })),
            {
              type: "RoleApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "RoleApisService", id: "LIST" }];
      }
    }),
    GetRoleById: builder.query<ListResponse<RoleDTO>, { id: string }>({
      query: ({ id }): any => ({
        url: `/role/getRoleAndNavigationByIdRole?idRole=${id}`
      }),
      providesTags: (result, error, arg) => [{ type: "RoleApisService", id: arg.id }]
    }),
    InsertRole: builder.mutation<payloadResult, { role: Partial<RoleDTO> }>({
      query: ({ role }) => ({
        url: `/role/insertRoleAndNavigation`,
        method: "POST",
        data: role
      }),
      invalidatesTags: (result) => (result ? [{ type: "RoleApisService", id: "LIST" }] : [])
    }),
    UpdateRole: builder.mutation<payloadResult, { role: Partial<RoleDTO> }>({
      query: ({ role }) => ({
        url: `/role/updateRoleAndNavigation`,
        method: "PUT",
        data: role
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "RoleApisService", id: arg.role.role?.id }] : [])
    }),
    DeleteRole: builder.mutation<payloadResult, { idRole: string[] }>({
      query: ({ idRole }) => ({
        url: `/role/deleteRoleAndNavigation`,
        method: "DELETE",
        data: idRole
      }),
      invalidatesTags: (result) => (result ? [{ type: "RoleApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListRoleQuery,
  useGetRoleByIdQuery,
  useInsertRoleMutation,
  useUpdateRoleMutation,
  useDeleteRoleMutation
} = RoleApisService;
