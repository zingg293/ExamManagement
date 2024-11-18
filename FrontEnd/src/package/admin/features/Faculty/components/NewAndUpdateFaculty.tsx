import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Select, Space, Spin, Tag } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { FacultyDTO } from "@models/FacultyDTO";
import {
  useGetFacultyByIdQuery,
  useInsertFacultyMutation,
  useUpdateFacultyMutation
} from "@API/services/Faculty.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateFaculty(props: IProps) {
  const { setVisible, id } = props;
  const { data: Faculty, isLoading: LoadingFaculty } = useGetFacultyByIdQuery({ idFaculty: id! }, { skip: !id });
  const [newFaculty, { isLoading: LoadingInsertCategoryNationaly }] = useInsertFacultyMutation();
  const [updateFaculty, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateFacultyMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (Faculty?.payload && id) {
      formRef.setFieldsValue(Faculty?.payload);
    } else {
      formRef.resetFields();
    }
  }, [Faculty, formRef, id]);
  const onfinish = async (values: FacultyDTO) => {
    try {
      const result = id
        ? await updateFaculty({
            Faculty: values
          }).unwrap()
        : await newFaculty({ Faculty: values }).unwrap();
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
      <Spin spinning={LoadingFaculty}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"facultyName"}>
                <Input />
              </Form.Item>
              <Form.Item label="Trạng thái" name={"isHide"}>
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

export const NewAndUpdateFaculty = WithErrorBoundaryCustom(_NewAndUpdateFaculty);
