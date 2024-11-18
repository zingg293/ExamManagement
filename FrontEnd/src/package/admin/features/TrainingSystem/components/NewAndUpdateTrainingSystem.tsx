import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Select, Space, Spin, Tag } from "antd";
import React, { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { TrainingSystemDTO } from "@models/TrainingSystemDTO";
import {
  useGetTrainingSystemByIdQuery,
  useInsertTrainingSystemMutation,
  useUpdateTrainingSystemMutation
} from "@API/services/TrainingSystem.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
  
}

function _NewAndUpdateTrainingSystem(props: IProps) {
  const { setVisible, id } = props;
  const { data: TrainingSystem, isLoading: LoadingTrainingSystem } = useGetTrainingSystemByIdQuery(
    { idTrainingSystem: id! },
    { skip: !id }
  );
  const [newTrainingSystem, { isLoading: LoadingInsertTrainingSystem }] = useInsertTrainingSystemMutation();
  const [updateTrainingSystem, { isLoading: LoadingUpdateTrainingSystem }] = useUpdateTrainingSystemMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (TrainingSystem?.payload && id) {
      formRef.setFieldsValue(TrainingSystem?.payload);
    } else {
      formRef.resetFields();
    }
  }, [TrainingSystem, formRef, id]);
  const onfinish = async (values: TrainingSystemDTO) => {
    try {
      const result = id
        ? await updateTrainingSystem({
            TrainingSystem: values
          }).unwrap()
        : await newTrainingSystem({ TrainingSystem: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateTrainingSystem">
      <Spin spinning={LoadingTrainingSystem}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 7 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên hệ đào tạo" name={"examTypeName"}>
                <Input />
              </Form.Item>
              <Form.Item label="Tên chương trình đào tạo" name={"EducationProgramName"}
                rules={[{ required: true, message: "Vui lòng nhập tên chương trình đào tạo" }]
              }>
                <Input />
              </Form.Item>
              <Form.Item label="Trạng thái" name={"isActive"}>
                <Select
                  allowClear
                  options={[
                    { label: <Tag color="red-inverse">Ẩn</Tag>, value: false },
                    { label: <Tag color="green-inverse">Hiển thị</Tag>, value: true }
                  ]}
                  placeholder={"Chọn trạng thái"}
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
                    loading={LoadingInsertTrainingSystem || LoadingUpdateTrainingSystem}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertTrainingSystem || LoadingUpdateTrainingSystem}
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

export const NewAndUpdateTrainingSystem = WithErrorBoundaryCustom(_NewAndUpdateTrainingSystem);
