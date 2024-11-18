import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, DatePicker, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { ExaminationDTO } from "@models/ExaminationDTO";
import {
  useGetExaminationByIdQuery,
  useInsertExaminationMutation,
  useUpdateExaminationMutation
} from "@API/services/Examination.service";
import dayjs from "dayjs";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateExamination(props: IProps) {
  const { setVisible, id } = props;
  const { data: Examination, isLoading: LoadingExamination } = useGetExaminationByIdQuery(
    { idExamination: id! },
    { skip: !id }
  );
  const [newExamination, { isLoading: LoadingInsertCategoryNationaly }] = useInsertExaminationMutation();
  const [updateExamination, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateExaminationMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    formRef.resetFields();
    if (Examination?.payload && id) {
      formRef.setFieldsValue(Examination?.payload);
    } else {
      formRef.resetFields();
    }
    if (Examination?.payload && id) {
      formRef.setFieldsValue({
        ...Examination?.payload,
        schoolYear: dayjs(Examination?.payload.schoolYear)
      });
    }
  }, [Examination, formRef, id]);
  const onfinish = async (values: ExaminationDTO) => {
    try {
      const result = id
        ? await updateExamination({
            Examination: values
          }).unwrap()
        : await newExamination({ Examination: values }).unwrap();
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
      <Spin spinning={LoadingExamination}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"examName"}>
                <Input />
              </Form.Item>
              <Form.Item name="schoolYear" label="Năm học">
                <DatePicker format="DD/MM/YYYY" placeholder="00/00/0000" />
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

export const NewAndUpdateExamination = WithErrorBoundaryCustom(_NewAndUpdateExamination);
