import React from "react";
import { createRoot } from "react-dom/client";
import { ConfigProvider, theme } from "antd";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import { store } from "./app/store";
import lang_vn from "antd/es/locale/vi_VN";
import "antd/dist/reset.css";
import "dayjs/locale/vi";
import "./styles/main.css";
import "./styles/responsive.css";
import "./styles/custom.css";
import "./index.css";
import dayjs from "dayjs";
import { reportWebVitals } from "~/reportWebVitals";
import AppRoot from "~/AppRoot";

dayjs.locale("vi");
(() => {
  const initSetting = {
    PrimaryColor: "#1890ff",
    BorderRadius: 8,
    FontFamily: "",
    darkMode: true,
    compactAlgorithm: false,
    fixedNavbar: true,
    styleSideNav: "transparent",
    RandomPrimaryColorEachDay: false
  };
  if (!localStorage.getItem("setting")) {
    localStorage.setItem("setting", JSON.stringify(initSetting));
  }
})();

const localSetting = localStorage.getItem("setting")!;
const algorithm =
  JSON.parse(localSetting).darkMode && !JSON.parse(localSetting).compactAlgorithm
    ? theme.darkAlgorithm
    : JSON.parse(localSetting).darkMode && JSON.parse(localSetting).compactAlgorithm
    ? [theme.compactAlgorithm, theme.darkAlgorithm]
    : !JSON.parse(localSetting).darkMode && !JSON.parse(localSetting).compactAlgorithm
    ? theme.defaultAlgorithm
    : theme.compactAlgorithm;

const rootApp = (
  <React.StrictMode>
    <BrowserRouter>
      <Provider store={store}>
        <ConfigProvider
          locale={lang_vn}
          theme={{
            token: {
              colorIconHover: JSON.parse(localSetting)?.PrimaryColor || "#1890ff",
              // colorIcon: JSON.parse(localSetting)?.PrimaryColor || "#1890ff",
              colorPrimary: JSON.parse(localSetting)?.PrimaryColor || "#1890ff",
              colorLink: JSON.parse(localSetting)?.PrimaryColor || "#1890ff",
              borderRadius: JSON.parse(localSetting).BorderRadius || 8,
              fontFamily:
                JSON.parse(localSetting).FontFamily ||
                "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, 'Noto Sans', sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol', 'Noto Color Emoji'"
            },
            algorithm: algorithm
          }}
        >
          <AppRoot />
        </ConfigProvider>
      </Provider>
    </BrowserRouter>
  </React.StrictMode>
);

const rootElement = document.getElementById("asp.net")!;
createRoot(rootElement).render(rootApp);
reportWebVitals(console.table);
