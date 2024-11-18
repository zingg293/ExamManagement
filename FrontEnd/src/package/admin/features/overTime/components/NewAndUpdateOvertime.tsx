import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useGetOvertimeByIdQuery,
  useInsertOvertimeMutation,
  useUpdateOvertimeMutation
} from "@API/services/OvertimeApis.service";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { Button, Col, DatePicker, Form, Input, Row, Select, Space, Spin } from "antd";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { HandleError } from "@admin/components";
import { useEffect } from "react";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";
import dayjs from "dayjs";

interface Props {
  id?: string;
  AfterSave?: () => void;
}

function _NewAndUpdateOvertime(props: Props) {
  const { id, AfterSave } = props;
  const { data: Overtime, isLoading: LoadingOvertime } = useGetOvertimeByIdQuery({ idOvertime: id! }, { skip: !id });
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitQuery({ pageNumber: 0, pageSize: 0 });
  const { data: ListEmployee, isLoading: LoadingListEmployee } = useGetListEmployeeQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const [newOvertime, { isLoading: LoadingInsertOvertime }] = useInsertOvertimeMutation();
  const [updateOvertime, { isLoading: LoadingUpdateOvertime }] = useUpdateOvertimeMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    formRef.resetFields();
    if (Overtime?.payload && id) {
      formRef.setFieldsValue({
        ...Overtime?.payload,
        RangeTime: [dayjs(Overtime?.payload?.fromDate), dayjs(Overtime?.payload?.toDate)]
      });
    } else {
      formRef.resetFields();
    }
  }, [Overtime?.payload, formRef, id]);

  const onfinish = async (values: any) => {
    try {
      const formDate = values.RangeTime[0];
      const toDate = values.RangeTime[1];
      values = {
        ...values,
        fromDate: formDate.format("DD/MM/YYYY HH:mm"),
        toDate: toDate.format("DD/MM/YYYY HH:mm")
      };
      delete values.RangeTime;
      const result = id
        ? await updateOvertime({
            overtime: values
          }).unwrap()
        : await newOvertime({
            overtime: values
          }).unwrap();
      if (result.success) {
        AfterSave && AfterSave();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="NewAndUpdateOvertime">
      <Spin spinning={LoadingOvertime}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Mô tả" name={"description"}>
                <Input.TextArea />
              </Form.Item>
              <Form.Item label="Tên phòng ban" name={"unitName"} hidden>
                <Input />
              </Form.Item>
              <Form.Item
                label="Lịch gác thi"
                name={"idUnit"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn lịch gác thi"
                  }
                ]}
              >
                <Select
                  loading={LoadingListUnit}
                  showSearch
                  onChange={(_, option: any) => {
                    formRef.setFieldsValue({
                      unitName: option?.label
                    });
                  }}
                  options={ListUnit?.listPayload?.map((item) => ({
                    label: item.unitName,
                    value: item.id
                  }))}
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item
                label="Giảng viên"
                name={"idEmployee"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn giảng viên"
                  }
                ]}
              >
                <Select
                  loading={LoadingListEmployee}
                  showSearch
                  options={ListEmployee?.listPayload?.map((item) => ({
                    label: item.name + " - " + item.code,
                    value: item.id
                  }))}
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item name={"RangeTime"} label={"Thòi gian"} initialValue={[dayjs(), dayjs().add(2, "hour")]}>
                <DatePicker.RangePicker format={"DD/MM/YYYY HH:mm"} showTime />
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
                    loading={LoadingInsertOvertime || LoadingUpdateOvertime}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertOvertime || LoadingUpdateOvertime}
                    icon={<CheckCircleOutlined />}
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

export const NewAndUpdateOvertime = WithErrorBoundaryCustom(_NewAndUpdateOvertime);
