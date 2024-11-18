import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useDeleteTicketLaborEquipmentMutation,
  useGetListTicketLaborEquipmentByRoleQuery
} from "@API/services/TicketLaborEquipmentApis.service";
import { Fragment, useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { Button, Card, Col, Divider, Dropdown, Menu, Modal, Row, Space, TableProps, Tag, Typography } from "antd";
import { TicketLaborEquipmentDTO } from "@models/ticketLaborEquipmentDTO";
import {
  CheckCircleOutlined,
  ControlOutlined,
  DeleteOutlined,
  EditOutlined,
  FileProtectOutlined,
  InfoCircleOutlined,
  SettingOutlined
} from "@ant-design/icons";
import dayjs from "dayjs";
import { HandleActionWorkFlow, HandleError, ModalContent, WorkFlowHistory } from "@admin/components";
import {
  ConfirmStatusOfTicketLaborEquipmentUnit,
  NewAndUpdateTicketLaborEquipment,
  TicketLaborEquipmentDetail,
  TicketLaborEquipmentDetailFixAndOther
} from "@admin/features/TicketLaborEquipment";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import {
  listColorForTypeTicketLaborEquipment,
  ViewTicketLaborEquipment
} from "@admin/features/TicketLaborEquipment/components/ViewTicketLaborEquipment";
import { useGetUserQuery } from "@API/services/UserApis.service";

function _TicketLaborEquipment() {
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const {
    data: ListTicketLaborEquipment,
    isLoading: LoadingListTicketLaborEquipment,
    refetch
  } = useGetListTicketLaborEquipmentByRoleQuery({
    pageSize: pagination.pageSize,
    pageNumber: pagination.pageNumber
  });
  const templateId = ListTicketLaborEquipment?.listPayload?.at(0)?.workflowInstances?.at(0)?.templateId;

  const [deleteListTicketLaborEquipment, { isLoading: isLoadingDeleteListTicketLaborEquipment }] =
    useDeleteTicketLaborEquipmentMutation();
  const [modal, contextHolder] = Modal.useModal();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  const { data: User } = useGetUserQuery({ fetch: true });
  const handleDeleteTicketLaborEquipment = async (id: string) => {
    modal.confirm({
      title: "Xác nhận xóa yêu cầu",
      icon: <DeleteOutlined />,
      content: "Bạn có chắc chắn muốn xóa yêu cầu này?",
      okText: "Xác nhận",
      cancelText: "Hủy",
      okButtonProps: {
        loading: isLoadingDeleteListTicketLaborEquipment
      },
      onOk: async () => {
        try {
          await deleteListTicketLaborEquipment({
            idTicketLaborEquipment: [id]
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
  const handleTicketLaborEquipmentDetail = (
    idTicketLaborEquipment: string,
    idUnit: string | undefined = undefined,
    type: number,
    idEmployee: string | undefined = undefined
  ) => {
    modal.confirm({
      title: "Chi tiết phiếu yêu cầu thiết bi lao động",
      width: 1400,
      icon: <CheckCircleOutlined />,
      content:
        type !== 0 ? (
          <TicketLaborEquipmentDetailFixAndOther
            idUnit={idUnit}
            setVisible={() => Modal.destroyAll()}
            idTicketLaborEquipment={idTicketLaborEquipment}
            idEmployee={idEmployee}
            type={type}
          />
        ) : (
          <TicketLaborEquipmentDetail setVisible={() => Modal.destroyAll()} id={idTicketLaborEquipment} />
        ),
      footer: null,
      closable: true
    });
  };

  const handleAction = (record: TicketLaborEquipmentDTO) => {
    modal.confirm({
      title: `Phiếu ${record?.workflowInstances?.at(0)?.workflowCode}`,
      icon: <SettingOutlined />,
      content: (
        <Fragment>
          <p>Bạn muốn thực hiện thao tác nào?</p>
          <Row>
            <Col span={24}>
              <Space
                direction={"vertical"}
                style={{
                  width: "100%"
                }}
              >
                <Button
                  type="primary"
                  size={"middle"}
                  block
                  onClick={() => handleTicketLaborEquipmentDetail(record.id, record.idUnit, record.type)}
                >
                  Danh sách thiết bi lao động
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

  const handleConfirmStatusOfTicketLaborEquipmentUnit = (record: TicketLaborEquipmentDTO) => {
    modal.info({
      title: `Xác nhận phiếu ${record?.workflowInstances?.at(0)?.workflowCode}`,
      icon: <FileProtectOutlined />,
      content: <ConfirmStatusOfTicketLaborEquipmentUnit idTicketLaborEquipment={record.id} refetchListMain={refetch} />,
      footer: null,
      closable: true,
      width: 1400
    });
  };

  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<TicketLaborEquipmentDTO>["onChange"] = (pagination, filters) => {
    setPagination({ pageNumber: pagination.current || 1, pageSize: pagination.pageSize || 10 });
    setFilteredInfo(filters);
  };
  const menuAction = (record: TicketLaborEquipmentDTO) => {
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
            await handleDeleteTicketLaborEquipment(record.id);
          }}
        >
          <Button type="link" size={"small"} danger>
            Xóa
          </Button>
        </Menu.Item>
      </Menu>
    );
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
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListTicketLaborEquipment?.listPayload?.map((item) => ({
        text: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm"),
        value: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm")
      })),
      filteredValue: filteredInfo?.createdDate || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.createdDate.toString().startsWith(value),
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Loại",
      render: (_, record) => listColorForTypeTicketLaborEquipment(record?.type),
      width: 120
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
      title: "Ký duyệt",
      width: 150,
      render: (_, record) => {
        const workflowInstances = record?.workflowInstances?.at(0);
        const isTicketLaborEquipmentDetailAvailable = record?.ticketLaborEquipmentDetail?.length > 0;
        const condition =
          workflowInstances?.createdBy === User?.payload?.data?.id &&
          workflowInstances?.isApproved &&
          workflowInstances?.currentStep === record.countWorkFlowStep &&
          !workflowInstances?.isCompleted &&
          record.type === 1;
        return !isTicketLaborEquipmentDetailAvailable ? (
          <Button type={"dashed"} icon={<ControlOutlined />} onClick={() => handleAction(record)}>
            Thêm chi tiết
          </Button>
        ) : condition ? (
          <Button
            type={"default"}
            danger
            icon={<FileProtectOutlined />}
            onClick={() => handleConfirmStatusOfTicketLaborEquipmentUnit(record)}
          >
            Lưu
          </Button>
        ) : (
          <HandleActionWorkFlow record={record} templateId={templateId} refetchListMain={refetch} />
        );
      }
    },
    {
      title: "Lịch sử",
      width: 110,
      render: (_, record) => <WorkFlowHistory workFlowInstance={record?.workflowInstances?.at(0)} />
    },
    {
      title: "Hiệu chỉnh",
      dataIndex: "Action",
      key: "Action",
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
            <ControlOutlined style={{ fontSize: 20 }} onClick={() => handleAction(record)} />
          </Button>
        </Space.Compact>
      ),
      width: "140px"
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
    dataSource: ListTicketLaborEquipment?.listPayload,
    onChange: handleChange,
    loading: LoadingListTicketLaborEquipment,
    pagination: {
      total: ListTicketLaborEquipment?.totalElement,
      pageSize: 10,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className={"TicketLaborEquipment"}>
      {contextHolder}
      <ModalContent
        visible={isOpenModal}
        setVisible={setIsOpenModal}
        title={"Chỉnh sửa phiếu yêu cầu thiết bi lao động"}
        width={"700px"}
      >
        <NewAndUpdateTicketLaborEquipment setVisible={setIsOpenModal} id={id} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách phiếu yêu cầu thiết bị lao động </Typography.Title>
          <Divider />
          <Space
            style={{
              marginBottom: 16
            }}
          ></Space>
          <Card bordered={false} className="criclebox">
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const TicketLaborEquipment = WithErrorBoundaryCustom(_TicketLaborEquipment);
