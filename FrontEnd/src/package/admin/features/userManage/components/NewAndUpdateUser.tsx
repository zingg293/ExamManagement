import { Button, Col, Form, Input, Popconfirm, Row, Select, Space, Spin, Tooltip } from "antd";
import { UserDTO } from "~/models/userDto";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useCreateUserMutation,
  useForgotPasswordMutation,
  useGetUserByIdQuery,
  useUpdateUserMutation
} from "@API/services/UserApis.service";
import { HandleError } from "@admin/components";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";
import { useGetListUserTypeQuery } from "@API/services/UserType.service";
import { useEffect } from "react";
import { CheckCircleOutlined } from "@ant-design/icons";

interface NewAndUpdateUserProps {
  setVisible: (value: boolean) => void;
  idUser?: string;
}

function _NewAndUpdateUser(props: NewAndUpdateUserProps) {
  const { setVisible, idUser } = props;
  const [addUser, { isLoading: isLoadingInsertUser }] = useCreateUserMutation();
  const { data: ListUnit, isLoading: isLoadingListUnit } = useGetListUnitAvailableQuery({ pageSize: 0, pageNumber: 0 });
  const { data: ListUserType, isLoading: isLoadingListUserType } = useGetListUserTypeQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data, isLoading } = useGetUserByIdQuery({ id: idUser }, { skip: !idUser });
  const [updateUser, { isLoading: isLoadingUpdateUser }] = useUpdateUserMutation();
  const [forGotPassword, { isLoading: isLoadingForgotPassword }] = useForgotPasswordMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    formRef.resetFields();
    if (data?.payload && idUser) {
      formRef.setFieldsValue(data?.payload);
    } else {
      formRef.resetFields();
    }
  }, [data?.payload, formRef, idUser]);
  const onfinish = async (values: UserDTO) => {
    try {
      const newData = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        const processedValue = value || "";
        newData.append(key, processedValue);
      });
      newData.append("isActive", "true");
      const result = idUser
        ? await updateUser({ user: newData as Partial<UserDTO> }).unwrap()
        : await addUser({ user: newData as Partial<UserDTO> }).unwrap();
      if (result.success) {
        setVisible(false);
      }
    } catch (error: any) {
      await HandleError(error);
    }
  };
  return (
    <div className="NewAndUpdateUser">
      <Spin spinning={isLoading} size="large">
        <Form onFinish={onfinish} layout={"horizontal"} labelCol={{ span: 4 }} form={formRef}>
          <Form.Item name={"id"} />
          <Row>
            <Col xs={24} sm={24} md={24} lg={24} xl={24}>
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
                <Input />
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
                <Input allowClear readOnly={!!idUser} />
              </Form.Item>
              <Form.Item
                label="Mật khẩu"
                name="password"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập mật khẩu"
                  }
                ]}
                hidden={!!idUser}
              >
                <Input.Password allowClear />
              </Form.Item>
              <Form.Item label="Số điện thoại" name="phone">
                <Input allowClear />
              </Form.Item>
              <Form.Item label="Địa chỉ" name="address">
                <Input.TextArea rows={1} allowClear />
              </Form.Item>
              <Form.Item label="Ghi chú" name="description">
                <Input.TextArea rows={1} allowClear />
              </Form.Item>
              <Form.Item
                label="Phòng ban"
                name="unitId"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn phòng ban"
                  }
                ]}
              >
                <Select
                  loading={isLoadingListUnit}
                  options={ListUnit?.listPayload?.map((unit) => ({
                    label: `${unit.unitName} - ${unit.unitCode}`,
                    value: unit.id
                  }))}
                  showSearch
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item
                label="Loại người dùng"
                name="userTypeId"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn loại người dùng"
                  }
                ]}
              >
                <Select
                  loading={isLoadingListUserType}
                  options={ListUserType?.listPayload?.map((userType) => ({
                    label: `${userType.typeName} - ${userType.typeCode}`,
                    value: userType.id
                  }))}
                  showSearch
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item style={{ textAlign: "end" }}>
                <Space direction="horizontal" wrap>
                  <Tooltip placement="leftTop" title={"Mật khẩu làm mới mặc định là 12345678"}>
                    <Popconfirm
                      title="Bạn có chắc chắn không ?"
                      okText="Có"
                      cancelText="Không"
                      onConfirm={async () => {
                        try {
                          if (!data?.payload?.email) return;
                          await forGotPassword({
                            email: data?.payload?.email,
                            newPassword: "12345678"
                          });
                        } catch (e: any) {
                          await HandleError(e);
                        }
                      }}
                    >
                      <Button loading={isLoadingForgotPassword} type="default" hidden={!idUser}>
                        Làm mới mật khẩu
                      </Button>
                    </Popconfirm>
                  </Tooltip>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={isLoadingInsertUser || isLoadingUpdateUser}
                    icon={<CheckCircleOutlined />}
                  >
                    Lưu
                  </Button>
                </Space>
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Spin>
    </div>
  );
}

export const NewAndUpdateUser = WithErrorBoundaryCustom(_NewAndUpdateUser);
