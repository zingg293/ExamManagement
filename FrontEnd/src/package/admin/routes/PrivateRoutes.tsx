import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { deleteCookie, getCookie } from "~/units";
import { useGetUserQuery } from "@API/services/UserApis.service";
import { globalVariable } from "~/globalVariable";
import { App } from "antd";

export function PrivateRoutes(): any {
  const { notification } = App.useApp();
  const navigate = useNavigate();
  const location = useLocation();
  const { data: user, isLoading } = useGetUserQuery({ fetch: false });
  if (isLoading) return null;

  if (!getCookie("jwt")) return (window.location.href = globalVariable.pathNameLogin);

  if (user?.success === false) {
    deleteCookie("jwt");
    notification.warning({
      message: "Ngăn chặn truy cập",
      description: "Phiên làm việc của bạn đã hết hạn, vui lòng đăng nhập lại"
    });
    return navigate(globalVariable.pathNameLogin, {
      state: { from: location.pathname }
    });
  }

  return <Outlet />;
}
