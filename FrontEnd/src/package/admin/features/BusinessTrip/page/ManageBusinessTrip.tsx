import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { ColumnsType } from "antd/lib/table/interface";
import {
  App,
  Button,
  Card,
  Col,
  DatePicker,
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
import dayjs from "dayjs";
import { HandleError } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { InfoCircleOutlined, SearchOutlined } from "@ant-design/icons";
import { useLazyFilterListBusinessTripQuery } from "@API/services/BusinessTripApis.service";
import { BusinessTripDTO } from "@models/businessTripDTO";
import { ViewDetailBusinessTrip } from "@admin/features/BusinessTrip";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";

function _ManageBusinessTrip() {
  const { modal } = App.useApp();
  const [FilterBusinessTrip, { data: ListBusinessTrip, isLoading: LoadingListBusinessTrip }] =
    useLazyFilterListBusinessTripQuery();
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitAvailableQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const handleFilter = async (values: any) => {
    try {
      if (values.startDate) values.startDate = dayjs(values.startDate).format("DD/MM/YYYY HH:mm");
      if (values.endDate) values.endDate = dayjs(values.endDate).format("DD/MM/YYYY HH:mm");
      await FilterBusinessTrip({
        ...values,
        pageNumber: 0,
        pageSize: 0
      }).unwrap();
    } catch (e: any) {
      await HandleError(e);
    }
  };
  const handleViewDetail = (id: string) => {
    modal.info({
      title: "Chi tiết yêu cầu",
      icon: <InfoCircleOutlined />,
      width: 900,
      content: <ViewDetailBusinessTrip idBusinessTrip={id} />,
      footer: null,
      closable: true
    });
  };

  //#region Table config
  const columns: ColumnsType<BusinessTripDTO> = [
    {
      title: "Phòng ban",
      dataIndex: "unitName",
      key: "unitName",
      render: (text) => <Tag color="blue-inverse">{text}</Tag>
    },
    {
      title: "Thời gian",
      render(text, record) {
        const startDate = dayjs(record.startDate).format("DD-MM-YYYY HH:mm");
        const endDate = dayjs(record.endDate).format("DD-MM-YYYY HH:mm");
        return (
          <Tooltip title={startDate + "=>" + endDate}>
            <Space>
              <Tag color="green-inverse">{startDate}</Tag>
              {"=>"}
              <Tag color="red-inverse">{endDate}</Tag>
            </Space>
          </Tooltip>
        );
      }
    },
    {
      title: "Ghi chú",
      dataIndex: "description",
      key: "description"
    },
    {
      title: "Chi tiết",
      render: (_, record) => (
        <Button type="link" onClick={() => handleViewDetail(record.id)}>
          Xem chi tiết
        </Button>
      ),
      width: 120
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm"),
      width: 150
    }
  ];
  const propsTable: TableProps<BusinessTripDTO> = {
    scroll: {
      x: 800
    },
    bordered: true,
    rowKey: (record) => record.id,
    columns: columns.map((item) => ({
      ellipsis: true,
      with: 150,
      ...item
    })),
    dataSource: ListBusinessTrip?.listPayload,
    loading: LoadingListBusinessTrip,
    pagination: {
      total: ListBusinessTrip?.totalElement,
      pageSize: 10,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className="ManageBusinessTrip">
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Quản lý đăng ký công tác </Typography.Title>
          <Divider />
          <Card bordered={false} className="criclebox">
            <Space
              style={{
                marginBottom: 16
              }}
              wrap
            >
              <Form layout="vertical" onFinish={handleFilter}>
                <Space>
                  <Form.Item name="idUnit" label="Phòng ban">
                    <Select
                      allowClear
                      showSearch
                      options={ListUnit?.listPayload?.map((user) => {
                        return { label: user.unitName + " - " + user.unitCode, value: user.id };
                      })}
                      loading={LoadingListUnit}
                      optionFilterProp={"label"}
                      placeholder={"Chọn nhân viên"}
                    />
                  </Form.Item>
                  <Form.Item name="startDate" label="Từ ngày">
                    <DatePicker format="DD-MM-YYYY HH:mm" showTime />
                  </Form.Item>
                  <Form.Item name="endDate" label="Đến ngày">
                    <DatePicker format="DD-MM-YYYY HH:mm" showTime />
                  </Form.Item>
                  <Form.Item label={"Tìm kiếm"}>
                    <Button type="primary" htmlType="submit" icon={<SearchOutlined />}>
                      Tìm kiếm
                    </Button>
                  </Form.Item>
                </Space>
              </Form>
            </Space>
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const ManageBusinessTrip = WithErrorBoundaryCustom(_ManageBusinessTrip);
