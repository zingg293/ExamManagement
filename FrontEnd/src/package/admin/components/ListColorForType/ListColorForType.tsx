import { Tag } from "antd";
import React from "react";

export function ListColorForType({ type }: { type: number }): React.JSX.Element {
  switch (type) {
    case 0:
      return <Tag color="blue-inverse">Mua mới</Tag>;
    case 1:
      return <Tag color="#0000FF">PBCM đã chuyển HR</Tag>;
    case 2:
      return <Tag color="#FF0000">Hr từ chối</Tag>;
    case 3:
      return <Tag color="#008000">HR đã chuyển BGĐ</Tag>;
    case 4:
      return <Tag color="#FFA500">BGĐ từ chối</Tag>;
    case 5:
      return <Tag color="#800080">BGĐ đã duyệt</Tag>;
    case 6:
      return <Tag color="green">Hoàn thành</Tag>;
    default:
      return <></>;
  }
}
