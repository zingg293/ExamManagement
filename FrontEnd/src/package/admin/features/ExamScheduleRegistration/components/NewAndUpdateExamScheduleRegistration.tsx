import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Select, Space, Spin, Tag } from "antd";
import React, { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { ExaminationTypeDTO } from "@models/ExaminationTypeDTO";
import {
  useGetExaminationTypeByIdQuery,
  useInsertExaminationTypeMutation,
  useUpdateExaminationTypeMutation
} from "@API/services/ExaminationType.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateExaminationType(props: IProps) {
  const { setVisible, id } = props;
  const { data: ExaminationType, isLoading: LoadingExaminationType } = useGetExaminationTypeByIdQuery(
    { idExaminationType: id! },
    { skip: !id }
  );
  const [newExaminationType, { isLoading: LoadingInsertCategoryNationaly }] = useInsertExaminationTypeMutation();
  const [updateExaminationType, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateExaminationTypeMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (ExaminationType?.payload && id) {
      formRef.setFieldsValue(ExaminationType?.payload);
    } else {
      formRef.resetFields();
    }
  }, [ExaminationType, formRef, id]);
  const onfinish = async (values: ExaminationTypeDTO) => {
    try {
      const result = id
        ? await updateExaminationType({
            ExaminationType: values
          }).unwrap()
        : await newExaminationType({ ExaminationType: values }).unwrap();
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
      <Spin spinning={LoadingExaminationType}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"examTypeName"}>
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

export const NewAndUpdateExaminationType = WithErrorBoundaryCustom(_NewAndUpdateExaminationType);
