import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Select, Space, Spin, Tag } from "antd";
import React, { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { EducationProgramDTO } from "@models/EducationProgramDTO";
import {
  useGetEducationProgramByIdQuery,
  useInsertEducationProgramMutation,
  useUpdateEducationProgramMutation
} from "@API/services/EducationProgram.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
  
}

function _NewAndUpdateEducationProgram(props: IProps) {
  const { setVisible, id } = props;
  const { data: EducationProgram, isLoading: LoadingEducationProgram } = useGetEducationProgramByIdQuery(
    { idEducationProgram: id! },
    { skip: !id }
  );
  const [newEducationProgram, { isLoading: LoadingInsertEducationProgram }] = useInsertEducationProgramMutation();
  const [updateEducationProgram, { isLoading: LoadingUpdateEducationProgram }] = useUpdateEducationProgramMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (EducationProgram?.payload && id) {
      formRef.setFieldsValue(EducationProgram?.payload);
    } else {
      formRef.resetFields();
    }
  }, [EducationProgram, formRef, id]);
  const onfinish = async (values: EducationProgramDTO) => {
    try {
      const result = id
        ? await updateEducationProgram({
            EducationProgram: values
          }).unwrap()
        : await newEducationProgram({ EducationProgram: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateEducationProgram">
      <Spin spinning={LoadingEducationProgram}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 5 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              
              <Form.Item label="Tên CT đào tạo" name={"EducationProgramName"}
               rules={[
                 {
                   required: true,
                   message: "Vui lòng nhập tên chương trình đào tạo"
                 }
               ]}
             >
            
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
                    loading={LoadingInsertEducationProgram || LoadingUpdateEducationProgram}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertEducationProgram || LoadingUpdateEducationProgram}
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

export const NewAndUpdateEducationProgram = WithErrorBoundaryCustom(_NewAndUpdateEducationProgram);
