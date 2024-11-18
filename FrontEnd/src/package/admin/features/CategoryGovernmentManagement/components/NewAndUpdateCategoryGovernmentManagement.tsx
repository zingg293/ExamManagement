import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import  CategoryGovernmentManagementDTO  from "@models/CategoryGovernmentManagementDTO";
import {
  useGetCategoryGovernmentManagementByIdQuery,
  useInsertCategoryGovernmentManagementMutation,
  useUpdateCategoryGovernmentManagementMutation
} from "@API/services/CategoryGovernmentManagement.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryGovernmentManagement(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryGovernmentManagement, isLoading: LoadingCategoryGovernmentManagement } = useGetCategoryGovernmentManagementByIdQuery(
    {idCategoryGovernmentManagement: id!}, {skip: !id});
  const [newCategoryGovernmentManagement, { isLoading: LoadingInsertCategoryGovernmentManagement }] = useInsertCategoryGovernmentManagementMutation();
  const [updateCategoryGovernmentManagement, { isLoading: LoadingUpdateCategoryGovernmentManagement }] = useUpdateCategoryGovernmentManagementMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (CategoryGovernmentManagement?.payload && id) {
      formRef.setFieldsValue(CategoryGovernmentManagement?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryGovernmentManagement, formRef, id]);
  const onfinish = async (values: CategoryGovernmentManagementDTO) => {
    try {
      const result = id
        ? await updateCategoryGovernmentManagement({
          CategoryGovernmentManagement: values
        }).unwrap()
        : await newCategoryGovernmentManagement({ CategoryGovernmentManagement: values }).unwrap();
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
      <Spin spinning={LoadingCategoryGovernmentManagement}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"nameGovernmentManagement"}>
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
                    loading={LoadingInsertCategoryGovernmentManagement|| LoadingUpdateCategoryGovernmentManagement}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryGovernmentManagement || LoadingUpdateCategoryGovernmentManagement}
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

export const NewAndUpdateCategoryGovernmentManagement = WithErrorBoundaryCustom(_NewAndUpdateCategoryGovernmentManagement);
