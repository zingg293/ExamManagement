import { Button, Image, Menu, message, Spin, Tooltip } from "antd";
import { MenuProps } from "rc-menu";
import React from "react";
import { NavLink, useLocation } from "react-router-dom";
import { DataNavigate } from "./DataNavigate";
import "./style.css";
import { dashboard, MyIcon } from "@admin/components";
import { InformationCompany } from "~/globalVariable";
import { newLogoDPD } from "@admin/asset/logo";

interface IProps {
  isOpenSideBar: boolean;
}

export const SideNav: React.FC<IProps> = ({ isOpenSideBar }) => {
  const { pathname } = useLocation();
  const setting = localStorage.getItem("setting")!;
  const color = JSON.parse(setting)?.PrimaryColor;
  const generalSettingsItem = DataNavigate();
  const page = pathname?.replace("/", "/");
  const menuSideNav: MenuProps["items"] = [
    {
      label: (
        <div>
          <NavLink to="/admin/dashboard">
            <span
              className="anticon anticon-code icon ant-menu-item-icon"
              style={{
                background: page === "/admin/dashboard" ? color : "",
                margin: 0
              }}
            >
              {dashboard(color)}
            </span>
            <span className="label" style={{ display: isOpenSideBar ? "none" : "inline-block" }}>
              Trang chủ
            </span>
          </NavLink>
        </div>
      ),
      key: "item-1"
    },
    ...generalSettingsItem.map((items) => {
      return {
        label: (
          <Tooltip
            key={items?.navigation?.id}
            title={items?.navigation?.menuName}
            color={"#87d068"}
            placement="rightTop"
          >
            {!isOpenSideBar ? (
              <span
                style={{
                  color: "#8c8c8c",
                  fontWeight: 700,
                  fontSize: 13,
                  textTransform: "uppercase",
                  display: "block",
                  overflow: "hidden",
                  marginLeft: -12
                }}
              >
                {items?.navigation?.menuName}
              </span>
            ) : (
              <span
                style={{
                  color: "#8c8c8c",
                  fontWeight: 700,
                  fontSize: 25,
                  textTransform: "uppercase",
                  display: "block",
                  overflow: "hidden",
                  marginLeft: -12
                }}
              >
                {items?.navigation?.menuName?.charAt(0)}
              </span>
            )}
          </Tooltip>
        ),
        key: `submenu-${items?.navigation?.id + 1}`,
        children: items?.navigationsChild?.map((item) => {
          return {
            label: (
              <div>
                <Tooltip title={item?.menuName} color={"#87d068"} placement="rightTop">
                  <NavLink to={item?.path}>
                    <span
                      className="anticon anticon-code icon ant-menu-item-icon"
                      style={{
                        background: page === item?.path ? color : "",
                        margin: 0
                      }}
                    >
                      {item?.icon}
                    </span>
                    <span className="label" style={{ display: isOpenSideBar ? "none" : "inline-block", width: 100 }}>
                      {item?.menuName}
                    </span>
                  </NavLink>
                </Tooltip>
              </div>
            ),
            key: `submenu-item-${item.id}`
          };
        })
      };
    })
  ];

  return (
    <div className={`sidebar`}>
      <div className="brand" style={{ textAlign: "center" }}>
        {!isOpenSideBar ? (
          <Image
            src={newLogoDPD}
            preview={false}
            width={150}
            height={110}
            style={{
              objectFit: "contain"
            }}
          />
        ) : (
          <Image
            src={newLogoDPD}
            preview={false}
            width={40}
            height={40}
            style={{
              objectFit: "contain"
            }}
          />
        )}
      </div>
      <hr />
      <Spin spinning={false} size={"large"}>
        <Menu theme="light" mode="inline" items={menuSideNav} />
      </Spin>
      <div className="aside-footer">
        <div
          className="footer-box"
          style={{
            background: color
          }}
        >
          <span className="icon" style={{ color }}>
            <MyIcon type="icon-contact" fontSize={"20px"} />
          </span>
          <h6 style={{ display: isOpenSideBar ? "none" : "block" }}>Cần giúp đỡ?</h6>
          <p style={{ display: isOpenSideBar ? "none" : "block" }}>Vui lòng liên hệ chúng tôi</p>
          <Button
            style={{ display: isOpenSideBar ? "none" : "block" }}
            type="primary"
            className="ant-btn-block"
            onClick={async () => {
              await navigator.clipboard.writeText(InformationCompany.phone);
              message.success("Số điện thoại đã được copy vào bộ nhớ tạm");
            }}
          >
            {InformationCompany.phoneDisplay}
          </Button>
        </div>
      </div>
    </div>
  );
};
