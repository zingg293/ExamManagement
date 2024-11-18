import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CategorySalaryScaleDTO } from "@models/CategorySalaryScaleDTO";
import {
  useGetCategorySalaryScaleByIdQuery,
  useInsertCategorySalaryScaleMutation,
  useUpdateCategorySalaryScaleMutation
} from "@API/services/CategorySalaryScale.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategorySalaryScale(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategorySalaryScale, isLoading: LoadingCategorySalaryScale } = useGetCategorySalaryScaleByIdQuery(
    {idCategorySalaryScale: id!}, {skip: !id});
  const [newCategorySalaryScale, { isLoading: LoadingInsertCategoryNationaly }] = useInsertCategorySalaryScaleMutation();
  const [updateCategorySalaryScale, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateCategorySalaryScaleMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (CategorySalaryScale?.payload && id) {
      formRef.setFieldsValue(CategorySalaryScale?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategorySalaryScale, formRef, id]);
  const onfinish = async (values: CategorySalaryScaleDTO) => {
    try {
      const result = id
        ? await updateCategorySalaryScale({
          CategorySalaryScale: values
        }).unwrap()
        : await newCategorySalaryScale({ CategorySalaryScale: values }).unwrap();
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
      <Spin spinning={LoadingCategorySalaryScale}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"nameSalaryScale"}>
                <Input />
              </Form.Item>
              <Form.Item label="Mã" name={"code"}>
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

export const NewAndUpdateCategorySalaryScale = WithErrorBoundaryCustom(_NewAndUpdateCategorySalaryScale);
