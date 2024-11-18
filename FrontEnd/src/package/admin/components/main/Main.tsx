import { Layout } from "antd";
import React, { Suspense, useState } from "react";
import { Routes } from "react-router-dom";
import { useBreakPoint } from "~/hooks/useBreakpoint";
import { Footer, Header, SideNav } from "@admin/components";
import { LoadingProgress } from "@units/loading/LoadingProgress";

const { Header: AntHeader, Content, Sider } = Layout;

interface IProps {
  children: React.ReactNode;
}

export const Main: React.FC<IProps> = ({ children }) => {
  const breakPoint = useBreakPoint();
  const [isOpenSideBar, setIsOpenSideBar] = useState(false);
  const setting = JSON.parse(localStorage.getItem("setting")!);
  const { styleSideNav } = setting;
  return (
    <Layout
      className={`layout-dashboard`}
      style={{
        minHeight: "100vh"
      }}
    >
      <Sider
        width={isOpenSideBar ? 116 : 240}
        theme="light"
        breakpoint="lg"
        collapsedWidth="0"
        className={`sider-primary ant-layout-sider-primary  ${styleSideNav === "#8a6b6b" ? "active-route" : ""}`}
        style={{
          background: styleSideNav,
          margin: "20px 0 0 10px",
          borderRadius: "12px"
        }}
      >
        <SideNav isOpenSideBar={isOpenSideBar} />
      </Sider>
      <Layout style={{ marginLeft: breakPoint.isDesktop ? (isOpenSideBar ? 120 : 235) : 0 }}>
        {
          <AntHeader>
            <Header isOpenSideBar={isOpenSideBar} setIsOpenSideBar={setIsOpenSideBar} />
          </AntHeader>
        }
        <Content className="content-ant">
          <Suspense
            fallback={
              <div
                style={{
                  display: "grid",
                  placeItems: "center",
                  zIndex: "100",
                  width: "100%",
                  height: "100%",
                  backgroundColor: JSON.parse(localStorage.getItem("setting")!).darkMode ? "#000000" : "#ffffff"
                }}
              >
                <LoadingProgress isDarkMode={JSON.parse(localStorage.getItem("setting")!).darkMode} />
              </div>
            }
          >
            <Routes>{children}</Routes>
          </Suspense>
        </Content>
        <Footer />
      </Layout>
    </Layout>
  );
};
