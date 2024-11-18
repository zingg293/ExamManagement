import React from "react";
import { Upload } from "antd";
import { UploadFile } from "antd/lib/upload/interface";
import { CloudUploadOutlined } from "@ant-design/icons";
import { RcFile } from "antd/es/upload";

interface CustomUploadFileDragProps {
  fileList?: UploadFile[];
  content?: string;
  maxCount?: number;
  multiple?: boolean;
  accept?: string;
  beforeUpload?: (file: RcFile, fileList: RcFile[]) => boolean;
}

const defaultType: CustomUploadFileDragProps = {
  fileList: [],
  content: "Nhấp hoặc kéo tệp vào khu vực này để tải lên",
  maxCount: 1,
  multiple: false,
  beforeUpload: () => false
};

const _CustomUploadFileDrag: React.FC<CustomUploadFileDragProps> = ({ content, fileList, ...restProps }) => {
  return (
    <Upload.Dragger fileList={fileList} beforeUpload={restProps.beforeUpload} {...restProps} listType="picture">
      <p className="ant-upload-drag-icon">
        <CloudUploadOutlined />
      </p>
      <p className="ant-upload-text">{content}</p>
      <p className="ant-upload-hint">
        Hỗ trợ tải lên một lần hoặc hàng loạt. Nghiêm cấm tải lên dữ liệu độc hại hoặc các tệp bị cấm khác.
      </p>
    </Upload.Dragger>
  );
};
_CustomUploadFileDrag.defaultProps = defaultType;
/**
 * @param  {UploadFile[]} fileList List file uploaded
 * @param  {string} content content of drag area
 * @param  {number} maxCount quantity of file uploaded
 * @param  {boolean} multiple allow upload multiple file
 * @param  {string} accept accept file type.
 * @param  {(file:RcFile, fileList:RcFile[])=>boolean} beforeUpload function before upload.
 * @returns JSX.Element
 * @example <CustomUploadFileDrag multiple={true} />
 * @example for form.Item
 *<Form.Item label="file” name="file" getValueFromEvent={normFile} valuePropName="fileList">
 *   <CustomUploadFileDrag multiple={true} />
 *</Form.Item>
 * @author khanhdoan693@gmail.com
 * @description Upload file by a drag and drop.
 */
export const CustomUploadFileDrag = _CustomUploadFileDrag;
