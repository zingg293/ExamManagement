import { LogoutOutlined, MenuFoldOutlined, MenuUnfoldOutlined, SettingOutlined, UserOutlined } from "@ant-design/icons";
import { Button, Col, Drawer, Dropdown, Menu, Row, Space, Typography } from "antd";
import { Fragment, memo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { deleteCookie } from "~/units";
import { SideNavMobile } from "@admin/components";
import "./Header.css";
import { setting, toggler } from "./svg";
import { globalVariable } from "~/globalVariable";
import { SwitchDarkMode } from "@admin/components/header/SwitchDarkMode";
import { TransferOptions } from "@admin/components/header/TransferOptions";
import { TimeCountDown } from "@admin/components/header/TimeClock";
import { HeaderAutoComplete } from "@admin/components/header/HeaderAutoComplete";
import { useBreakPoint } from "@hooks/useBreakpoint";
import { useGetUserQuery } from "@API/services/UserApis.service";

interface PropsHeader {
  isOpenSideBar: boolean;
  setIsOpenSideBar: (isOpenSideBar: boolean) => void;
}

function _Header({ isOpenSideBar, setIsOpenSideBar }: PropsHeader) {
  const navigate = useNavigate();
  const { data: user } = useGetUserQuery({ fetch: false });
  const [visibleDrawSetting, setVisibleDrawSetting] = useState(false);
  const [visibleDraw, setVisibleDraw] = useState(false);
  const breakpoint = useBreakPoint();
  const menuUserActions = (
    <Menu
      style={{ padding: 10, borderRadius: "5px" }}
      items={[
        {
          label: (
            <Space direction="horizontal" size={"middle"} onClick={() => navigate("/admin/user/setting")}>
              <SettingOutlined />
              <Typography.Text>Cài đặt</Typography.Text>
            </Space>
          ),
          key: "1"
        },
        {
          type: "divider"
        },
        {
          label: (
            <Space
              direction="horizontal"
              size={"middle"}
              onClick={() => {
                deleteCookie("jwt");
                return (window.location.href = globalVariable.pathNameLogin);
              }}
            >
              <LogoutOutlined></LogoutOutlined>
              <Typography.Text>Đăng xuất</Typography.Text>
            </Space>
          ),
          key: "2"
        }
      ]}
    />
  );

  return (
    <Fragment>
      <div
        tabIndex={0}
        role={"button"}
        className="setting-drwer"
        onClick={() => setVisibleDrawSetting(true)}
        onKeyDown={() => setVisibleDrawSetting(true)}
      >
        {setting}
      </div>
      <Row>
        <Col span={24} md={12} hidden={breakpoint.isMobile}>
          <Space direction="horizontal" size={"large"} align="center">
            {isOpenSideBar ? (
              <MenuUnfoldOutlined
                style={{
                  fontSize: 28,
                  cursor: "pointer",
                  color: JSON.parse(localStorage.getItem("setting") || "")?.PrimaryColor
                }}
                className="btn-open-close-sidebar"
                onClick={() => {
                  setIsOpenSideBar(false);
                }}
              />
            ) : (
              <MenuFoldOutlined
                style={{
                  fontSize: 28,
                  cursor: "pointer",
                  color: JSON.parse(localStorage.getItem("setting") || "")?.PrimaryColor
                }}
                className="btn-open-close-sidebar"
                onClick={() => {
                  setIsOpenSideBar(true);
                }}
              />
            )}
            <TimeCountDown />
          </Space>
        </Col>
        <Col span={24} md={12} className="header-control">
          <Space size={1} wrap>
            <HeaderAutoComplete />
            {/*<HeaderLanguageSelect />*/}
            <Dropdown overlay={menuUserActions} trigger={["click"]}>
              <Button
                type={"link"}
                icon={
                  <UserOutlined
                    style={{
                      fontSize: 28,
                      cursor: "pointer",
                      color: JSON.parse(localStorage.getItem("setting") || "")?.PrimaryColor
                    }}
                  />
                }
              >
                <Typography.Text>{user?.payload?.data?.fullname}</Typography.Text>
              </Button>
            </Dropdown>
            <Drawer
              title="Danh mục"
              placement={"left"}
              onClose={() => {
                setVisibleDraw(false);
              }}
              open={visibleDraw}
              width={"80%"}
            >
              <SideNavMobile setVisibleDraw={setVisibleDraw} visibleDraw={visibleDraw} />
            </Drawer>
            <Button
              type="link"
              className="sidebar-toggler"
              onClick={() => {
                setVisibleDraw(true);
              }}
            >
              {toggler}
            </Button>
            {/*this is hidden Notification*/}
            {/*<HeaderDrawNotification />*/}
            <TransferOptions setVisibleDrawSetting={setVisibleDrawSetting} visibleDrawSetting={visibleDrawSetting} />
            <SwitchDarkMode />
          </Space>
        </Col>
      </Row>
    </Fragment>
  );
}

export const Header = memo(_Header);
