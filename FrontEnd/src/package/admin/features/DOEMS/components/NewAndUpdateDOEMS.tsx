import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { DOEMSDTO } from "@models/DOEMSDTO";
import { useGetDOEMSByIdQuery, useInsertDOEMSMutation, useUpdateDOEMSMutation } from "@API/services/DOEMS.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateDOEMS(props: IProps) {
  const { setVisible, id } = props;
  const { data: DOEMS, isLoading: LoadingDOEMS } = useGetDOEMSByIdQuery({ idDOEMS: id! }, { skip: !id });
  const [newDOEMS, { isLoading: LoadingInsertDOEMS }] = useInsertDOEMSMutation();
  const [updateDOEMS, { isLoading: LoadingUpdateDOEMS }] = useUpdateDOEMSMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (DOEMS?.payload && id) {
      formRef.setFieldsValue(DOEMS?.payload);
    } else {
      formRef.resetFields();
    }
  }, [DOEMS, formRef, id]);
  const onfinish = async (values: DOEMSDTO) => {
    try {
      const result = id
        ? await updateDOEMS({
            DOEMS: values
          }).unwrap()
        : await newDOEMS({ DOEMS: values }).unwrap();
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
      <Spin spinning={LoadingDOEMS}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"nameNationality"}>
                <Input />
              </Form.Item>
              {/*<Form.Item label="Số tiền" name={"amount"}>*/}
              {/*  <InputNumber*/}
              {/*    formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")}*/}
              {/*    parser={(value) => value!.replace(/\$\s?|(,*)/g, "")}*/}
              {/*  />*/}
              {/*</Form.Item>*/}
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
                    loading={LoadingInsertDOEMS || LoadingUpdateDOEMS}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertDOEMS || LoadingUpdateDOEMS}
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

export const NewAndUpdateDOEMS = WithErrorBoundaryCustom(_NewAndUpdateDOEMS);
