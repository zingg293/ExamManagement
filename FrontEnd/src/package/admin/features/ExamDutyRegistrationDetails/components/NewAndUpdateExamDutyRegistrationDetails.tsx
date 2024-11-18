import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, DatePicker, Form, Input, Row,Select,
  Space, Spin, Tag
} from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { ExamShiftDTO } from "@models/ExamShiftDTO";
import { useGetExamShiftByIdQuery, useInsertExamShiftMutation, useUpdateExamShiftMutation } from "@API/services/ExamShift.service";
import dayjs from "dayjs";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateExamShift(props: IProps) {
  const { setVisible, id } = props;
  const { data: ExamShift, isLoading: LoadingExamShift } = useGetExamShiftByIdQuery({ idExamShift: id! }, { skip: !id });
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

  return (
    <div className="NewAndUpdateExamShift">
      <Spin spinning={LoadingExamShift}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"isDeleted"} hidden />
              <Form.Item label="Tên ca thi" name={"examShiftName"}>
                <Input />
              </Form.Item>
              <Form.Item
                name="timeStart"
                label="Thời gian bắt đầu"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn thời gian bắt đầu"
                  }
                ]}
              >
                <DatePicker showTime format={"DD-MM-YYYY HH:mm"} placeholder="00/00/0000 00:00" />
              </Form.Item>
              <Form.Item
                name="timeEnd"
                label="Thời gian kết thúc"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn thời gian kết thúc"
                  }
                ]}
              >
                <DatePicker showTime format={"DD-MM-YYYY HH:mm"} placeholder="00/00/0000 00:00" />
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