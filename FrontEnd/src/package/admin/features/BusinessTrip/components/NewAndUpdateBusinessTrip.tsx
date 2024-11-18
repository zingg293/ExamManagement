import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  getFileBusinessTrip,
  useGetBusinessTripByIdQuery,
  useInsertBusinessTripMutation,
  useUpdateBusinessTripMutation
} from "@API/services/BusinessTripApis.service";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { Button, Col, DatePicker, Form, Input, InputNumber, Row, Select, Space, Spin } from "antd";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CustomUploadFileDrag, HandleError, normFile } from "@admin/components";
import { useEffect } from "react";
import dayjs from "dayjs";

interface Props {
  id?: string;
  AfterSave?: () => void;
}

function _NewAndUpdateBusinessTrip(props: Props) {
  const { id, AfterSave } = props;
  const { data: BusinessTrip, isLoading: LoadingBusinessTrip } = useGetBusinessTripByIdQuery(
    { idBusinessTrip: id! },
    { skip: !id }
  );
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitQuery({ pageNumber: 0, pageSize: 0 });
  const [newBusinessTrip, { isLoading: LoadingInsertBusinessTrip }] = useInsertBusinessTripMutation();
  const [updateBusinessTrip, { isLoading: LoadingUpdateBusinessTrip }] = useUpdateBusinessTripMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    formRef.resetFields();
    if (BusinessTrip?.payload && id) {
      formRef.setFieldsValue({
        ...BusinessTrip?.payload,
        timeRange: [dayjs(BusinessTrip?.payload?.startDate), dayjs(BusinessTrip?.payload?.endDate)]
      });
      if (BusinessTrip?.payload?.attachments) {
        formRef.setFieldsValue({
          Files: [
            {
              idFIle: BusinessTrip?.payload.id,
              uid: "-1",
              name: BusinessTrip?.payload.attachments,
              status: "done",
              url: getFileBusinessTrip(
                BusinessTrip.payload.id + "." + BusinessTrip?.payload?.attachments?.split(".")?.at(1)
              )
            }
          ]
        });
      }
    } else {
      formRef.resetFields();
    }
  }, [BusinessTrip?.payload, formRef, id]);

  const onfinish = async (values: any) => {
    try {
      const [startDate, endDate] = values.timeRange;
      values.startDate = dayjs(startDate).format("DD/MM/YYYY hh:mm");
      values.endDate = dayjs(endDate).format("DD/MM/YYYY hh:mm");
      delete values.timeRange;

      const newDataBusinessTrip = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        if (key === "Files") return;
        const processedValue = value || ("" as any);
        newDataBusinessTrip.append(key, processedValue);
      });
      if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
        if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
          // không chỉnh sửa file
          newDataBusinessTrip.append("idFile", values.Files?.at(0)?.idFIle as string);
        } else if (values.Files?.length === 0) {
          // xóa file
          newDataBusinessTrip.append("idFile", "");
        }
      } else {
        // đã chỉnh sửa file
        newDataBusinessTrip.append("Files", values.Files?.at(0)?.originFileObj as Blob);
      }

      const result = id
        ? await updateBusinessTrip({
            businessTrip: newDataBusinessTrip as any
          }).unwrap()
        : await newBusinessTrip({
            businessTrip: newDataBusinessTrip as any
          }).unwrap();
      if (result.success) {
        AfterSave && AfterSave();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="NewAndUpdateBusinessTrip">
      <Spin spinning={LoadingBusinessTrip}>
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
                  loading={LoadingListUnit}
                  showSearch
                  onChange={(value, option: any) => {
                    formRef.setFieldsValue({ unitName: option.label });
                  }}
                  options={ListUnit?.listPayload?.map((item) => ({
                    label: item.unitName,
                    value: item.id
                  }))}
                  optionFilterProp={"label"}
                />
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
                    },
                    {
                      label: "Cuối tuần",
                      value: [dayjs().day(5).startOf("day"), dayjs().day(7).endOf("day")]
                    }
                  ]}
                />
              </Form.Item>
              <Form.Item label={"Đối tác làm việc"} name={"client"}>
                <Input />
              </Form.Item>
              <Form.Item name={"businessTripLocation"} label={"Địa điểm công tác"}>
                <Input />
              </Form.Item>
              <Form.Item name={"vehicle"} label={"Phương tiện di chuyển"}>
                <Input />
              </Form.Item>
              <Form.Item name={"expense"} label={"Kinh phí công tác"}>
                <InputNumber min={0} formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")} />
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
                    loading={LoadingInsertBusinessTrip || LoadingUpdateBusinessTrip}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertBusinessTrip || LoadingUpdateBusinessTrip}
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

export const NewAndUpdateBusinessTrip = WithErrorBoundaryCustom(_NewAndUpdateBusinessTrip);
