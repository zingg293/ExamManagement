import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Card, Col, Form, Row, Select, Space, Tag } from "antd";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";
import { useGetListCategoryLaborEquipmentQuery } from "@API/services/CategoryLaborEquipmentApis.service";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { useCreateLaborEquipmentUnitMutation } from "@API/services/LaborEquipmentUnitApis.service";
import { HandleError } from "@admin/components";

function _NewLaborEquipmentUnit() {
  const { data: ListEmployee, isLoading: LoadingListEmployee } = useGetListEmployeeQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitAvailableQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const { data: ListCategoryLaborEquipment, isLoading: LoadingListCategoryLaborEquipment } =
    useGetListCategoryLaborEquipmentQuery({
      pageNumber: 0,
      pageSize: 0
    });
  const [InsertTicketLaborEquipment, { isLoading: LoadingInsertTicketLaborEquipment }] =
    useCreateLaborEquipmentUnitMutation();
  const [form] = Form.useForm();
  const handleNewLaborEquipmentUnit = async (values: any) => {
    try {
      const res = await InsertTicketLaborEquipment({
        model: values
      }).unwrap();
      if (res.success) {
        form.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="NewLaborEquipmentUnit">
      <Row>
        <Col span={24}>
          <Card>
            <Form layout={"vertical"} form={form} onFinish={handleNewLaborEquipmentUnit}>
              <Form.Item name={"idEmployee"} label={"Nhân viên"}>
                <Select
                  allowClear
                  showSearch
                  options={ListEmployee?.listPayload?.map((user) => {
                    return { label: user.name + " - " + user.email, value: user.id };
                  })}
                  loading={LoadingListEmployee}
                  optionFilterProp={"label"}
                  placeholder={"Chọn nhân viên"}
                />
              </Form.Item>
              <Form.Item name={"idUnit"} label={"Phòng ban"}>
                <Select
                  allowClear
                  showSearch
                  options={ListUnit?.listPayload?.map((user) => {
                    return { label: user.unitName + " - " + user.unitCode, value: user.id };
                  })}
                  loading={LoadingListUnit}
                  optionFilterProp={"label"}
                  placeholder={"Chọn phòng ban"}
                />
              </Form.Item>
              <Form.Item
                name={"idCategoryLaborEquipment"}
                label={"Chọn thiết bị"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn thiết bị"
                  }
                ]}
              >
                <Select
                  allowClear
                  showSearch
                  options={ListCategoryLaborEquipment?.listPayload?.map((user) => {
                    return { label: user.name + " - " + user.code, value: user.id };
                  })}
                  loading={LoadingListCategoryLaborEquipment}
                  optionFilterProp={"label"}
                  placeholder={"Chọn thiết bị"}
                />
              </Form.Item>
              <Form.Item
                name={"type"}
                label={"Loại"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn loại thiết bị"
                  }
                ]}
              >
                <Select
                  allowClear
                  showSearch
                  options={[
                    { label: <Tag color={"green-inverse"}>Đang sử dụng</Tag>, value: 0 },
                    { label: <Tag color={"red-inverse"}>Đã hỏng</Tag>, value: 1 },
                    { label: <Tag color={"purple-inverse"}>Thu hồi - nhập kho</Tag>, value: 2 }
                  ]}
                  optionFilterProp={"label"}
                  placeholder={"Chọn loại thiết bị"}
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
                    loading={LoadingInsertTicketLaborEquipment}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertTicketLaborEquipment}
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
    </div>
  );
}

export const NewLaborEquipmentUnit = WithErrorBoundaryCustom(_NewLaborEquipmentUnit);
