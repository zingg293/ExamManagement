import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect } from "react";
import { UserTypeDTO } from "@models/userTypeDto";
import {
  useGetUserTypeByIdQuery,
  useInsertUserTypeMutation,
  useUpdateUserTypeMutation
} from "@API/services/UserType.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateUserType(props: IProps) {
  const { setVisible, id } = props;
  const { data: UserType, isLoading: LoadingUserType } = useGetUserTypeByIdQuery(
    { id: id! },
    {
      skip: !id
    }
  );
  const [newUserType, { isLoading: LoadingInsertUserType }] = useInsertUserTypeMutation();
  const [updateUserType, { isLoading: LoadingUpdateUserType }] = useUpdateUserTypeMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    formRef.resetFields();
    if (UserType?.payload && id) {
      formRef.setFieldsValue(UserType?.payload);
    } else {
      formRef.resetFields();
    }
  }, [UserType, formRef, id]);
  const onfinish = async (values: UserTypeDTO) => {
    try {
      const result = id
        ? await updateUserType({
            userType: values
          }).unwrap()
        : await newUserType({ userType: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateUserType">
      <Spin spinning={LoadingUserType}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item
                label="Tên loại"
                name={"typeName"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập tên loại"
                  }
                ]}
              >
                <Input />
              </Form.Item>
              <Form.Item>
                <Space
                  style={{
                    width: "100%",
                    justifyContent: "flex-end"
                  }}
                >
                  <Button
                    type="default"
                    htmlType="reset"
                    loading={LoadingInsertUserType || LoadingUpdateUserType}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertUserType || LoadingUpdateUserType}
                    icon={<CheckCircleOutlined />}
                    style={{
                      float: "right"
                    }}
                  >
                    Lưu
                  </Button>
                </Space>
              </Form.Item>
            </Form>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const NewAndUpdateUserType = WithErrorBoundaryCustom(_NewAndUpdateUserType);
