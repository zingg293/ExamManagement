import { CloudUploadOutlined } from "@ant-design/icons";

interface UploadButtonProps {
  content?: string;
}

const defaultType: UploadButtonProps = {
  content: "Upload"
};

const _UploadButton = (props: UploadButtonProps) => {
  const { content } = props;
  return (
    <div
      style={{
        width: "500px"
      }}
    >
      <CloudUploadOutlined />
      <div style={{ marginTop: 8 }}>{content}</div>
    </div>
  );
};

export const normFile = (e: any) => {
  if (Array.isArray(e)) {
    return e;
  }
  return e?.fileList;
};
_UploadButton.defaultProps = defaultType;
/**
 * @param  {string} content content of button
 * @returns JSX.Element
 * @example <UploadButton content="Upload" />
 * @author khanhdoan693@gmail.com
 */
export const UploadButton = _UploadButton;
