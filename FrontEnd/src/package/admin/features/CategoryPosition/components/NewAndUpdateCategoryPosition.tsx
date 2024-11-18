import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import {
  useGetCategoryPositionByIdQuery,
  useInsertCategoryPositionMutation,
  useUpdateCategoryPositionMutation
} from "@API/services/CategoryPositionApis.service";
import { useEffect } from "react";
import TextArea from "antd/es/input/TextArea";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryPosition(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryPosition, isLoading: LoadingCategoryPosition } = useGetCategoryPositionByIdQuery(
    { idCategoryPosition: id! },
    { skip: !id }
  );
  const [newCategoryPosition, { isLoading: LoadingInsertCategoryPosition }] = useInsertCategoryPositionMutation();
  const [updateCategoryPosition, { isLoading: LoadingUpdateCategoryPosition }] = useUpdateCategoryPositionMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    formRef.resetFields();
    if (CategoryPosition?.payload && id) {
      formRef.setFieldsValue(CategoryPosition?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryPosition?.payload, formRef, id]);
  const onfinish = async (values: any) => {
    try {
      const result = id
        ? await updateCategoryPosition({
            categoryPosition: values
          }).unwrap()
        : await newCategoryPosition({
            categoryPosition: values
          }).unwrap();
      if (result.success) {
        setVisible(false);
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateCategoryPosition">
      <Spin spinning={LoadingCategoryPosition}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Tên" name={"positionName"}>
                <Input />
              </Form.Item>
              <Form.Item label="Mô tả" name={"description"}>
                <TextArea />
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
                    loading={LoadingInsertCategoryPosition || LoadingUpdateCategoryPosition}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryPosition || LoadingUpdateCategoryPosition}
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

export const NewAndUpdateCategoryPosition = WithErrorBoundaryCustom(_NewAndUpdateCategoryPosition);
