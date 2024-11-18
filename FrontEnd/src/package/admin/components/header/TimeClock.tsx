import { useNetWork } from "@hooks/useNetWork";
import { useClock } from "~/hooks";
import { Space, Typography } from "antd";
import { connect, disconnect } from "@admin/components/header/svg";

export function TimeCountDown() {
  const colorPrimary = JSON.parse(localStorage.getItem("setting")!)?.PrimaryColor;
  const internetStatus = useNetWork();
  const { Timer, Day } = useClock();

  return (
    <Space direction="horizontal" size={"middle"}>
      <Typography.Title
        level={4}
        style={{
          height: "100%",
          transform: "translateY(8px)"
        }}
      >
        {Day}
      </Typography.Title>
      <Typography.Title
        level={3}
        style={{
          height: "100%",
          transform: "translateY(8px)",
          color: colorPrimary,
          fontWeight: "bold",
          fontSize: 26
        }}
      >
        {Timer}
      </Typography.Title>

      {!internetStatus ? disconnect : connect}
      {!internetStatus ? (
        <Typography.Title
          level={5}
          style={{
            height: "100%",
            transform: "translateY(8px)",
            color: "red"
          }}
        >
          Mất kết nối...
        </Typography.Title>
      ) : (
        ""
      )}
    </Space>
  );
}
