import { FC, useState } from "react";
import { Modal, Upload } from "antd";
import { UploadFile } from "antd/lib/upload/interface";
import { UploadButton } from "@admin/components/UploadService/UploadButton";
import { RcFile, UploadChangeParam } from "antd/es/upload";
import ImgCrop from "antd-img-crop";

interface CustomUploadProps {
  fileList?: UploadFile[];
  maxCount?: number;
  showUploadList?: boolean;
  beforeUpload?: (file: RcFile, fileList: RcFile[]) => boolean;
  onChange?: (info: UploadChangeParam<UploadFile<any>>) => void;
  aspect?: number;
  multiple?: boolean;
  accept?: string;
}

const defaultType: CustomUploadProps = {
  fileList: [],
  maxCount: 1,
  showUploadList: true,
  beforeUpload: () => false,
  multiple: false,
  aspect: 16 / 9
};

const _CustomUploadImage: FC<CustomUploadProps> = ({ aspect, fileList, ...restProps }) => {
  const [previewVisible, setPreviewVisible] = useState(false);
  const [previewImage, setPreviewImage] = useState("");
  const [previewTitle, setPreviewTitle] = useState("");

  const handlePreview = async (file: UploadFile) => {
    let src = file.url as string;
    if (!src) {
      src = await new Promise((resolve) => {
        const reader = new FileReader();
        reader.readAsDataURL(file.originFileObj as RcFile);
        reader.onload = () => resolve(reader.result as string);
      });
    }
    setPreviewImage(file.url || src);
    setPreviewVisible(true);
    setPreviewTitle(file.name || file.url!.substring(file.url!.lastIndexOf("/") + 1));
  };

  const handleCancelPreview = () => {
    setPreviewVisible(false);
  };

  return (
    <>
      <ImgCrop rotationSlider showGrid aspect={aspect} showReset quality={1}>
        <Upload
          fileList={fileList}
          onPreview={handlePreview}
          {...restProps}
          listType={"picture-card"}
          maxCount={restProps.maxCount}
          multiple={restProps.multiple}
          beforeUpload={restProps.beforeUpload}
        >
          <UploadButton />
        </Upload>
      </ImgCrop>
      <Modal open={previewVisible} footer={null} onCancel={handleCancelPreview} title={previewTitle}>
        <div style={{ width: "100%", paddingTop: `${(1 / (aspect || 16 / 9)) * 100}%`, position: "relative" }}>
          <img
            alt="Preview"
            style={{
              position: "absolute",
              top: 0,
              left: 0,
              width: "100%",
              height: "100%",
              objectFit: "cover"
            }}
            src={previewImage}
          />
        </div>
      </Modal>
    </>
  );
};
_CustomUploadImage.defaultProps = defaultType;
/**
 * @param  {UploadFile[]} fileList List file uploaded
 * @param  {number} maxCount quantity of file uploaded
 * @param  {boolean} showUploadList show list file uploaded
 * @param  {(file:RcFile, fileList:RcFile[])=>boolean} beforeUpload function before upload
 * @param  {(info:UploadChangeParam<UploadFile<any>>)=>void} onChange function when change file
 * @param  {boolean} multiple allow upload multiple file
 * @param  {string} accept accept file type.
 * @returns JSX.Element
 * @example <CustomUploadImage />
 * @example for form.Item
 * <Form.Item label="avatar” name="avatar" getValueFromEvent={normFile} valuePropName="fileList">
 *   <CustomUploadImage />
 *  </Form.Item>
 * @author khanhdoan693@gmail.com
 * @description Custom Upload Image Component.
 */
export const CustomUploadImage = _CustomUploadImage;
