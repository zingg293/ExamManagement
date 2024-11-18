import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "../http/axiosBaseQuery";
import { ListResponse, payloadResult } from "~/models/common";
import { UserLoginRequest, UserLoginResponse } from "@models/userAuth";

export const AuthApisService = createApi({
  reducerPath: "AuthApisService",
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    Login: builder.mutation<ListResponse<UserLoginResponse>, { user: Partial<UserLoginRequest> }>({
      query: ({ user }) => ({
        url: `/User/Login`,
        method: "POST",
        data: user
      })
    }),
    Register: builder.mutation<payloadResult, { user: Partial<UserLoginRequest> }>({
      query: ({ user }) => ({
        url: `/User/Register`,
        method: "POST",
        data: user
      })
    })
  })
});
export const { useLoginMutation, useRegisterMutation } = AuthApisService;
