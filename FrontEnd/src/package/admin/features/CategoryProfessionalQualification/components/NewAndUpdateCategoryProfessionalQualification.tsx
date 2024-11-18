import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CategoryProfessionalQualificationDTO } from "@models/CategoryProfessionalQualificationDTO";
import {
  useGetCategoryProfessionalQualificationByIdQuery,
  useInsertCategoryProfessionalQualificationMutation,
  useUpdateCategoryProfessionalQualificationMutation
} from "@API/services/CategoryProfessionalQualification.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryProfessionalQualification(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryProfessionalQualification, isLoading: LoadingCategoryProfessionalQualification } = useGetCategoryProfessionalQualificationByIdQuery(
    {idCategoryProfessionalQualification: id!}, {skip: !id});
  const [newCategoryProfessionalQualification, { isLoading: LoadingInsertCategoryProfessionalQualification }] = useInsertCategoryProfessionalQualificationMutation();
  const [updateCategoryProfessionalQualification, { isLoading: LoadingUpdateCategoryProfessionalQualification }] = useUpdateCategoryProfessionalQualificationMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (CategoryProfessionalQualification?.payload && id) {
      formRef.setFieldsValue(CategoryProfessionalQualification?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryProfessionalQualification, formRef, id]);
  const onfinish = async (values: CategoryProfessionalQualificationDTO) => {
    try {
      const result = id
        ? await updateCategoryProfessionalQualification({
          CategoryProfessionalQualification: values
        }).unwrap()
        : await newCategoryProfessionalQualification({ CategoryProfessionalQualification: values }).unwrap();
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
      <Spin spinning={LoadingCategoryProfessionalQualification}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"nameProfessionalQualification"}>
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
                    loading={LoadingInsertCategoryProfessionalQualification || LoadingUpdateCategoryProfessionalQualification}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryProfessionalQualification || LoadingUpdateCategoryProfessionalQualification}
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

export const NewAndUpdateCategoryProfessionalQualification = WithErrorBoundaryCustom(_NewAndUpdateCategoryProfessionalQualification);
