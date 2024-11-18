import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, DatePicker, Form, Input, Row, Select, Space, Spin, Tag } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { StudyGroupDTO } from "@models/StudyGroupDTO";
import {
  useGetStudyGroupByIdQuery,
  useInsertStudyGroupMutation,
  useUpdateStudyGroupMutation
} from "@API/services/StudyGroup.service";
import { useGetListExaminationQuery } from "@API/services/Examination.service";
import { Option } from "antd/lib/mentions";
import { useGetListExamSubjectQuery } from "@API/services/ExamSubject.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateStudyGroup(props: IProps) {
  const { setVisible, id } = props;
  const { data: StudyGroup, isLoading: LoadingStudyGroup } = useGetStudyGroupByIdQuery(
    { idStudyGroup: id! },
    { skip: !id }
  );
  const [newStudyGroup, { isLoading: LoadingInsertStudyGroup }] = useInsertStudyGroupMutation();
  const [updateStudyGroup, { isLoading: LoadingUpdateStudyGroup }] = useUpdateStudyGroupMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (StudyGroup?.payload && id) {
      formRef.setFieldsValue(StudyGroup?.payload);
    } else {
      formRef.resetFields();
    }
    formRef.resetFields();
    

  }, [StudyGroup, formRef, id]);
  const onfinish = async (values: StudyGroupDTO) => {
    try {
      const result = id
        ? await updateStudyGroup({
            StudyGroup: values
          }).unwrap()
        : await newStudyGroup({ StudyGroup: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  const { data: ListExamination } = useGetListExaminationQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListExamSubject } = useGetListExamSubjectQuery({
    pageSize: 0,
    pageNumber: 0
  });
  return (
    <div className="NewAndUpdateAllowance">
      <Spin spinning={LoadingStudyGroup}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} labelCol={{ span: 8 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên nhóm học phần" name={"StudyGroupName"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập tên nhóm học phần"
                  }
                ]}
              >
                <Input />
              </Form.Item>
              <Form.Item label="Tên học phần" name="idExamSubject"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn học phần"
                  }
                ]}
              >
                <Select>
                  {ListExamSubject?.listPayload?.map((temp) => (
                    <Option key={temp.id} value={temp.id}>
                      {temp.examSubjectName}
                    </Option>
                  ))}
                </Select>
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
                    loading={LoadingInsertStudyGroup || LoadingUpdateStudyGroup}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertStudyGroup || LoadingUpdateStudyGroup}
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

export const NewAndUpdateStudyGroup = WithErrorBoundaryCustom(_NewAndUpdateStudyGroup);
