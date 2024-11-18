import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  getFileOnLeave,
  useGetOnLeaveByIdQuery,
  useInsertOnLeaveMutation,
  useUpdateOnLeaveMutation
} from "@API/services/OnLeaveApis.service";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { Button, Col, DatePicker, Form, Input, Radio, Row, Select, Space, Spin, Tag } from "antd";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CustomUploadFileDrag, HandleError, normFile } from "@admin/components";
import { useEffect } from "react";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";
import FormData from "form-data";
import dayjs from "dayjs";

interface Props {
  id?: string;
  AfterSave?: () => void;
}

function _NewAndUpdateOnLeave(props: Props) {
  const { id, AfterSave } = props;
  const { data: OnLeave, isLoading: LoadingOnLeave } = useGetOnLeaveByIdQuery({ idOnLeave: id! }, { skip: !id });
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitQuery({ pageNumber: 0, pageSize: 0 });
  const { data: ListEmployee, isLoading: LoadingListEmployee } = useGetListEmployeeQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const [newOnLeave, { isLoading: LoadingInsertOnLeave }] = useInsertOnLeaveMutation();
  const [updateOnLeave, { isLoading: LoadingUpdateOnLeave }] = useUpdateOnLeaveMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    formRef.resetFields();
    if (OnLeave?.payload && id) {
      formRef.setFieldsValue({
        ...OnLeave?.payload,
        timeRange: [dayjs(OnLeave?.payload?.fromDate), dayjs(OnLeave?.payload?.toDate)]
      });
      if (OnLeave?.payload?.attachments) {
        formRef.setFieldsValue({
          Files: [
            {
              uid: "-1",
              name: OnLeave?.payload.id,
              status: "done",
              url: getFileOnLeave(OnLeave.payload.id + "." + OnLeave?.payload?.attachments?.split(".")?.at(1))
            }
          ]
        });
      }
    } else {
      formRef.resetFields();
    }
  }, [OnLeave?.payload, formRef, id]);

  const onfinish = async (values: any) => {
    try {
      const [formDate, toDate] = values.timeRange;
      values.formDate = dayjs(formDate).format("DD/MM/YYYY hh:mm");
      values.toDate = dayjs(toDate).format("DD/MM/YYYY hh:mm");
      delete values.timeRange;
      const dataOnLeave = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        if (key === "Files") return;
        const processedValue = value || "";
        dataOnLeave.append(key, processedValue);
      });
      if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
        if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
          // không chỉnh sửa file
          dataOnLeave.append("idFile", values.Files?.at(0)?.name as string);
        } else if (values.Files?.length === 0) {
          // xóa file
          dataOnLeave.append("idFile", "");
        }
      } else {
        // đã chỉnh sửa file
        dataOnLeave.append("Files", values.Files?.at(0)?.originFileObj as Blob);
      }
      const result = id
        ? await updateOnLeave({
            onLeave: dataOnLeave as any
          }).unwrap()
        : await newOnLeave({
            onLeave: dataOnLeave as any
          }).unwrap();
      if (result.success) {
        AfterSave && AfterSave();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="NewAndUpdateOnLeave">
      <Spin spinning={LoadingOnLeave}>
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
                label="Phòng ban"
                name={"idUnit"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn phòng ban"
                  }
                ]}
              >
                <Select
                  onChange={(value, option: any) => {
                    formRef.setFieldsValue({ unitName: option.label });
                  }}
                  loading={LoadingListUnit}
                  showSearch
                  options={ListUnit?.listPayload?.map((item) => ({
                    label: item.unitName,
                    value: item.id
                  }))}
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item
                label="Nhân viên"
                name={"idEmployee"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn nhân viên"
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
              <Form.Item name={"unPaidLeave"} initialValue={"false"}>
                <Radio.Group>
                  <Radio value={"true"}>
                    <Tag color={"green-inverse"}>Nghỉ có lương</Tag>
                  </Radio>
                  <Radio value={"false"}>
                    <Tag color={"red-inverse"}>Nghỉ không lương</Tag>
                  </Radio>
                </Radio.Group>
              </Form.Item>
              <Form.Item
                label="Thời gian"
                name={"timeRange"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn thời gian"
                  }
                ]}
              >
                <DatePicker.RangePicker
                  format={"DD-MM-YYYY hh:mm"}
                  showTime
                  presets={[
                    {
                      label: "Hôm nay",
                      value: [dayjs().startOf("day"), dayjs().endOf("day")]
                    },
                    {
                      label: "3 ngày ",
                      value: [dayjs().startOf("day"), dayjs().add(3, "day").endOf("day")]
                    },
                    {
                      label: "7 ngày",
                      value: [dayjs().startOf("day"), dayjs().add(7, "day").endOf("day")]
                    },
                    {
                      label: "14 ngày",
                      value: [dayjs().startOf("day"), dayjs().add(14, "day").endOf("day")]
                    },
                    {
                      label: "Tuần này",
                      value: [dayjs().startOf("week"), dayjs().endOf("week")]
                    },
                    {
                      label: "Tháng này",
                      value: [dayjs().startOf("month"), dayjs().endOf("month")]
                    }
                  ]}
                />
              </Form.Item>
              <Form.Item label="File đính kèm" name={"Files"} getValueFromEvent={normFile} valuePropName="fileList">
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
                    loading={LoadingInsertOnLeave || LoadingUpdateOnLeave}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertOnLeave || LoadingUpdateOnLeave}
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

export const NewAndUpdateOnLeave = WithErrorBoundaryCustom(_NewAndUpdateOnLeave);
