import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  getFileTicketLaborEquipment,
  useGetTicketLaborEquipmentByIdQuery,
  useInsertTicketLaborEquipmentMutation,
  useUpdateTicketLaborEquipmentMutation
} from "@API/services/TicketLaborEquipmentApis.service";
import { Button, Col, Form, Input, Radio, Row, Space, Spin, Tag } from "antd";
import { CustomUploadFileDrag, HandleError, normFile } from "@admin/components";
import { TicketLaborEquipmentDTO } from "@models/ticketLaborEquipmentDTO";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect } from "react";
import FormData from "form-data";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateTicketLaborEquipment(props: IProps) {
  const { setVisible, id } = props;
  const { data: TicketLaborEquipment, isLoading: LoadingTicketLaborEquipment } = useGetTicketLaborEquipmentByIdQuery(
    { idTicketLaborEquipment: id! },
    {
      skip: !id
    }
  );
  const [newTicketLaborEquipment, { isLoading: LoadingInsertTicketLaborEquipment }] =
    useInsertTicketLaborEquipmentMutation();
  const [updateTicketLaborEquipment, { isLoading: LoadingUpdateTicketLaborEquipment }] =
    useUpdateTicketLaborEquipmentMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    formRef.resetFields();
    if (TicketLaborEquipment?.payload && id) {
      formRef.setFieldsValue({
        ...TicketLaborEquipment?.payload
      });
      if (TicketLaborEquipment?.payload?.fileAttachment) {
        formRef.setFieldsValue({
          Files: [
            {
              uid: "-1",
              name: TicketLaborEquipment?.payload.id,
              status: "done",
              url: getFileTicketLaborEquipment(
                TicketLaborEquipment.payload.id + "." + TicketLaborEquipment?.payload?.fileAttachment?.split(".")?.at(1)
              )
            }
          ]
        });
      }
    } else {
      formRef.resetFields();
    }
  }, [TicketLaborEquipment, formRef, id]);
  const onfinish = async (values: TicketLaborEquipmentDTO) => {
    try {
      const dataTicket = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        if (key === "Files") return;
        if (key === "type") dataTicket.append(key, value);
        const processedValue = value || "";
        dataTicket.append(key, processedValue);
      });
      if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
        if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
          // không chỉnh sửa file
          dataTicket.append("idFile", values.Files?.at(0)?.name as string);
        } else if (values.Files?.length === 0) {
          // xóa file
          dataTicket.append("idFile", "");
        }
      } else {
        // đã chỉnh sửa file
        dataTicket.append("Files", values.Files?.at(0)?.originFileObj as Blob);
      }
      const result = id
        ? await updateTicketLaborEquipment({
            ticketLaborEquipment: dataTicket as any
          }).unwrap()
        : await newTicketLaborEquipment({ ticketLaborEquipment: dataTicket as any }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateTicketLaborEquipment">
      <Spin spinning={LoadingTicketLaborEquipment}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"idUnit"} hidden />
              <Form.Item
                label="Loại"
                name={"type"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn loại"
                  }
                ]}
              >
                <Radio.Group
                  options={[
                    {
                      label: <Tag color="blue-inverse">Mua mới</Tag>,
                      value: 0
                    },
                    {
                      label: <Tag color="green-inverse">Sửa chữa</Tag>,
                      value: 1
                    },
                    {
                      label: <Tag color="purple-inverse">Thu hồi, nhập kho</Tag>,
                      value: 2
                    }
                  ]}
                />
              </Form.Item>
              <Form.Item label="Lý do" name={"reason"}>
                <Input.TextArea />
              </Form.Item>
              <Form.Item label="Mô tả" name={"description"}>
                <Input.TextArea />
              </Form.Item>
              <Form.Item label="File" name={"Files"} getValueFromEvent={normFile} valuePropName="fileList">
                <CustomUploadFileDrag multiple={false} maxCount={1} />
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
                    loading={LoadingInsertTicketLaborEquipment || LoadingUpdateTicketLaborEquipment}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertTicketLaborEquipment || LoadingUpdateTicketLaborEquipment}
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

export const NewAndUpdateTicketLaborEquipment = WithErrorBoundaryCustom(_NewAndUpdateTicketLaborEquipment);
