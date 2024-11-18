import React from "react";
import { BarsOutlined, DownOutlined } from "@ant-design/icons";
import { Dropdown, Button, Menu } from "antd";

interface DropOptionProps {
  onMenuClick: (e: any) => void;
  menuOptions: Array<{ key: string; name: string }>;
  buttonStyle?: React.CSSProperties;
  dropdownProps?: any;
}

export function DropOption(props: DropOptionProps) {
  const { onMenuClick, menuOptions = [], buttonStyle, dropdownProps } = props;
  const menu = menuOptions.map((item) => <Menu.Item key={item.key}>{item.name}</Menu.Item>);
  return (
    <Dropdown overlay={<Menu onClick={onMenuClick}>{menu}</Menu>} {...dropdownProps}>
      <Button style={{ border: "none", ...buttonStyle }}>
        <BarsOutlined style={{ marginRight: 2 }} />
        <DownOutlined />
      </Button>
    </Dropdown>
  );
}
