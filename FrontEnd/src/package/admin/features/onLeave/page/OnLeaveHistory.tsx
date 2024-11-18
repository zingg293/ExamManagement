import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useState } from "react";
import { useGetListOnLeaveByHistoryQuery } from "@API/services/OnLeaveApis.service";
import { Button, Card, Col, Divider, Modal, Row, Space, TableProps, Tag, Typography } from "antd";
import { InfoCircleOutlined } from "@ant-design/icons";
import { WorkFlowHistory } from "@admin/components";
import { ViewDetailOnLeave } from "@admin/features/onLeave";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { OnLeaveDTO } from "@models/onLeaveDTO";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";

function _OnLeaveHistory() {
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const { data: ListOnLeave, isLoading: LoadingListOnLeave } = useGetListOnLeaveByHistoryQuery({
    pageSize: pagination.pageSize,
    pageNumber: pagination.pageNumber
  });

  const [modal, contextHolder] = Modal.useModal();
  const handleViewDetail = (id: string) => {
    modal.info({
      title: "Chi tiết yêu cầu",
      icon: <InfoCircleOutlined />,
      width: 900,
      content: <ViewDetailOnLeave idOnLeave={id} />,
      footer: null,
      closable: true
    });
  };

  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<OnLeaveDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setPagination({ pageNumber: pagination.current || 1, pageSize: pagination.pageSize || 10 });
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<OnLeaveDTO> = [
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
      render: (_, record) => record.workflowInstances?.at(0)?.message
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListOnLeave?.listPayload?.map((item) => ({
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
        <Button type="link" onClick={() => handleViewDetail(record.id)}>
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
  const propsTable: TableProps<OnLeaveDTO> = {
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
    dataSource: ListOnLeave?.listPayload,
    onChange: handleChange,
    loading: LoadingListOnLeave,
    pagination: {
      total: ListOnLeave?.totalElement,
      pageSize: 10,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className={"OnLeaveHistory"}>
      {contextHolder}
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Lịch sử danh sách đăng ký ngày nghỉ </Typography.Title>
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

export const OnLeaveHistory = WithErrorBoundaryCustom(_OnLeaveHistory);
