import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CategoryTypeSalaryScaleDTO } from "@models/CategoryTypeSalaryScaleDTO";
import {
  useGetCategoryTypeSalaryScaleByIdQuery,
  useInsertCategoryTypeSalaryScaleMutation,
  useUpdateCategoryTypeSalaryScaleMutation
} from "@API/services/CategoryTypeSalaryScale.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryTypeSalaryScale(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryTypeSalaryScale, isLoading: LoadingCategoryTypeSalaryScale } = useGetCategoryTypeSalaryScaleByIdQuery(
    {idCategoryTypeSalaryScale: id!}, {skip: !id});
  const [newCategoryTypeSalaryScale, { isLoading: LoadingInsertCategoryNationaly }] = useInsertCategoryTypeSalaryScaleMutation();
  const [updateCategoryTypeSalaryScale, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateCategoryTypeSalaryScaleMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (CategoryTypeSalaryScale?.payload && id) {
      formRef.setFieldsValue(CategoryTypeSalaryScale?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryTypeSalaryScale, formRef, id]);
  const onfinish = async (values: CategoryTypeSalaryScaleDTO) => {
    try {
      const result = id
        ? await updateCategoryTypeSalaryScale({
          CategoryTypeSalaryScale: values
        }).unwrap()
        : await newCategoryTypeSalaryScale({ CategoryTypeSalaryScale: values }).unwrap();
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
      <Spin spinning={LoadingCategoryTypeSalaryScale}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"nameTypeSalaryScale"}>
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
                    loading={LoadingInsertCategoryNationaly || LoadingUpdateCategoryNationaly}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryNationaly || LoadingUpdateCategoryNationaly}
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

export const NewAndUpdateCategoryTypeSalaryScale = WithErrorBoundaryCustom(_NewAndUpdateCategoryTypeSalaryScale);
