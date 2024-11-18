import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, InputNumber, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { AllowanceDTO } from "@models/allowanceDTO";
import {
  useGetAllowanceByIdQuery,
  useInsertAllowanceMutation,
  useUpdateAllowanceMutation
} from "@API/services/Allowance.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateAllowance(props: IProps) {
  const { setVisible, id } = props;
  const { data: Allowance, isLoading: LoadingAllowance } = useGetAllowanceByIdQuery(
    { idAllowance: id! },
    {
      skip: !id
    }
  );
  const [newAllowance, { isLoading: LoadingInsertAllowance }] = useInsertAllowanceMutation();
  const [updateAllowance, { isLoading: LoadingUpdateAllowance }] = useUpdateAllowanceMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (Allowance?.payload && id) {
      formRef.setFieldsValue(Allowance?.payload);
    } else {
      formRef.resetFields();
    }
  }, [Allowance, formRef, id]);
  const onfinish = async (values: AllowanceDTO) => {
    try {
      const result = id
        ? await updateAllowance({
            allowance: values
          }).unwrap()
        : await newAllowance({ allowance: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateAllowance">
      <Spin spinning={LoadingAllowance}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Tên" name={"name"}>
                <Input />
              </Form.Item>
              <Form.Item label="Số tiền" name={"amount"}>
                <InputNumber
                  formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")}
                  parser={(value) => value!.replace(/\$\s?|(,*)/g, "")}
                />
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
                    loading={LoadingInsertAllowance || LoadingUpdateAllowance}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertAllowance || LoadingUpdateAllowance}
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

export const NewAndUpdateAllowance = WithErrorBoundaryCustom(_NewAndUpdateAllowance);
