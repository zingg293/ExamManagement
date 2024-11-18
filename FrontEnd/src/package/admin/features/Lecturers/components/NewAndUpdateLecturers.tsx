import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Radio, Row, Select, Space, Spin, DatePicker } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { LecturersDTO } from "@models/LecturersDTO";
import {
  useGetLecturersByIdQuery,
  useInsertLecturersMutation,
  useUpdateLecturersMutation
} from "@API/services/Lecturers.service";
import { useGetListFacultyQuery } from "@API/services/Faculty.service";
import { Option } from "antd/lib/mentions";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateLecturers(props: IProps) {
  const { setVisible, id } = props;
  const { data: Lecturers, isLoading: LoadingLecturers } = useGetLecturersByIdQuery(
    { idLecturers: id! },
    { skip: !id }
  );
  const [newLecturers, { isLoading: LoadingInsertCategoryNationaly }] = useInsertLecturersMutation();
  const [updateLecturers, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateLecturersMutation();
  const [formRef] = Form.useForm();
  const { data: ListFaculty } = useGetListFacultyQuery({
    pageSize: 0,
    pageNumber: 0
  });
  useEffect(() => {
    if (Lecturers?.payload && id) {
      formRef.setFieldsValue(Lecturers?.payload);
    } else {
      formRef.resetFields();
    }
  }, [Lecturers, formRef, id]);
  const onfinish = async (values: LecturersDTO) => {
    values.avatar = "";
    try {
      const result = id
        ? await updateLecturers({
            Lecturers: values
          }).unwrap()
        : await newLecturers({ Lecturers: values }).unwrap();
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
      <Spin spinning={LoadingLecturers}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 5 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item name={"avatar"} hidden />
              <Form.Item label="Tên giảng viên" name={"fullName"}>
                <Input />
              </Form.Item>
              <Form.Item label="Mã giảng viên" name={"IntroduceCode"}

               rules={[
                {
                  required: true,
                  message: "Vui lòng nhập mã giảng viên"
                }
              ]}>
                <Input   />
               
              </Form.Item>
              <Form.Item label="Số điện thoại" name={"phone"}>
                <Input />
              </Form.Item>
              <Form.Item label="Khoa - Viện" name="idFaculty">
                <Select>
                  {ListFaculty?.listPayload?.map((faculty) => (
                    <Option key={faculty.id} value={faculty.id}>
                      {faculty.facultyName}
                    </Option>
                  ))}
                </Select>
              </Form.Item>
              <Form.Item
                name="gender"
                label="Giới tính"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn giới tính"
                  }
                ]}
              >
                <Radio.Group optionType="button">
                  <Radio value={true}>Nam</Radio>
                  <Radio value={false}>Nữ</Radio>
                </Radio.Group>
              </Form.Item>
              <Form.Item label="Ngày sinh" name={"birthday"}>
               {/* <Input type={"date"} /> */}
                 <DatePicker format={"DD-MM-YYYY "} placeholder="00/00/0000" />
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

export const NewAndUpdateLecturers = WithErrorBoundaryCustom(_NewAndUpdateLecturers);
