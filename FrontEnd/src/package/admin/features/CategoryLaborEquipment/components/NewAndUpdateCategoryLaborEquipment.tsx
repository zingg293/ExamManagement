import {
  useGetCategoryLaborEquipmentByIdQuery,
  useInsertCategoryLaborEquipmentMutation,
  useUpdateCategoryLaborEquipmentMutation
} from "@API/services/CategoryLaborEquipmentApis.service";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { CategoryLaborEquipmentDTO } from "@models/categoryLaborEquipmentDTO";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import TextArea from "antd/es/input/TextArea";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryLaborEquipment(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryLaborEquipment, isLoading: LoadingCategoryLaborEquipment } =
    useGetCategoryLaborEquipmentByIdQuery(
      { idCategoryLaborEquipment: id! },
      {
        skip: !id
      }
    );
  const [newCategoryLaborEquipment, { isLoading: LoadingInsertCategoryLaborEquipment }] =
    useInsertCategoryLaborEquipmentMutation();
  const [updateCategoryLaborEquipment, { isLoading: LoadingUpdateCategoryLaborEquipment }] =
    useUpdateCategoryLaborEquipmentMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (CategoryLaborEquipment?.payload && id) {
      formRef.setFieldsValue(CategoryLaborEquipment?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryLaborEquipment, formRef, id]);
  const onfinish = async (values: CategoryLaborEquipmentDTO) => {
    try {
      const result = id
        ? await updateCategoryLaborEquipment({
            categoryLaborEquipment: values
          }).unwrap()
        : await newCategoryLaborEquipment({ categoryLaborEquipment: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateCategoryLaborEquipment">
      <Spin spinning={LoadingCategoryLaborEquipment}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"code"} hidden />
              <Form.Item label="Tên" name={"name"}>
                <Input />
              </Form.Item>
              <Form.Item label="Đơn vị tính" name={"unit"}>
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
                    loading={LoadingInsertCategoryLaborEquipment || LoadingUpdateCategoryLaborEquipment}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryLaborEquipment || LoadingUpdateCategoryLaborEquipment}
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

export const NewAndUpdateCategoryLaborEquipment = WithErrorBoundaryCustom(_NewAndUpdateCategoryLaborEquipment);
