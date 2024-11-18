import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, DatePicker, Form, Input, Row, Select, Space, Spin, Tag } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { TestScheduleDTO } from "@models/TestScheduleDTO";
import {
  useGetTestScheduleByIdQuery,
  useInsertTestScheduleMutation,
  useUpdateTestScheduleMutation
} from "@API/services/TestSchedule.service";
import { useGetListExaminationQuery } from "@API/services/Examination.service";
import { Option } from "antd/lib/mentions";
import { useGetListExamSubjectQuery } from "@API/services/ExamSubject.service";
import dayjs from "dayjs";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateTestSchedule(props: IProps) {
  const { setVisible, id } = props;
  const { data: TestSchedule, isLoading: LoadingTestSchedule } = useGetTestScheduleByIdQuery(
    { idTestSchedule: id! },
    { skip: !id }
  );
  const [newTestSchedule, { isLoading: LoadingInsertCategoryNationaly }] = useInsertTestScheduleMutation();
  const [updateTestSchedule, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateTestScheduleMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (TestSchedule?.payload && id) {
      formRef.setFieldsValue(TestSchedule?.payload);
    } else {
      formRef.resetFields();
    }
    formRef.resetFields();
    if (TestSchedule?.payload && id) {
      const { fromDate, toDate, ...restPayload } = TestSchedule.payload;
      formRef.setFieldsValue({
        ...restPayload,
        fromDate: fromDate ? dayjs(fromDate) : null,
        toDate: toDate ? dayjs(toDate) : null
      });
    }

  }, [TestSchedule, formRef, id]);
  const onfinish = async (values: TestScheduleDTO) => {
    try {
      const result = id
        ? await updateTestSchedule({
            TestSchedule: values
          }).unwrap()
        : await newTestSchedule({ TestSchedule: values }).unwrap();
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
      <Spin spinning={LoadingTestSchedule}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"testScheduleName"}>
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
              <Form.Item label="Môn thi" name="idExamSubject">
                <Select>
                  {ListExamSubject?.listPayload?.map((temp) => (
                    <Option key={temp.id} value={temp.id}>
                      {temp.examSubjectName}
                    </Option>
                  ))}
                </Select>
              </Form.Item>
              <Form.Item
                name="fromDate"
                label="Từ Ngày"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn ngày"
                  }
                ]}
              >
                <DatePicker format={"DD-MM-YYYY hh:mm"} placeholder="00/00/0000" />
              </Form.Item>
              <Form.Item
                name="toDate"
                label="Đến Ngày"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn ngày"
                  }
                ]}
              >
                <DatePicker format={"DD-MM-YYYY hh:mm"} placeholder="00/00/0000" />
              </Form.Item>
              {/*<Form.Item*/}
              {/*  name="toDate"*/}
              {/*  label="Đến Ngày"*/}
              {/*  rules={[*/}
              {/*    {*/}
              {/*      required: true,*/}
              {/*      message: "Vui lòng chọn ngày"*/}
              {/*    }*/}
              {/*  ]}*/}
              {/*>*/}
              {/*  <DatePicker format={"DD-MM-YYYY hh:mm"} placeholder="00/00/0000" />*/}
              {/*</Form.Item>*/}
              <Form.Item label="Thi cuối kì" name={"organizeFinalExams"}>
                <Select
                  allowClear
                  options={[
                    { label: <Tag color="red-inverse">Không</Tag>, value: false },
                    { label: <Tag color="green-inverse">Có</Tag>, value: true }
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

export const NewAndUpdateTestSchedule = WithErrorBoundaryCustom(_NewAndUpdateTestSchedule);
