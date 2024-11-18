import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useGetEmployeeTypeByIdQuery,
  useInsertEmployeeTypeMutation,
  useUpdateEmployeeTypeMutation
} from "@API/services/EmployeeType.service";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { HandleError } from "@admin/components";
import { EmployeeTypeDTO } from "@models/employeeTypeDTO";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect } from "react";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateEmployeeType(props: IProps) {
  const { setVisible, id } = props;
  const { data: EmployeeType, isLoading: LoadingEmployeeType } = useGetEmployeeTypeByIdQuery(
    { id: id! },
    {
      skip: !id
    }
  );
  const [newEmployeeType, { isLoading: LoadingInsertEmployeeType }] = useInsertEmployeeTypeMutation();
  const [updateEmployeeType, { isLoading: LoadingUpdateEmployeeType }] = useUpdateEmployeeTypeMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (EmployeeType?.payload && id) {
      formRef.setFieldsValue(EmployeeType?.payload);
    } else {
      formRef.resetFields();
    }
  }, [EmployeeType, formRef, id]);
  const onfinish = async (values: EmployeeTypeDTO) => {
    try {
      const result = id
        ? await updateEmployeeType({
            employeeType: values
          }).unwrap()
        : await newEmployeeType({ employeeType: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateEmployeeType">
      <Spin spinning={LoadingEmployeeType}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item
                label="Tên"
                name={"typeName"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập tên"
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
                    loading={LoadingInsertEmployeeType || LoadingUpdateEmployeeType}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertEmployeeType || LoadingUpdateEmployeeType}
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

export const NewAndUpdateEmployeeType = WithErrorBoundaryCustom(_NewAndUpdateEmployeeType);
