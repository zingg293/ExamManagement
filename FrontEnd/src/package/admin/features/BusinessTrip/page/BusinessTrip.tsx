import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Fragment, useState } from "react";
import {
  useDeleteBusinessTripMutation,
  useGetListBusinessTripByRoleQuery
} from "@API/services/BusinessTripApis.service";
import { Button, Card, Col, Divider, Dropdown, Menu, Modal, Row, Space, TableProps, Tag, Typography } from "antd";
import {
  ControlOutlined,
  DeleteOutlined,
  EditOutlined,
  InfoCircleOutlined,
  SettingOutlined,
  TeamOutlined
} from "@ant-design/icons";
import { HandleActionWorkFlow, HandleError, ModalContent, WorkFlowHistory } from "@admin/components";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { BusinessTripDTO } from "@models/businessTripDTO";
import {
  NewAndUpdateBusinessTrip,
  NewAndUpdateBusinessTripEmployee,
  ViewDetailBusinessTrip
} from "@admin/features/BusinessTrip";

function _BusinessTrip() {
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const {
    data: ListBusinessTrip,
    isLoading: LoadingListBusinessTrip,
    refetch
  } = useGetListBusinessTripByRoleQuery({
    pageSize: pagination.pageSize,
    pageNumber: pagination.pageNumber
  });
  const templateId = ListBusinessTrip?.listPayload?.at(0)?.workflowInstances?.at(0)?.templateId;

  const [deleteListBusinessTrip, { isLoading: isLoadingDeleteListBusinessTrip }] = useDeleteBusinessTripMutation();
  const [modal, contextHolder] = Modal.useModal();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  const handleDeleteBusinessTrip = async (id: string) => {
    modal.confirm({
      title: "Xác nhận xóa yêu cầu",
      icon: <DeleteOutlined />,
      content: "Bạn có chắc chắn muốn xóa yêu cầu này?",
      okText: "Xác nhận",
      cancelText: "Hủy",
      okButtonProps: {
        loading: isLoadingDeleteListBusinessTrip
      },
      onOk: async () => {
        try {
          await deleteListBusinessTrip({
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
      content: <ViewDetailBusinessTrip idBusinessTrip={id} />,
      footer: null,
      closable: true
    });
  };
  const handleManageEmployee = (idBusinessTrip: string) => {
    modal.info({
      title: "Danh sách nhân viên tham gia công tác",
      content: <NewAndUpdateBusinessTripEmployee idBusinessTrip={idBusinessTrip} />,
      width: 1200,
      footer: null,
      closable: true,
      icon: <TeamOutlined />
    });
  };
  const handleActionBusinessTrip = (record: BusinessTripDTO) => {
    modal.confirm({
      title: `Phiếu ${record?.workflowInstances?.at(0)?.workflowCode}`,
      icon: <SettingOutlined />,
      content: (
        <Fragment>
          <p>Chỉnh sửa, thêm danh sách nhân viên tham gia công tác</p>
          <Row>
            <Col span={24}>
              <Space
                direction={"vertical"}
                style={{
                  width: "100%"
                }}
              >
                <Button type="primary" size={"middle"} block onClick={() => handleManageEmployee(record.id)}>
                  Danh sách nhân viên tham gia công tác
                </Button>
              </Space>
            </Col>
          </Row>
        </Fragment>
      ),
      footer: null,
      closable: true,
      width: 600
    });
  };

  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<BusinessTripDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setPagination({ pageNumber: pagination.current || 1, pageSize: pagination.pageSize || 10 });
    setFilteredInfo(filters);
  };
  const menuAction = (record: BusinessTripDTO) => {
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
            await handleDeleteBusinessTrip(record.id);
          }}
        >
          <Button type="link" size={"small"} danger>
            Xóa
          </Button>
        </Menu.Item>
      </Menu>
    );
  };
  const columns: ColumnsType<BusinessTripDTO> = [
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
      filters: ListBusinessTrip?.listPayload?.map((item) => ({
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
        <Space.Compact
          style={{
            background: "transparent"
          }}
        >
          <Dropdown dropdownRender={() => menuAction(record)} trigger={["click"]} placement={"bottomCenter"}>
            <Button type={"text"} size={"small"} hidden={(record?.workflowInstances?.at(0)?.currentStep || 0) !== 0}>
              <SettingOutlined style={{ fontSize: 20 }} />
            </Button>
          </Dropdown>
          <Button
            type={"text"}
            size={"small"}
            hidden={record?.workflowInstances?.at(0)?.isCompleted || record?.workflowInstances?.at(0)?.isTerminated}
          >
            <ControlOutlined style={{ fontSize: 20 }} onClick={() => handleActionBusinessTrip(record)} />
          </Button>
        </Space.Compact>
      ),

      width: "140px"
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
    onChange: handleChange,
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
    <div className={"BusinessTrip"}>
      {contextHolder}
      <ModalContent
        visible={isOpenModal}
        setVisible={setIsOpenModal}
        title={id ? "Chỉnh sửa đăng ký công tác" : "Thêm mới đăng ký công tác"}
        width={"900px"}
      >
        <NewAndUpdateBusinessTrip id={id} AfterSave={() => setIsOpenModal(false)} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách đăng ký công tác </Typography.Title>
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

export const BusinessTrip = WithErrorBoundaryCustom(_BusinessTrip);
