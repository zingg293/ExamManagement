import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { ColumnsType } from "antd/lib/table/interface";
import { App, Button, Card, Col, Divider, Form, Row, Select, Space, TableProps, Tag, Tooltip, Typography } from "antd";
import dayjs from "dayjs";
import { HandleError } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";
import { InfoCircleOutlined, SearchOutlined } from "@ant-design/icons";
import { useLazyFilterListRequestToHiredQuery } from "@API/services/RequestToHiredApis.service";
import { RequestToHiredDTO } from "@models/requestToHiredDTO";
import { DetailRequestToHired } from "@admin/features/requestToHired";
import { useGetListCategoryVacanciesApprovedQuery } from "@API/services/CategoryVacanciesApis.service";

function _ManageRequestToHired() {
  const { modal } = App.useApp();
  const [FilterRequestToHired, { data: ListRequestToHired, isLoading: LoadingListRequestToHired }] =
    useLazyFilterListRequestToHiredQuery();
  const { data: ListVacancies, isLoading: LoadingListVacancies } = useGetListCategoryVacanciesApprovedQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitAvailableQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const handleFilter = async (values: any) => {
    try {
      await FilterRequestToHired({
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
      width: 1100,
      content: <DetailRequestToHired idRequestToHired={id} />,
      footer: null,
      closable: true,
      style: { top: 20 }
    });
  };

  //#region Table config
  const columns: ColumnsType<RequestToHiredDTO> = [
    {
      title: "Vị trí",
      dataIndex: "idCategoryVacancies",
      key: "idCategoryVacancies",
      render: (text) => {
        const Vacancies = ListVacancies?.listPayload?.find((item) => item.id === text);
        return (
          <Tooltip title={Vacancies?.positionName}>
            <Tag color="purple-inverse">{Vacancies?.positionName}</Tag>
          </Tooltip>
        );
      }
    },
    {
      title: "Phòng ban",
      dataIndex: "idUnit",
      key: "idUnit",
      render: (text) => {
        const unit = ListUnit?.listPayload?.find((item) => item.id === text);
        return (
          <Tooltip title={unit?.unitName + "-" + unit?.unitCode}>
            <Tag color="blue-inverse">
              {unit?.unitName} - {unit?.unitCode}
            </Tag>
          </Tooltip>
        );
      }
    },
    {
      title: "Mô tả",
      dataIndex: "reason",
      key: "reason"
    },
    {
      title: "Số lượng",
      dataIndex: "quantity",
      key: "quantity"
    },
    {
      title: "Chi tiết",
      render: (_, record) => (
        <Button type="link" onClick={() => handleViewDetail(record.id)}>
          Xem chi tiết
        </Button>
      )
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    }
  ];
  const propsTable: TableProps<RequestToHiredDTO> = {
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
    dataSource: ListRequestToHired?.listPayload,
    loading: LoadingListRequestToHired,
    pagination: {
      total: ListRequestToHired?.totalElement,
      pageSize: 10,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className="ManageRequestToHired">
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Quản lý tuyển dụng </Typography.Title>
          <Divider />
          <Card bordered={false} className="criclebox">
            <Space
              wrap
              style={{
                width: "100%",
                justifyContent: "space-between"
              }}
            >
              <Form layout="vertical" onFinish={handleFilter}>
                <Space wrap>
                  <Form.Item name="idCategoryVacancies" label="Vị trí">
                    <Select
                      allowClear
                      showSearch
                      options={ListVacancies?.listPayload?.map((user) => {
                        return { label: user.positionName, value: user.id };
                      })}
                      loading={LoadingListVacancies}
                      optionFilterProp={"label"}
                      placeholder={"Chọn vị trí"}
                    />
                  </Form.Item>
                  <Form.Item name="idUnit" label="Phòng ban">
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

export const ManageRequestToHired = WithErrorBoundaryCustom(_ManageRequestToHired);
