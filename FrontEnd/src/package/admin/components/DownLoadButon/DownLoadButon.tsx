import React from "react";
import { Button } from "antd";
import { MyIcon } from "@admin/components";

interface DownloadButtonProps {
  downloadUrl: string;
  title?: string;
  size?: "small" | "middle" | "large";
  type?: "primary" | "dashed" | "link" | "text" | "default";
}

const defaultType: DownloadButtonProps = {
  type: "primary",
  downloadUrl: "",
  title: "Tải xuống"
};

const _DownloadButton: React.FC<DownloadButtonProps> = ({ downloadUrl, title, size, type }) => {
  const [isDownload, setIsDownload] = React.useState(false);
  const handleDownload = async () => {
    try {
      setIsDownload(true);
      const response = await fetch(downloadUrl);
      const blob = await response.blob();
      const url = URL.createObjectURL(blob);

      const link = document.createElement("a");
      link.href = url;
      link.target = "_blank";
      link.rel = "noopener noreferrer";
      link.click();

      URL.revokeObjectURL(url);
    } catch (e) {
      console.log("err => ", e);
    } finally {
      setIsDownload(false);
    }
  };

  return (
    <Button
      loading={isDownload}
      onClick={handleDownload}
      icon={<MyIcon type={"icon-cloud-download"} fontSize={"1em"} />}
      size={size}
      type={type}
    >
      {isDownload ? "Downloading" : title}
    </Button>
  );
};
_DownloadButton.defaultProps = defaultType;
/**
 * @param {string} downloadUrl
 * @param {string} title
 * @param {"small" | "middle" | "large"} size
 * @param {"primary" | "ghost" | "dashed" | "link" | "text" | "default"} type
 * @returns {React.FC<DownloadButtonProps>}
 * @example <DownloadButton downloadUrl="https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf" />
 * @author khanhdoan693@gmail.com
 * @description Download file by a button.
 */
export const DownloadButton = _DownloadButton;
