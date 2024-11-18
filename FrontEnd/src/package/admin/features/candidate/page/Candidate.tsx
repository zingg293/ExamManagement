import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useGetListCandidateQuery } from "@API/services/CandidateApis.service";
import { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { App, Button, Card, Col, Divider, Row, Space, TableProps, Typography } from "antd";
import { CandidateDTO } from "@models/candidateDTO";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { DetailCandidate } from "@admin/features/candidate";
import { UserOutlined } from "@ant-design/icons";

function _Candidate() {
  const { modal } = App.useApp();
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const { data: ListCandidate, isLoading: LoadingListCandidate } = useGetListCandidateQuery({
    pageSize: pagination.pageSize,
    pageNumber: pagination.pageNumber
  });

  const handleViewDetail = (id: string) => {
    modal.info({
      title: "Chi tiết ứng viên",
      content: <DetailCandidate idCandidate={id} />,
      width: 1000,
      footer: null,
      closable: true,
      icon: <UserOutlined />
    });
  };

  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<CandidateDTO>["onChange"] = (pagination, filters) => {
    setPagination({ pageNumber: pagination.current || 1, pageSize: pagination.pageSize || 10 });
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<CandidateDTO> = [
    {
      title: "Tên",
      dataIndex: "name",
      key: "name"
    },
    {
      title: "Email",
      dataIndex: "email",
      key: "email"
    },
    {
      title: "Ghi chú",
      dataIndex: "note",
      key: "note"
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListCandidate?.listPayload?.map((item) => ({
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
        <Button type="link" size={"small"} onClick={() => handleViewDetail(record.id)}>
          Xem chi tiết
        </Button>
      )
    },
    {
      title: "Hiệu chỉnh",
      dataIndex: "Action",
      key: "Action",
      fixed: "right",
      width: "8%"
    }
  ];
  const propsTable: TableProps<CandidateDTO> = {
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
    dataSource: ListCandidate?.listPayload,
    onChange: handleChange,
    loading: LoadingListCandidate,
    pagination: {
      total: ListCandidate?.totalElement,
      pageSize: 6,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className={"Candidate"}>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách ứng viên </Typography.Title>
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

export const Candidate = WithErrorBoundaryCustom(_Candidate);
