import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Card, Col, Form, InputNumber, Row, Select, Space, Spin, Typography } from "antd";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, MinusCircleOutlined, PlusOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect } from "react";
import {
  useGetTicketLaborEquipmentByIdQuery,
  useUpdateTicketLaborEquipmentDetailMutation
} from "@API/services/TicketLaborEquipmentApis.service";
import { useGetListCategoryLaborEquipmentQuery } from "@API/services/CategoryLaborEquipmentApis.service";
import { TicketLaborEquipmentDetailDTO } from "@models/ticketLaborEquipmentDetailDTO";
import { useGetUserQuery } from "@API/services/UserApis.service";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _TicketLaborEquipmentDetail(props: IProps) {
  const { id } = props;
  const { data: User } = useGetUserQuery({
    fetch: true
  });
  const { data: TicketLaborEquipment, isLoading: LoadingTicketLaborEquipment } = useGetTicketLaborEquipmentByIdQuery(
    { idTicketLaborEquipment: id! },
    {
      skip: !id
    }
  );
  const { data: ListEmployee } = useGetListEmployeeQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const idEmployee = ListEmployee?.listPayload?.find((item) => item.idUser === User?.payload?.data?.id)?.id;
  const { data: ListCategoryLaborEquipment, isLoading: LoadingListCategoryLaborEquipment } =
    useGetListCategoryLaborEquipmentQuery({
      pageSize: 0,
      pageNumber: 0
    });
  const [UpdateTicketLaborEquipmentDetail, { isLoading: isLoadingUpdateTicketLaborEquipmentDetail }] =
    useUpdateTicketLaborEquipmentDetailMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    if (TicketLaborEquipment?.payload) {
      formRef.setFieldsValue({
        ListTicketLaborEquipmentDetail: TicketLaborEquipment?.payload?.ticketLaborEquipmentDetail
      });
    }
  }, [TicketLaborEquipment?.payload, formRef]);
  const onfinish = async (values: { ListTicketLaborEquipmentDetail: TicketLaborEquipmentDetailDTO[] }) => {
    try {
      await UpdateTicketLaborEquipmentDetail(values?.ListTicketLaborEquipmentDetail).unwrap();
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="TicketLaborEquipmentDetail">
      <Spin spinning={LoadingTicketLaborEquipment} size="large">
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
            <Card>
              <Form layout={"vertical"} labelCol={{ span: 6 }} form={formRef} onFinish={onfinish}>
                <Form.List name={"ListTicketLaborEquipmentDetail"}>
                  {(fields, { add, remove }) => (
                    <Space
                      wrap
                      direction={"vertical"}
                      style={{
                        width: "100%"
                      }}
                    >
                      {fields.map((field, index) => (
                        <Space key={field.key} align={"baseline"} wrap>
                          <Typography.Text strong>{index + 1}.</Typography.Text>
                          <Form.Item name={[field.name, "idTicketLaborEquipment"]} hidden initialValue={id} />
                          <Form.Item
                            name={[field.name, "id"]}
                            hidden
                            initialValue={"00000000-0000-0000-0000-000000000000"}
                          />
                          <Form.Item
                            {...field}
                            name={[field.name, "idCategoryLaborEquipment"]}
                            rules={[{ required: true, message: "Vui lòng chọn" }]}
                          >
                            <Select
                              showSearch={true}
                              placeholder="Chọn thiết bị lao động - mã thiết bị lao động"
                              loading={LoadingListCategoryLaborEquipment}
                              options={ListCategoryLaborEquipment?.listPayload?.map((item) => ({
                                label: `${item.name} - ${item.code}`,
                                value: item.id
                              }))}
                              style={{
                                maxWidth: 500
                              }}
                            />
                          </Form.Item>
                          <Form.Item {...field} name={[field.name, "idEmployee"]} hidden initialValue={idEmployee} />
                          <Form.Item {...field} name={[field.name, "quantity"]} initialValue={1}>
                            <InputNumber min={1} />
                          </Form.Item>
                          <MinusCircleOutlined onClick={() => remove(field.name)} />
                        </Space>
                      ))}
                      <Form.Item>
                        <Button type="default" onClick={() => add()} block icon={<PlusOutlined />}>
                          Thêm
                        </Button>
                      </Form.Item>
                    </Space>
                  )}
                </Form.List>
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
                      loading={isLoadingUpdateTicketLaborEquipmentDetail}
                      icon={<RetweetOutlined />}
                    >
                      Xóa
                    </Button>
                    <Button
                      type="primary"
                      htmlType="submit"
                      loading={isLoadingUpdateTicketLaborEquipmentDetail}
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
            </Card>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const TicketLaborEquipmentDetail = WithErrorBoundaryCustom(_TicketLaborEquipmentDetail);
