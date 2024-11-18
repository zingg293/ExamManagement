import { lazy } from "react";

export const LoginAdminLayout = lazy(() =>
  import("@admin/features/auth").then((module) => ({
    default: module.LoginAdminLayout
  }))
);
export const DashBoardLayout = lazy(() =>
  import("@admin/features/dashBoard/page/DashBoardLayout").then((module) => ({
    default: module.DashBoardLayout
  }))
);
export const UserManage = lazy(() =>
  import("@admin/features/userManage/page/UserManage").then((module) => ({
    default: module.UserManage
  }))
);
export const UserSetting = lazy(() =>
  import("~/package/admin/components").then((module) => ({ default: module.UserSetting }))
);
