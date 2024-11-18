import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, DatePicker, Form, Input, Row, Select, Space, Spin, Tag, TimePicker } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { ExamShiftDTO } from "@models/ExamShiftDTO";
import {
  useGetExamShiftByIdQuery,
  useInsertExamShiftMutation,
  useUpdateExamShiftMutation
} from "@API/services/ExamShift.service";
import { useGetListExaminationQuery } from "@API/services/Examination.service";
import { Option } from "antd/lib/mentions";
import { useGetListExamSubjectQuery } from "@API/services/ExamSubject.service";
import dayjs from "dayjs";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateExamShift(props: IProps) {
  const { setVisible, id } = props;
  const { data: ExamShift, isLoading: LoadingExamShift } = useGetExamShiftByIdQuery(
    { idExamShift: id! },
    { skip: !id }
  );
  const [newExamShift, { isLoading: LoadingInsertExamShift }] = useInsertExamShiftMutation();
  const [updateExamShift, { isLoading: LoadingUpdateExamShift }] = useUpdateExamShiftMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (ExamShift?.payload && id) {
      formRef.setFieldsValue(ExamShift?.payload);
    } else {
      formRef.resetFields();
    }
    formRef.resetFields();
    if (ExamShift?.payload && id) {
      const { timeStart, timeEnd, ...restPayload } = ExamShift.payload;
      formRef.setFieldsValue({
        ...restPayload,
        timeStart: timeStart ? dayjs(timeStart) : null,
        timeEnd: timeEnd ? dayjs(timeEnd) : null
      });
    }

  }, [ExamShift, formRef, id]);
  const onfinish = async (values: ExamShiftDTO) => {
    try {
      const result = id
        ? await updateExamShift({
            ExamShift: values
          }).unwrap()
        : await newExamShift({ ExamShift: values }).unwrap();
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
      <Spin spinning={LoadingExamShift}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên ca thi" name={"ExamShiftName"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập tên ca thi"
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
           
              <Form.Item
                name="timeStart"
                label="Từ giờ"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn giờ bắt đầu"
                  }
                ]}
              >
              <TimePicker format={"HH:mm:ss"} placeholder="00:00:00" />
              </Form.Item>
              <Form.Item
                name="timeEnd"
                label="Đến giờ"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn giờ kết thúc"
                  }
                ]}
              >
                <TimePicker format={"HH:mm:ss"} placeholder="00:00:00" />
              </Form.Item>
              {/*<Form.Item*/}
              {/*  name="timeEnd"*/}
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
              {/* <Form.Item label="Thi cuối kì" name={"organizeFinalExams"}>
                <Select
                  allowClear
                  options={[
                    { label: <Tag color="red-inverse">Không</Tag>, value: false },
                    { label: <Tag color="green-inverse">Có</Tag>, value: true }
                  ]}
                  placeholder={"Chọn trạng thái"}
                />
              </Form.Item> */}
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
                    loading={LoadingInsertExamShift || LoadingUpdateExamShift}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertExamShift || LoadingUpdateExamShift}
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

export const NewAndUpdateExamShift = WithErrorBoundaryCustom(_NewAndUpdateExamShift);
