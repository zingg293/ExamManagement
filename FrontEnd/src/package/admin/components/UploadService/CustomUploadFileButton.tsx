import { FC } from "react";
import { Button, Upload } from "antd";
import { UploadFile } from "antd/lib/upload/interface";
import { RcFile, UploadChangeParam } from "antd/es/upload";
import { CloudUploadOutlined } from "@ant-design/icons";

interface CustomUploadProps {
  fileList?: UploadFile[];
  maxCount?: number;
  showUploadList?: boolean;
  beforeUpload?: (file: RcFile, fileList: RcFile[]) => boolean;
  onChange?: (info: UploadChangeParam<UploadFile<any>>) => void;
  multiple?: boolean;
  accept?: string;
}

const defaultType: CustomUploadProps = {
  fileList: [],
  maxCount: 1,
  showUploadList: true,
  beforeUpload: () => false,
  multiple: false
};

const _CustomUploadFileButton: FC<CustomUploadProps> = ({ fileList, ...restProps }) => {
  return (
    <>
      <Upload
        fileList={fileList}
        {...restProps}
        listType="picture"
        maxCount={restProps.maxCount}
        multiple={restProps.multiple}
        beforeUpload={restProps.beforeUpload}
      >
        <Button icon={<CloudUploadOutlined />}>Nhấp để tải lên</Button>
      </Upload>
    </>
  );
};
_CustomUploadFileButton.defaultProps = defaultType;
/**
 * @param  {UploadFile[]} fileList List file uploaded
 * @param  {number} maxCount quantity of file uploaded
 * @param  {boolean} showUploadList show list file uploaded
 * @param  {(file:RcFile, fileList:RcFile[])=>boolean} beforeUpload function before upload
 * @param  {(info:UploadChangeParam<UploadFile<any>>)=>void} onChange function when change file
 * @param  {boolean} multiple allow upload multiple file
 * @param  {string} accept accept file type.
 * @returns JSX.Element
 * @example <CustomUploadFileButton multiple={true} />
 * @example for form.Item
 *<Form.Item label="file” name="file" getValueFromEvent={normFile} valuePropName="fileList">
 *  <CustomUploadFileDrag multiple={true} />
 *</Form.Item>
 * @author khanhdoan693@gmail.com
 * @description Upload file by a button.
 */
export const CustomUploadFileButton = _CustomUploadFileButton;
