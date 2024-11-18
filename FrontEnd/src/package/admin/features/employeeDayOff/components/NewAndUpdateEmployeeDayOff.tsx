import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, DatePicker, Form, Radio, Row, Select, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { EmployeeDayOffDTO } from "@models/employeeDayOffDto";
import {
  useGetEmployeeDayOffByIdQuery,
  useInsertEmployeeDayOffMutation,
  useUpdateEmployeeDayOffMutation
} from "@API/services/EmployeeDayOff.service";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";
import dayjs from "dayjs";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateEmployeeDayOff(props: IProps) {
  const { setVisible, id } = props;
  const { data: EmployeeDayOff, isLoading: LoadingEmployeeDayOff } = useGetEmployeeDayOffByIdQuery(
    { id: id! },
    {
      skip: !id
    }
  );
  const { data: ListEmployee, isLoading: LoadingListEmployee } = useGetListEmployeeQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [newEmployeeDayOff, { isLoading: LoadingInsertEmployeeDayOff }] = useInsertEmployeeDayOffMutation();
  const [updateEmployeeDayOff, { isLoading: LoadingUpdateEmployeeDayOff }] = useUpdateEmployeeDayOffMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (EmployeeDayOff?.payload && id) {
      formRef.setFieldsValue({
        ...EmployeeDayOff?.payload,
        dayOff: dayjs(EmployeeDayOff?.payload.dayOff)
      });
    } else {
      formRef.resetFields();
    }
  }, [EmployeeDayOff, formRef, id]);
  const onfinish = async (values: EmployeeDayOffDTO) => {
    try {
      const result = id
        ? await updateEmployeeDayOff({
            employeeDayOff: values
          }).unwrap()
        : await newEmployeeDayOff({ employeeDayOff: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateEmployeeDayOff">
      <Spin spinning={LoadingEmployeeDayOff}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Nhân viên" name={"idEmployee"}>
                <Select
                  loading={LoadingListEmployee}
                  optionFilterProp={"label"}
                  showSearch
                  options={ListEmployee?.listPayload?.map((x) => ({
                    label: `${x.name} - ${x.code}`,
                    value: x.id
                  }))}
                />
              </Form.Item>
              <Form.Item label="Ngày nghỉ" name={"dayOff"}>
                <DatePicker format={"DD/MM/YYYY"} />
              </Form.Item>
              <Form.Item label="Loại ngày nghỉ" name={"typeOfDayOff"}>
                <Select
                  showSearch
                  options={[
                    { label: "Nghỉ buổi sáng", value: 1 },
                    { label: "Nghỉ buổi chiều", value: 2 },
                    { label: "Nghỉ cả ngày", value: 3 },
                    { label: "Tăng ca sáng", value: 4 },
                    { label: "Tăng ca chiều", value: 5 },
                    { label: "Tăng ca tối", value: 6 }
                  ]}
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item label="Nghỉ tính phép" name={"onLeave"}>
                <Radio.Group>
                  <Radio.Button value={0}>Nghỉ có phép</Radio.Button>
                  <Radio.Button value={1}>Nghỉ không phép</Radio.Button>
                  <Radio.Button value={2}>Tăng ca</Radio.Button>
                </Radio.Group>
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
                    loading={LoadingInsertEmployeeDayOff || LoadingUpdateEmployeeDayOff}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertEmployeeDayOff || LoadingUpdateEmployeeDayOff}
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

export const NewAndUpdateEmployeeDayOff = WithErrorBoundaryCustom(_NewAndUpdateEmployeeDayOff);
