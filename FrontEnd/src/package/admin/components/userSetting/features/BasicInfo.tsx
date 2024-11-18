import { Button, Card, Col, Divider, Form, Input, Row, Space, Spin, Typography } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { useGetUserQuery, useUpdateUserMutation } from "@API/services/UserApis.service";
import { UserDTO } from "~/models/userDto";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";

function _BasicInfo() {
  const { data, isLoading } = useGetUserQuery({ fetch: false });
  const [updateUser, { isLoading: isUpdating }] = useUpdateUserMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    if (data?.payload?.data) {
      formRef.setFieldsValue(data?.payload?.data);
    }
  }, [data?.payload?.data, formRef]);
  const onfinish = async (values: UserDTO) => {
    try {
      if (!data?.payload?.data) return;
      const newData = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        const processedValue = value || "";
        newData.append(key, processedValue);
      });
      return await updateUser({ user: newData as Partial<UserDTO> });
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="BasicInfo">
      <Spin spinning={isLoading}>
        <Card bordered={false} className="criclebox h-full">
          <Typography.Title level={4}>Thông tin cơ bản</Typography.Title>
          <Divider />
          <Form onFinish={onfinish} layout="vertical" form={formRef}>
            <Form.Item name="id" hidden />
            <Form.Item name="isActive" hidden />
            <Form.Item name="unitId" hidden />
            <Form.Item name="userTypeId" hidden />
            <Form.Item name="password" hidden />
            <Row gutter={[24, 0]}>
              <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                <Form.Item
                  label="Họ và tên"
                  name="fullname"
                  rules={[
                    {
                      required: true,
                      message: "Vui lòng nhập họ và tên"
                    }
                  ]}
                >
                  <Input allowClear />
                </Form.Item>
                <Form.Item
                  label="Email"
                  name="email"
                  rules={[
                    {
                      required: true,
                      message: "Vui lòng nhập email"
                    }
                  ]}
                >
                  <Input allowClear />
                </Form.Item>
                <Form.Item label="Ghi chú" name="description">
                  <Input.TextArea rows={1} showCount allowClear />
                </Form.Item>
              </Col>
              <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                <Form.Item label="Số điện thoại" name="phone">
                  <Input allowClear />
                </Form.Item>
                <Form.Item label="Địa chỉ" name="address">
                  <Input.TextArea rows={1} allowClear />
                </Form.Item>
              </Col>
            </Row>
            <Form.Item style={{ textAlign: "end" }}>
              <Space direction="horizontal">
                <Button type="primary" htmlType="submit" loading={isUpdating}>
                  Lưu
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </Card>
      </Spin>
    </div>
  );
}

export const BasicInfo = WithErrorBoundaryCustom(_BasicInfo);
