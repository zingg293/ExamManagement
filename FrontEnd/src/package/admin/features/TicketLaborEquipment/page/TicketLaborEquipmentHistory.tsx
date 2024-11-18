import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Card, Col, Divider, Modal, Row, Space, TableProps, Tag, Typography } from "antd";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { InfoCircleOutlined } from "@ant-design/icons";
import { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import dayjs from "dayjs";
import { WorkFlowHistory } from "@admin/components";
import { TicketLaborEquipmentDTO } from "@models/ticketLaborEquipmentDTO";
import { useGetListTicketLaborEquipmentByHistoryQuery } from "@API/services/TicketLaborEquipmentApis.service";
import {
  listColorForTypeTicketLaborEquipment,
  ViewTicketLaborEquipment
} from "@admin/features/TicketLaborEquipment/components/ViewTicketLaborEquipment";

function _TicketLaborEquipmentHistory() {
  const [modal, contextHolder] = Modal.useModal();
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const { data: ListTicketLaborEquipmentHistory, isLoading: LoadingListTicketLaborEquipmentHistory } =
    useGetListTicketLaborEquipmentByHistoryQuery({
      pageNumber: pagination.pageNumber,
      pageSize: pagination.pageSize
    });
  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<TicketLaborEquipmentDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setPagination({ pageNumber: pagination.current || 1, pageSize: pagination.pageSize || 10 });
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<TicketLaborEquipmentDTO> = [
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
      title: "Loại",
      render: (_, record) => listColorForTypeTicketLaborEquipment(record?.type),
      width: 120
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListTicketLaborEquipmentHistory?.listPayload?.map((item) => ({
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
              content: <ViewTicketLaborEquipment idTicketLaborEquipment={record.id} />,
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
  const propsTable: TableProps<TicketLaborEquipmentDTO> = {
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
    dataSource: ListTicketLaborEquipmentHistory?.listPayload,
    onChange: handleChange,
    loading: LoadingListTicketLaborEquipmentHistory,
    pagination: {
      total: ListTicketLaborEquipmentHistory?.totalElement,
      pageSize: 10,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className="TicketLaborEquipmentHistory">
      {contextHolder}
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Lịch sử danh sách phiếu yêu cầu thiết bị lao động </Typography.Title>
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

export const TicketLaborEquipmentHistory = WithErrorBoundaryCustom(_TicketLaborEquipmentHistory);
