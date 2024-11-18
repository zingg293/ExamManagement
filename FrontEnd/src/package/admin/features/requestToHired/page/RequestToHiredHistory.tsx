import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Card, Col, Divider, Modal, Row, Space, TableProps, Tag, Typography } from "antd";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { InfoCircleOutlined } from "@ant-design/icons";
import { DetailRequestToHired } from "@admin/features/requestToHired";
import { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { RequestToHiredDTO } from "@models/requestToHiredDTO";
import dayjs from "dayjs";
import { useGetListRequestToHireByHistoryQuery } from "@API/services/RequestToHiredApis.service";
import { WorkFlowHistory } from "@admin/components";

function _RequestToHiredHistory() {
  const [modal, contextHolder] = Modal.useModal();
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const { data: ListRequestToHiredHistory, isLoading: LoadingListRequestToHiredHistory } =
    useGetListRequestToHireByHistoryQuery({
      pageNumber: pagination.pageNumber,
      pageSize: pagination.pageSize
    });
  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<RequestToHiredDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setPagination({ pageNumber: pagination.current || 1, pageSize: pagination.pageSize || 10 });
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<RequestToHiredDTO> = [
    {
      title: "STT",
      dataIndex: "index",
      key: "index",
      render: (text, record, index) => index + 1,
      width: 50
    },
    {
      title: "Số phiếu",
      render: (_, record) => record.workflowInstances?.at(0)?.workflowCode
    },
    {
      title: "Trạng thái",
      render: (_, record) => (
        <Tag color={record?.currentWorkFlowStep?.statusColor}>{record.workflowInstances?.at(0)?.nameStatus}</Tag>
      )
    },
    {
      title: "Ghi chú",
      render: (_, record) => record.workflowInstances?.at(0)?.message
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListRequestToHiredHistory?.listPayload?.map((item) => ({
        text: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm"),
        value: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm")
      })),
      filteredValue: filteredInfo?.createdDate || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.createdDate.toString().startsWith(value),
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Chi tiết",
      render: (_, record) => (
        <Button
          type="link"
          onClick={() =>
            modal.info({
              title: `Chi tiết phiếu ${record.workflowInstances?.at(0)?.workflowCode}`,
              icon: <InfoCircleOutlined />,
              width: 900,
              content: <DetailRequestToHired idRequestToHired={record.id} />,
              footer: null,
              closable: true
            })
          }
        >
          Xem chi tiết
        </Button>
      )
    },
    {
      title: "Lịch sử",
      width: 110,
      render: (_, record) => <WorkFlowHistory workFlowInstance={record?.workflowInstances?.at(0)} />
    },
    {
      title: "Hiệu chỉnh",
      dataIndex: "Action",
      fixed: "right",
      width: "8%"
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
    dataSource: ListRequestToHiredHistory?.listPayload,
    onChange: handleChange,
    loading: LoadingListRequestToHiredHistory,
    pagination: {
      total: ListRequestToHiredHistory?.totalElement,
      pageSize: 10,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className="RequestToHiredHistory">
      {contextHolder}
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Lịch sử danh sách phiếu yêu cầu tuyển dụng </Typography.Title>
          <Divider />
          <Space
            style={{
              marginBottom: 16
            }}
            wrap
          ></Space>
          <Card bordered={false} className="criclebox">
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const RequestToHiredHistory = WithErrorBoundaryCustom(_RequestToHiredHistory);
