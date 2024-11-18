import { Button, Modal, notification, Result, Typography } from "antd";
import { CloseCircleOutlined, CopyFilled, ExclamationCircleFilled } from "@ant-design/icons";
import { InternalAxiosRequestConfig } from "axios";

const { Text, Paragraph } = Typography;

interface error extends InternalAxiosRequestConfig {
  message: string;
  Authorization: string;
}

interface HandleErrorProps {
  error: error;
  status: number;
  data: any;
}

export function convertFormDataToObject(formData: FormData): Record<string, any> {
  const object: Record<string, any> = {};

  for (const [key, value] of formData.entries()) {
    object[key] = value;
  }

  return object;
}

/**
 * @param props
 * @description One function Handle error when request api
 * @author khanhdoan693@gmail.com
 * @example
 * try {
 *  call api here and to do something
 *  } catch (error) {
 *  await HandleError(error);
 *  }
 */
export const HandleError = (props: HandleErrorProps): Promise<void> => {
  const message = props?.data?.Message;
  const { method, url, Authorization, data } = props.error || {};
  const handleCopy = async () => {
    let requestData: object = {};
    if (data instanceof FormData) {
      requestData = convertFormDataToObject(data);
    } else if (typeof data === "string") {
      requestData = JSON.parse(data);
    }
    const object = {
      currentUrl: window.location.href,
      baseUrl: url,
      status: props.status,
      method,
      messageError: props.error?.message || "",
      messageRespond: message,
      Authorization,
      requestData
    };
    const json = JSON.stringify(object, null, 2);
    try {
      await navigator.clipboard.writeText(json);
      notification.success({ message: "Đã copy" });
    } catch (error) {
      console.error("Error copying to clipboard:", error);
      notification.error({ message: "Lỗi khi copy" });
    }
  };

  return new Promise<void>((resolve) => {
    Modal.error({
      title: "Hệ thống xử lý lỗi",
      width: 900,
      style: { top: 20 },
      zIndex: 1031,
      content: (
        <Result
          icon={<ExclamationCircleFilled />}
          status="error"
          title="Đã phát hiện lỗi trong quá trình xử lý"
          subTitle={
            <Typography.Text type={"secondary"}>
              Vui lòng kiểm tra và chỉnh sửa thông tin trước khi gửi lại <br />
              <Typography.Text strong>hoặc bạn hãy gửi ảnh này cho quản trị viên</Typography.Text>
            </Typography.Text>
          }
          extra={[
            <Button type="primary" key="copy" icon={<CopyFilled />} onClick={() => handleCopy()}>
              Copy JSON và gửi cho quản trị viên
            </Button>
          ]}
        >
          <div className="desc">
            <Paragraph>
              <Text
                strong
                style={{
                  fontSize: 16
                }}
              >
                Nội dung liên quan đến lỗi:
              </Text>
            </Paragraph>
            <Paragraph>
              <CloseCircleOutlined style={{ color: "red" }} /> Status:{" "}
              <Typography.Text type={"danger"}>{props.error?.message || ""}</Typography.Text>
            </Paragraph>
            <Paragraph>
              <CloseCircleOutlined style={{ color: "red" }} /> baseURL: <Typography.Text strong>{url}</Typography.Text>
            </Paragraph>
            <Paragraph>
              <CloseCircleOutlined style={{ color: "red" }} /> CurrentUrl:{" "}
              <Typography.Text strong>{window.location.href}</Typography.Text>
            </Paragraph>
            <Paragraph>
              <CloseCircleOutlined style={{ color: "red" }} /> method:{" "}
              <Typography.Text strong>{method}</Typography.Text>
            </Paragraph>
            <Paragraph>
              <CloseCircleOutlined style={{ color: "red" }} /> isToken:{" "}
              <Typography.Text strong>{Authorization ? "Available" : "Unavailable"}</Typography.Text>
            </Paragraph>
            <Paragraph>
              <CloseCircleOutlined style={{ color: "red" }} /> Message:{" "}
              <Typography.Text type={"danger"}>{message}</Typography.Text>
            </Paragraph>
          </div>
        </Result>
      ),
      afterClose: () => {
        resolve();
      },
      footer: null,
      closable: true
    });
  });
};
