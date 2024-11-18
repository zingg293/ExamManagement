import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Divider, Drawer, DrawerProps } from "antd";

function _DrawerContent(props: DrawerProps) {
  const { PrimaryColor } = JSON.parse(localStorage.getItem("setting")!);
  const { children } = props;

  return (
    <div className="DrawerContent">
      <Drawer {...props}>
        <Divider
          style={{
            margin: "-25px -24px 20px -24px",
            background: PrimaryColor,
            width: "calc(100% + 48px)",
            height: 16
          }}
        />
        {children}
      </Drawer>
    </div>
  );
}
export const DrawerContent = WithErrorBoundaryCustom(_DrawerContent);
