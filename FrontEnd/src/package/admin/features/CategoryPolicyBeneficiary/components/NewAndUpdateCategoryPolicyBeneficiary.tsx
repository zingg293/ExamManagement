
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CategoryPolicyBeneficiaryDTO } from "@models/CategoryPolicyBeneficiaryDTO";
import {
  useGetCategoryPolicyBeneficiaryByIdQuery,
  useInsertCategoryPolicyBeneficiaryMutation,
  useUpdateCategoryPolicyBeneficiaryMutation
} from "@API/services/CategoryPolicyBeneficiary.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryPolicyBeneficiary(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryPolicyBeneficiary, isLoading: LoadingCategoryPolicyBeneficiary } = useGetCategoryPolicyBeneficiaryByIdQuery(
    {idCategoryPolicybeneficiary: id!}, {skip: !id});
  const [newCategoryPolicyBeneficiary, { isLoading: LoadingInsertCategoryPolicyBeneficiary }] = useInsertCategoryPolicyBeneficiaryMutation();
  const [updateCategoryPolicyBeneficiary, { isLoading: LoadingUpdateCategoryPolicyBeneficiary }] = useUpdateCategoryPolicyBeneficiaryMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (CategoryPolicyBeneficiary?.payload && id) {
      formRef.setFieldsValue(CategoryPolicyBeneficiary?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryPolicyBeneficiary, formRef, id]);
  const onfinish = async (values: CategoryPolicyBeneficiaryDTO) => {
    try {
      const result = id
        ? await updateCategoryPolicyBeneficiary({
          CategoryPolicyBeneficiary: values
        }).unwrap()
        : await newCategoryPolicyBeneficiary({ CategoryPolicyBeneficiary: values }).unwrap();
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
      <Spin spinning={LoadingCategoryPolicyBeneficiary}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"namePolicybeneficiary"}>
                <Input />
              </Form.Item>
              {/*<Form.Item label="Số tiền" name={"amount"}>*/}
              {/*  <InputNumber*/}
              {/*    formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")}*/}
              {/*    parser={(value) => value!.replace(/\$\s?|(,*)/g, "")}*/}
              {/*  />*/}
              {/*</Form.Item>*/}
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
                    loading={LoadingInsertCategoryPolicyBeneficiary || LoadingUpdateCategoryPolicyBeneficiary}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryPolicyBeneficiary || LoadingUpdateCategoryPolicyBeneficiary}
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

export const NewAndUpdateCategoryPolicyBeneficiary = WithErrorBoundaryCustom(_NewAndUpdateCategoryPolicyBeneficiary);
