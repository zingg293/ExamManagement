import WithErrorBoundaryCustom from "~/units/errorBounDary/WithErrorBoundaryCustom";
import { NavLink, useLocation } from "react-router-dom";
import logo from "@admin/asset/logo/logo-daiphatdat.png";
import { Divider, Image, Menu, MenuProps, Spin, Typography } from "antd";
import { DataNavigate, MyIcon } from "@admin/components";
import React from "react";

interface IProps {
  visibleDraw: boolean;
  setVisibleDraw: (value: boolean) => void;
}

type MenuItem = Required<MenuProps>["items"][number];

function getItem(
  label: React.ReactNode,
  key: React.Key,
  icon?: React.ReactNode,
  children?: MenuItem[],
  type?: "group"
): MenuItem {
  return {
    key,
    icon,
    children,
    label,
    type
  } as MenuItem;
}

function _SideNavMobile(props: IProps) {
  const { pathname } = useLocation();
  const { visibleDraw, setVisibleDraw } = props;
  const setting = localStorage.getItem("setting")!;
  const color = JSON.parse(setting)?.PrimaryColor;
  const generalSettingsItem = DataNavigate();
  const page = pathname?.replace("/", "");
  const handleVisibleDraw = () => {
    if (visibleDraw) {
      setVisibleDraw(false);
    }
  };
  const elementMenu = generalSettingsItem.map((items, index) =>
    getItem(
      items?.navigation?.menuName,
      `submenu-${index + 1}`,
      null,
      items?.navigationsChild?.map((item) =>
        getItem(
          <NavLink
            to={item?.path}
            onClick={() => {
              handleVisibleDraw();
            }}
          >
            <Typography.Text
              strong
              style={{
                color: page === item.path ? color : ""
              }}
            >
              {item?.menuName}
            </Typography.Text>
          </NavLink>,
          `submenu-item-${item.path}`,
          item?.icon
        )
      )
    )
  );
  const menuSideNavMobile: MenuItem[] = [
    getItem(
      <NavLink
        to="/admin/dashboard"
        onClick={() => {
          handleVisibleDraw();
        }}
      >
        <Typography.Text
          strong
          style={{
            color: page === "admin/dashboard" ? color : ""
          }}
        >
          Trang chủ
        </Typography.Text>
      </NavLink>,
      "1",
      <MyIcon type={"icon-dashboard"} fontSize={"20px"} fill={color} />
    ),
    ...elementMenu
  ];

  return (
    <div className="SideNavMobile">
      <div className="brand" style={{ textAlign: "center" }}>
        <Image src={logo} preview={false} width={150} height={110} />
      </div>
      <Divider />
      <Spin spinning={false} size={"large"}>
        <Menu mode={"inline"} items={menuSideNavMobile} />
      </Spin>
    </div>
  );
}

export const SideNavMobile = WithErrorBoundaryCustom(_SideNavMobile);
