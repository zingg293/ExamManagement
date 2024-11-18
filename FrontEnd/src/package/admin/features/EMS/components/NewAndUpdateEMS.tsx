import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, InputNumber, Row, Select, Space, Spin, Tag } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { EMSDTO } from "@models/EMSDTO";
import { useGetEMSByIdQuery, useInsertEMSMutation, useUpdateEMSMutation } from "@API/services/EMS.service";
// import { useGetListTestScheduleQuery } from "@API/services/TestSchedule.service";
import { useGetListExaminationQuery } from "@API/services/Examination.service";
import { useGetListExaminationTypeQuery } from "@API/services/ExaminationType.service";
import { useGetListExamSubjectQuery } from "@API/services/ExamSubject.service";
import { Option } from "antd/lib/mentions";
import {ExaminationTypeDTO} from "@models/ExaminationTypeDTO";
// import {useGetListLecturersQuery} from "@API/services/Lecturers.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateEMS(props: IProps) {
  const { setVisible, id } = props;
  const { data: EMS, isLoading: LoadingEMS } = useGetEMSByIdQuery({ idEMS: id! }, { skip: !id });
  const [newEMS, { isLoading: LoadingInsertCategoryNationaly }] = useInsertEMSMutation();
  const [updateEMS, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateEMSMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (EMS?.payload && id) {
      formRef.setFieldsValue(EMS?.payload);
    } else {
      formRef.resetFields();
    }
  }, [EMS, formRef, id]);
  const onfinish = async (values: EMSDTO) => {
    try {
      const result = id
        ? await updateEMS({
            EMS: values
          }).unwrap()
        : await newEMS({ EMS: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  // const { data: ListTestSchedule } = useGetListTestScheduleQuery({
  //   pageSize: 0,
  //   pageNumber: 0
  // });
  const { data: ListExam, isLoading: LoadingListExam } = useGetListExaminationTypeQuery({
    pageSize: 0,
    pageNumber: 0
  });
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
      <Spin spinning={LoadingEMS}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{span: 4}} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden/>
              <Form.Item name={"status"} hidden/>
              <Form.Item label="Tên" name={"emsName"}>
                <Input/>
              </Form.Item>
              <Form.Item label="SL sinh viên" name={"numberOfStudents"}>
                <InputNumber/>
              </Form.Item>
              <Form.Item label="SL giảng viên" name={"numberOfLecturers"}>
                <InputNumber/>
              </Form.Item>
              <Form.Item label="Loại kì thi" name="idExamType">
                <Select
                  loading={LoadingListExam}
                  options={
                  ListExam?.listPayload?.map((temp: ExaminationTypeDTO) => ({
                    label: temp.examTypeName,
                    value: temp.id
                  }))
                }/>

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
              <Form.Item label="Môn thi" name="idExamSubject">
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
                    {label: <Tag color="red-inverse">Ẩn</Tag>, value: false},
                    {label: <Tag color="green-inverse">Hiển thị</Tag>, value: true}
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
                    icon={<RetweetOutlined/>}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryNationaly || LoadingUpdateCategoryNationaly}
                    icon={<CheckCircleOutlined/>}
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

export const NewAndUpdateEMS = WithErrorBoundaryCustom(_NewAndUpdateEMS);
