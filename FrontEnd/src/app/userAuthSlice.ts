import { createSlice } from "@reduxjs/toolkit";
import { UserResponse } from "~/models/userDto";

const userAuthSlice = createSlice({
  name: "user",
  initialState: {
    loading: false,
    logging: false,
    connect: false,
    user: null as UserResponse | null,
    error: null as string | null,
    isOpenForgotPassword: false,
    isOpenConfirmOTPCode: false,
    email: ""
  },
  reducers: {
    CancelConfirmOTPCode(state) {
      state.isOpenConfirmOTPCode = false;
    },
    CancelForgotPassword(state) {
      state.isOpenForgotPassword = false;
      state.email = "";
    },
    sendActiveCode(state, action) {
      state.isOpenConfirmOTPCode = true;
      state.email = action.payload;
    },
    confirmOTPCode(state) {
      state.isOpenForgotPassword = true;
    }
  }
});

export default userAuthSlice;
