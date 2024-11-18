import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, DatePicker, Form, Input, Row, Select, Space, Spin, Tag } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { RoomDTO } from "@models/RoomDTO";
import { useGetRoomByIdQuery, useInsertRoomMutation, useUpdateRoomMutation } from "@API/services/Room.service";
import dayjs from "dayjs";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateRoom(props: IProps) {
  const { setVisible, id } = props;
  const { data: Room, isLoading: LoadingRoom } = useGetRoomByIdQuery({ idRoom: id! }, { skip: !id });
  const [newRoom, { isLoading: LoadingInsertCategoryNationaly }] = useInsertRoomMutation();
  const [updateRoom, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateRoomMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (Room?.payload && id) {
      formRef.setFieldsValue(Room?.payload);
    } else {
      formRef.resetFields();
    }
    formRef.resetFields();
    if (Room?.payload && id) {
      const { fromDate, toDate, ...restPayload } = Room.payload;
      formRef.setFieldsValue({
        ...restPayload,
        fromDate: fromDate ? dayjs(fromDate) : null,
        toDate: toDate ? dayjs(toDate) : null
      });
    }
  }, [Room, formRef, id]);
  const onfinish = async (values: RoomDTO) => {
    try {
      const result = id
        ? await updateRoom({
            Room: values
          }).unwrap()
        : await newRoom({ Room: values }).unwrap();
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
      <Spin spinning={LoadingRoom}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên phòng thi" name={"roomName"}>
                <Input />
              </Form.Item>
              <Form.Item label="Trạng thái" name={"isActive"}>
                <Select
                  allowClear
                  options={[
                    { label: <Tag color="red-inverse">Ẩn</Tag>, value: false },
                    { label: <Tag color="green-inverse">Hiển thị</Tag>, value: true }
                  ]}
                  placeholder={"Chọn trạng thái"}
                />
              </Form.Item>
              <Form.Item label="Tình trạng: " name={"isOrder"}>
                <Select
                  allowClear
                  options={[
                    { label: <Tag color="red-inverse">Đã có lịch thi</Tag>, value: false },
                    { label: <Tag color="green-inverse">Trống</Tag>, value: true }
                  ]}
                  placeholder={"Chọn trạng thái"}
                />
              </Form.Item>
              <Form.Item
                name="fromDate"
                label="Từ Ngày"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn ngày"
                  }
                ]}
              >
                <DatePicker format={"DD-MM-YYYY hh:mm"} placeholder="00/00/0000" />
              </Form.Item>
              <Form.Item
                name="toDate"
                label="Đến Ngày"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn ngày"
                  }
                ]}
              >
                <DatePicker format={"DD-MM-YYYY hh:mm"} placeholder="00/00/0000" />
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

export const NewAndUpdateRoom = WithErrorBoundaryCustom(_NewAndUpdateRoom);
