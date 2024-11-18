import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { ColumnsType } from "antd/lib/table/interface";
import {
  App,
  Button,
  Card,
  Col,
  DatePicker,
  Descriptions,
  Divider,
  Form,
  Row,
  Select,
  Space,
  TableProps,
  Tag,
  Tooltip,
  Typography
} from "antd";
import { OvertimeDTO } from "@models/overtimeDTO";
import dayjs from "dayjs";
import { HandleError } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";
import { SearchOutlined, UserOutlined } from "@ant-design/icons";
import {
  getFileExcelMarkWorkPoints,
  useLazyGetListMarkWorkPointsQuery
} from "@API/services/MarkWorkPointsApis.service";
import { OnLeaveDTO } from "@models/onLeaveDTO";
import { DetailEmployee } from "@admin/features/employee";

function _ManageCalculateWorkingDays() {
  const { modal } = App.useApp();
  const [form] = Form.useForm();
  const [FilterCalculateWorkingDays, { data: ListCalculateWorkingDays, isLoading: LoadingListCalculateWorkingDays }] =
    useLazyGetListMarkWorkPointsQuery();
  const dataEmployee = ListCalculateWorkingDays?.payload?.employee;
  const { data: ListEmployee, isLoading: LoadingListEmployee } = useGetListEmployeeQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const handleFilter = async (values: any) => {
    try {
      if (values.fromDate) values.fromDate = dayjs(values.fromDate).format("MM/YYYY");
      if (values.toDate) values.toDate = dayjs(values.toDate).format("MM/YYYY");
      await FilterCalculateWorkingDays({
        ...values,
        pageNumber: 0,
        pageSize: 0
      }).unwrap();
    } catch (e: any) {
      await HandleError(e);
    }
  };

  const handleViewDetailEmployee = (id: string) => {
    return modal.info({
      title: "Thông tin nhân viên",
      width: "80%",
      content: <DetailEmployee idEmployee={id} />,
      footer: null,
      closable: true,
      icon: <UserOutlined />
    });
  };
  const handleExportReport = async (values: any) => {
    try {
      if (values.fromDate) values.fromDate = dayjs(values.fromDate).format("MM/YYYY");
      if (values.toDate) values.toDate = dayjs(values.toDate).format("MM/YYYY");
      const res = await getFileExcelMarkWorkPoints(values);
      const data = res?.data as Blob;
      const file = new Blob([data]);

      const a = document.createElement("a");
      a.href = window.URL.createObjectURL(file);

      a.download = "report.xlsx";

      a.click();
    } catch (e: any) {
      await HandleError(e);
    }
  };

  //#region Table overtime config
  const columnsOvertime: ColumnsType<OvertimeDTO> = [
    {
      title: "Thời gian",
      render(text, record) {
        const fromDate = dayjs(record.fromDate).format("DD-MM-YYYY HH:mm");
        const toDate = dayjs(record.toDate).format("DD-MM-YYYY HH:mm");
        return (
          <Tooltip title={fromDate + "=>" + toDate}>
            <Space>
              <Tag color="green-inverse">{fromDate}</Tag>
              {"=>"}
              <Tag color="red-inverse">{toDate}</Tag>
            </Space>
          </Tooltip>
        );
      }
    },
    {
      title: "Phòng ban",
      dataIndex: "unitName",
      key: "unitName"
    },
    {
      title: "Ghi chú",
      dataIndex: "description",
      key: "description"
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    }
  ];
  const propsTableOvertime: TableProps<OvertimeDTO> = {
    scroll: {
      x: 800
    },
    bordered: true,
    rowKey: (record) => record.id,
    columns: columnsOvertime.map((item) => ({
      ellipsis: true,
      with: 150,
      ...item
    })),
    dataSource: ListCalculateWorkingDays?.payload.overtimes,
    loading: LoadingListCalculateWorkingDays,
    pagination: {
      total: ListCalculateWorkingDays?.totalElement,
      pageSize: 5,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small",
    title: () => <Typography.Title level={5}>Tăng ca</Typography.Title>
  };
  //#endregion

  //#region Table onLeave config
  const columnsOnLeave: ColumnsType<OnLeaveDTO> = [
    {
      title: "Thời gian",
      render(text, record) {
        const fromDate = dayjs(record.fromDate).format("DD-MM-YYYY HH:mm");
        const toDate = dayjs(record.toDate).format("DD-MM-YYYY HH:mm");
        return (
          <Tooltip title={fromDate + "=>" + toDate}>
            <Space>
              <Tag color="green-inverse">{fromDate}</Tag>
              {"=>"}
              <Tag color="red-inverse">{toDate}</Tag>
            </Space>
          </Tooltip>
        );
      }
    },
    {
      title: "Phòng ban",
      dataIndex: "unitName",
      key: "unitName"
    },
    {
      title: "Nghỉ phép",
      render: (_, record) => {
        const unpaidLeave = record?.unPaidLeave;
        return unpaidLeave ? (
          <Tag color={"green-inverse"}>Nghỉ có lương</Tag>
        ) : (
          <Tag color={"red-inverse"}>Nghỉ không lương</Tag>
        );
      }
    },
    {
      title: "Ghi chú",
      dataIndex: "description",
      key: "description"
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    }
  ];
  const propsTableOnLeave: TableProps<OnLeaveDTO> = {
    scroll: {
      x: 800
    },
    bordered: true,
    rowKey: (record) => record.id,
    columns: columnsOnLeave.map((item) => ({
      ellipsis: true,
      with: 150,
      ...item
    })),
    dataSource: ListCalculateWorkingDays?.payload.onLeaves,
    loading: LoadingListCalculateWorkingDays,
    pagination: {
      total: ListCalculateWorkingDays?.totalElement,
      pageSize: 5,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small",
    title: () => <Typography.Title level={5}>Ngày nghỉ</Typography.Title>
  };
  //#endregion
  return (
    <div className="ManageCalculateWorkingDays">
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Quản lý chấm công </Typography.Title>
          <Divider />
          <Card bordered={false} className="criclebox">
            <Space
              style={{
                marginBottom: 16
              }}
              wrap
            >
              <Form form={form} layout="vertical" onFinish={handleFilter}>
                <Space>
                  <Form.Item
                    name="idEmployee"
                    label="Nhân viên"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng chọn nhân viên"
                      }
                    ]}
                  >
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
                  <Form.Item
                    name="fromDate"
                    label="Thời gian"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng chọn ngày"
                      }
                    ]}
                  >
                    <DatePicker.MonthPicker format="MM-YYYY" />
                  </Form.Item>
                  <Form.Item label={"Tìm kiếm"}>
                    <Button type="primary" htmlType="submit" icon={<SearchOutlined />}>
                      Tìm kiếm
                    </Button>
                  </Form.Item>
                </Space>
              </Form>
            </Space>
            {dataEmployee && (
              <Row gutter={[24, 24]}>
                <Col span={24}>
                  <Card
                    bordered={true}
                    extra={
                      <Space>
                        <Button
                          type="primary"
                          size={"middle"}
                          onClick={() => handleViewDetailEmployee(dataEmployee.id)}
                        >
                          Xem chi tiết
                        </Button>
                        <Button
                          type="primary"
                          size={"middle"}
                          onClick={() => handleExportReport(form.getFieldsValue())}
                        >
                          Xuất báo cáo
                        </Button>
                      </Space>
                    }
                    title={<Typography.Title level={5}>Thông tin nhân viên</Typography.Title>}
                  >
                    <Descriptions column={1} bordered layout={"horizontal"}>
                      <Descriptions.Item label="Họ và tên">{dataEmployee?.name}</Descriptions.Item>
                      <Descriptions.Item label="Email">{dataEmployee?.email}</Descriptions.Item>
                      <Descriptions.Item label="Mã">{dataEmployee?.code}</Descriptions.Item>
                      <Descriptions.Item label="SĐT">{dataEmployee?.phone}</Descriptions.Item>
                    </Descriptions>
                  </Card>
                </Col>
                {/*<Col span={12}>*/}
                {/*  <Card*/}
                {/*    bordered={true}*/}
                {/*    title={<Typography.Title level={5}>Báo cáo</Typography.Title>}*/}
                {/*    extra={*/}
                {/*      <Button type="primary" size={"small"} onClick={() => handleExportReport(form.getFieldsValue())}>*/}
                {/*        Xuất báo cáo*/}
                {/*      </Button>*/}
                {/*    }*/}
                {/*  >*/}
                {/*    <Descriptions column={1} bordered layout={"horizontal"}>*/}
                {/*      <Descriptions.Item label="Ngày nghỉ"> </Descriptions.Item>*/}
                {/*      <Descriptions.Item label="Tổng số ngày làm việc(trừ t7)"> </Descriptions.Item>*/}
                {/*      <Descriptions.Item label="Tăng ca"> </Descriptions.Item>*/}
                {/*      <Descriptions.Item label="Tổng số ngày làm việc(trừ t7,cn)"> </Descriptions.Item>*/}
                {/*    </Descriptions>*/}
                {/*  </Card>*/}
                {/*</Col>*/}
              </Row>
            )}
            <Typography.Title />
            <DragAndDropTable {...propsTableOvertime} />
            <Typography.Title />
            <DragAndDropTable {...propsTableOnLeave} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const ManageCalculateWorkingDays = WithErrorBoundaryCustom(_ManageCalculateWorkingDays);
