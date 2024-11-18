import React, { useEffect, useState } from "react";
import Loading from "@units/loading/loading";
import { App } from "antd";
import { Admin } from "~/Routes";

export default function AppRoot(): React.JSX.Element {
  const [isAppMounted, setAppMounted] = useState(false);
  useEffect(() => {
    const startTime = window.performance.now();
    const endTime = window.performance.now();
    const loadingTime = endTime - startTime;
    setTimeout(() => {
      setAppMounted(true);
    }, Math.max(1000 - loadingTime, 0));
  }, []);

  if (!isAppMounted) {
    return (
      <div
        style={{
          display: "grid",
          placeItems: "center",
          zIndex: "100",
          width: "100vw",
          height: "100vh",
          backgroundColor: JSON.parse(localStorage.getItem("setting")!).darkMode ? "#000000" : "#ffffff"
        }}
      >
        <Loading />
      </div>
    );
  }

  return (
    <App>
      <Admin />
    </App>
  );
}
