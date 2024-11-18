import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Select, Space, Spin, Tag } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { ExamSubjectDTO } from "@models/ExamSubjectDTO";
import {
  useGetExamSubjectByIdQuery,
  useInsertExamSubjectMutation,
  useUpdateExamSubjectMutation
} from "@API/services/ExamSubject.service";
import { useGetListExaminationQuery } from "@API/services/Examination.service";
import { useGetListExaminationTypeQuery } from "@API/services/ExaminationType.service";
import { Option } from "antd/lib/mentions";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateExamSubject(props: IProps) {
  const { setVisible, id } = props;
  const { data: ExamSubject, isLoading: LoadingExamSubject } = useGetExamSubjectByIdQuery(
    { idExamSubject: id! },
    { skip: !id }
  );
  const [newExamSubject, { isLoading: LoadingInsertCategoryNationaly }] = useInsertExamSubjectMutation();
  const [updateExamSubject, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateExamSubjectMutation();
  const [formRef] = Form.useForm();
  const { data: ListExamination } = useGetListExaminationQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListExaminationType } = useGetListExaminationTypeQuery({
    pageSize: 0,
    pageNumber: 0
  });
  useEffect(() => {
    if (ExamSubject?.payload && id) {
      formRef.setFieldsValue(ExamSubject?.payload);
    } else {
      formRef.resetFields();
    }
  }, [ExamSubject, formRef, id]);
  const onfinish = async (values: ExamSubjectDTO) => {
    try {
      const result = id
        ? await updateExamSubject({
            ExamSubject: values
          }).unwrap()
        : await newExamSubject({ ExamSubject: values }).unwrap();
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
      <Spin spinning={LoadingExamSubject}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"examSubjectName"}>
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
              <Form.Item label="Kì thi" name="idExam">
                <Select>
                  {ListExamination?.listPayload?.map((temp) => (
                    <Option key={temp.id} value={temp.id}>
                      {temp.examName}
                    </Option>
                  ))}
                </Select>
              </Form.Item>
              <Form.Item label="Loại kì thi" name="idExamType">
                <Select>
                  {ListExaminationType?.listPayload?.map((temp) => (
                    <Option key={temp.id} value={temp.id}>
                      {temp.examTypeName}
                    </Option>
                  ))}
                </Select>
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

export const NewAndUpdateExamSubject = WithErrorBoundaryCustom(_NewAndUpdateExamSubject);
