import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useState } from "react";
import {
  useDeleteInternRequestMutation,
  useGetListInternRequestByRoleQuery
} from "@API/services/InternRequestApis.service";
import { Button, Card, Col, Divider, Dropdown, Menu, Modal, Row, Space, TableProps, Tag, Typography } from "antd";
import { DeleteOutlined, EditOutlined, InfoCircleOutlined, SettingOutlined } from "@ant-design/icons";
import { HandleActionWorkFlow, HandleError, ModalContent, WorkFlowHistory } from "@admin/components";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { InternRequestDTO } from "@models/internRequestDTO";
import { NewAndUpdateInternRequest, ViewDetailInternRequest } from "@admin/features/internRequest";

function _InternRequest() {
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const {
    data: ListInternRequest,
    isLoading: LoadingListInternRequest,
    refetch
  } = useGetListInternRequestByRoleQuery({
    pageSize: pagination.pageSize,
    pageNumber: pagination.pageNumber
  });
  const templateId = ListInternRequest?.listPayload?.at(0)?.workflowInstances?.at(0)?.templateId;

  const [deleteListInternRequest, { isLoading: isLoadingDeleteListInternRequest }] = useDeleteInternRequestMutation();
  const [modal, contextHolder] = Modal.useModal();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  const handleDeleteInternRequest = async (id: string) => {
    modal.confirm({
      title: "Xác nhận xóa yêu cầu",
      icon: <DeleteOutlined />,
      content: "Bạn có chắc chắn muốn xóa yêu cầu này?",
      okText: "Xác nhận",
      cancelText: "Hủy",
      okButtonProps: {
        loading: isLoadingDeleteListInternRequest
      },
      onOk: async () => {
        try {
          await deleteListInternRequest({
            listId: [id]
          }).unwrap();
          Modal.destroyAll();
        } catch (e: any) {
          await HandleError(e);
        }
      },
      onCancel: () => {
        Modal.destroyAll();
      }
    });
  };
  const handleViewDetail = (id: string) => {
    modal.info({
      title: "Chi tiết yêu cầu",
      icon: <InfoCircleOutlined />,
      width: 900,
      content: <ViewDetailInternRequest idInternRequest={id} />,
      footer: null,
      closable: true
    });
  };

  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<InternRequestDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setPagination({ pageNumber: pagination.current || 1, pageSize: pagination.pageSize || 10 });
    setFilteredInfo(filters);
  };
  const menuAction = (record: InternRequestDTO) => {
    return (
      <Menu hidden={record?.workflowInstances?.at(0)?.isTerminated || record?.workflowInstances?.at(0)?.isCompleted}>
        <Menu.Item
          key="1"
          icon={<EditOutlined />}
          onClick={() => {
            setId(record.id);
            setIsOpenModal(true);
          }}
        >
          <Button type="default" size={"small"}>
            Chỉnh sửa
          </Button>
        </Menu.Item>
        <Menu.Item
          key="4"
          icon={<DeleteOutlined />}
          onClick={async () => {
            await handleDeleteInternRequest(record.id);
          }}
        >
          <Button type="link" size={"small"} danger>
            Xóa
          </Button>
        </Menu.Item>
      </Menu>
    );
  };
  const columns: ColumnsType<InternRequestDTO> = [
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
      filters: ListInternRequest?.listPayload?.map((item) => ({
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
      title: "Ký duyệt",
      width: 130,
      render: (_, record) => <HandleActionWorkFlow record={record} templateId={templateId} refetchListMain={refetch} />
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
      render: (_, record) => (
        <Dropdown dropdownRender={() => menuAction(record)} trigger={["click"]} placement={"bottomCenter"}>
          <Button type={"text"} size={"small"} hidden={(record?.workflowInstances?.at(0)?.currentStep || 0) !== 0}>
            <SettingOutlined style={{ fontSize: 20 }} />
          </Button>
        </Dropdown>
      ),

      width: "100px"
    }
  ];
  const propsTable: TableProps<InternRequestDTO> = {
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
    dataSource: ListInternRequest?.listPayload,
    onChange: handleChange,
    loading: LoadingListInternRequest,
    pagination: {
      total: ListInternRequest?.totalElement,
      pageSize: 10,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className={"InternRequest"}>
      {contextHolder}
      <ModalContent
        visible={isOpenModal}
        setVisible={setIsOpenModal}
        title={id ? "Chỉnh sửa yêu cầu thử việc" : "Thêm mới yêu cầu thử việc"}
        width={"900px"}
      >
        <NewAndUpdateInternRequest id={id} AfterSave={() => setIsOpenModal(false)} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách yêu cầu thử việc </Typography.Title>
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

export const InternRequest = WithErrorBoundaryCustom(_InternRequest);