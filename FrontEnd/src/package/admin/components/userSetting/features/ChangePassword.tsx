import { Button, Card, Divider, Form, Input, Space, Typography } from "antd";
import { HandleError } from "@admin/components";
import { useChangePasswordMutation } from "@API/services/UserApis.service";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";

function _ChangePassword({ email }: { email?: string }) {
  const [formRef] = Form.useForm();
  const [changePasswordUser, { isLoading: isloadingChangePassword }] = useChangePasswordMutation();

  const onFinish = async (values: { password: string; newpassword: string }) => {
    try {
      if (email && values) {
        const { password, newpassword } = values;
        const result = await changePasswordUser({ email, oldPassword: password, newPassword: newpassword }).unwrap();
        if (result.success) {
          formRef?.resetFields();
        }
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="ChangePassword">
      <Card bordered={false} className="criclebox h-full">
        <Typography.Title level={4}>Thay đổi mật khẩu</Typography.Title>
        <Divider />
        <Form layout="vertical" onFinish={onFinish} form={formRef}>
          <Form.Item
            required
            name={"password"}
            label="Mật khẩu hiện tại"
            rules={[
              {
                required: true,
                message: "Vui lòng nhập mật khẩu hiện tại"
              }
            ]}
          >
            <Input.Password allowClear />
          </Form.Item>
          <Form.Item
            required
            name={"newpassword"}
            label="Mật khẩu mới"
            rules={[
              {
                required: true,
                message: "Vui lòng nhập mật khẩu mới"
              },
              {
                min: 8,
                message: "Mật khẩu phải có ít nhất 8 ký tự"
              },
              {
                max: 32,
                message: "Mật khẩu không được quá 32 ký tự"
              },
              {
                pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,32}$/,
                message: "Mật khẩu phải có ít nhất 1 chữ hoa, 1 chữ thường và 1 số, 1 ký tự đặc biệt"
              },
              {
                pattern: /^(?!.*\s).*$/,
                message: "Mật khẩu không được có khoảng trắng"
              }
            ]}
          >
            <Input.Password allowClear />
          </Form.Item>
          <Form.Item
            required
            name={"renewpassword"}
            label="Nhập lại mật khẩu mới"
            rules={[
              {
                required: true,
                message: "Vui lòng nhập lại mật khẩu mới"
              },
              // renenewpassword === newpassword
              ({ getFieldValue }) => ({
                validator(rule, value) {
                  if (!value || getFieldValue("newpassword") === value) {
                    return Promise.resolve();
                  }
                  return Promise.reject("Mật khẩu nhập lại không khớp");
                }
              })
            ]}
          >
            <Input.Password allowClear />
          </Form.Item>
          <Typography.Title level={4}>Yêu cầu mật khẩu</Typography.Title>
          <Typography.Text type="secondary">Vui lòng làm theo hướng dẫn sau để tạo mật khẩu mới: </Typography.Text>
          <Typography.Paragraph style={{ marginTop: 20 }}>
            <ul>
              <li>Độ dài mật khẩu từ 8 đến 32 ký tự</li>
              <li> Chứa 1 ký tự đặc biệt</li>
              <li> Chứa 1 ký tự viết hoa</li>
              <li> Chứa 1 ký tự viết thường</li>
              <li> Chứa 1 ký tự số</li>
            </ul>
          </Typography.Paragraph>
          <Form.Item style={{ textAlign: "end" }}>
            <Space direction="horizontal">
              <Button type="primary" htmlType="submit" loading={isloadingChangePassword}>
                Cập nhật mật khẩu
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
}

export const ChangePassword = WithErrorBoundaryCustom(_ChangePassword);
