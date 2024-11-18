import { createFromIconfontCN } from "@ant-design/icons";
import { FC } from "react";

const Icon = createFromIconfontCN({
  scriptUrl: "//at.alicdn.com/t/c/font_4106631_j7hsxg9m5v.js"
});

type definition = [
  "icon-excel",
  "icon-check-circle-fill",
  "icon-check",
  "icon-print",
  "icon-WORD",
  "icon-contact",
  "icon-dashboard",
  "icon-Account",
  "icon-cloud-download"
];

interface IProps {
  type: definition[number] | string;
  color?: string;
  fontSize?: string;
  fill?: string;
}

/**
 * @param {definition[number] | string}type - icon type
 * @param {string}color - icon color
 * @param {string}fontSize - icon font size
 * @param {string}fill - icon fill
 * @returns {ReactNode}
 * @author: khanhdoan693@gmail.com
 * @description: one components for all icon
 * @example: <MyIcon type="icon-excel" color="#1890ff" fontSize="24px" fill="#1890ff" />
 */
export const MyIcon: FC<IProps> = ({ type, color, fontSize, fill }) => (
  <Icon
    type={type}
    style={{
      fontSize: fontSize || "24px",
      color: color,
      fill: fill
    }}
  />
);
