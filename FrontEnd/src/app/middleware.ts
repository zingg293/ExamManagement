import { AnyAction, isRejectedWithValue, Middleware } from "@reduxjs/toolkit";
import { notification } from "antd";
import { ListResponse } from "@models/common";

function isPayloadErrorMessage(payload: unknown): payload is {
  data: {
    message: string;
    success: boolean;
  };
  status: number;
} {
  return (
    typeof payload === "object" &&
    payload !== null &&
    typeof (payload as any).data?.message === "string" &&
    typeof (payload as any).data?.success === "boolean" &&
    typeof (payload as any).status === "number"
  );
}

// function isPayloadErrorMessage2(payload: unknown): payload is {
//   fail?: boolean;
//   message: string;
//   success: boolean;
//   totalElement?: number;
// } {
//   const obj = payload as {
//     fail?: boolean;
//     message: string;
//     success: boolean;
//     totalElement?: number;
//   };
//   return (
//     typeof obj === "object" &&
//     obj !== null &&
//     true &&
//     typeof obj.success === "boolean" &&
//     (typeof obj.fail === "undefined" || typeof obj.fail === "boolean") &&
//     (typeof obj.totalElement === "undefined" || typeof obj.totalElement === "number")
//   );
// }
function isApiResponse(data: any): data is ListResponse<any> {
  return (
    typeof data === "object" &&
    data !== null &&
    "message" in data &&
    "success" in data &&
    "pageNumber" in data &&
    "pageSize" in data &&
    "totalElement" in data &&
    "totalPages" in data
  );
}

export const rtkQueryErrorLogger: Middleware = () => (next) => (action: AnyAction) => {
  const { payload } = action;
  if (isRejectedWithValue(action) && isPayloadErrorMessage(payload)) {
    notification.warning({
      message: "Cảnh báo",
      description: payload.data.message,
      placement: "top"
    });
  }
  if (isApiResponse(payload)) {
    if (payload.payload || payload?.listPayload) return next(action);
    !payload.success
      ? notification.warning({
          message: "Cảnh báo",
          description: payload.message,
          placement: "top"
        })
      : notification.success({
          message: "Thành công",
          description: payload.message,
          placement: "top"
        });
  }
  return next(action);
};
