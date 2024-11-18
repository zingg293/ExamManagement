import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useGetCategoryCompensationBenefitsByIdQuery,
  useInsertCategoryCompensationBenefitsMutation,
  useUpdateCategoryCompensationBenefitsMutation
} from "@API/services/CategoryCompensationBenefits.service";
import { Button, Col, Form, Input, InputNumber, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CategoryCompensationBenefitsDTO } from "@models/categoryCompensationBenefitsDTO";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryCompensationBenefits(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryCompensationBenefits, isLoading: LoadingCategoryCompensationBenefits } =
    useGetCategoryCompensationBenefitsByIdQuery(
      { id: id! },
      {
        skip: !id
      }
    );
  const [newCategoryCompensationBenefits, { isLoading: LoadingInsertCategoryCompensationBenefits }] =
    useInsertCategoryCompensationBenefitsMutation();
  const [updateCategoryCompensationBenefits, { isLoading: LoadingUpdateCategoryCompensationBenefits }] =
    useUpdateCategoryCompensationBenefitsMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    formRef.resetFields();
    if (CategoryCompensationBenefits?.payload && id) {
      formRef.setFieldsValue(CategoryCompensationBenefits?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryCompensationBenefits, formRef, id]);
  const onfinish = async (values: CategoryCompensationBenefitsDTO) => {
    try {
      const result = id
        ? await updateCategoryCompensationBenefits({
            categoryCompensationBenefits: values
          }).unwrap()
        : await newCategoryCompensationBenefits({ categoryCompensationBenefits: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateCategoryCompensationBenefits">
      <Spin spinning={LoadingCategoryCompensationBenefits}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Tên" name={"name"}>
                <Input />
              </Form.Item>
              <Form.Item label="Số tiền" name={"amountMoney"}>
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
                    loading={LoadingInsertCategoryCompensationBenefits || LoadingUpdateCategoryCompensationBenefits}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryCompensationBenefits || LoadingUpdateCategoryCompensationBenefits}
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

export const NewAndUpdateCategoryCompensationBenefits = WithErrorBoundaryCustom(
  _NewAndUpdateCategoryCompensationBenefits
);
