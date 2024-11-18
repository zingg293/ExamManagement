import WithErrorBoundaryCustom from "~/units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Card, Col, Divider, Dropdown, Menu, Popconfirm, Row, Space, TableProps, Tag, Typography } from "antd";
import React, { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { EmployeeTypeDTO } from "@models/employeeTypeDTO";
import {
  DeleteOutlined,
  EditOutlined,
  PlusCircleOutlined,
  SafetyCertificateOutlined,
  SettingOutlined,
  StopOutlined
} from "@ant-design/icons";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import {
  useDeleteEmployeeTypeMutation,
  useGetListEmployeeTypeQuery,
  useHideEmployeeTypeMutation
} from "@API/services/EmployeeType.service";
import { HandleError, ModalContent } from "@admin/components";
import { NewAndUpdateEmployeeType } from "@admin/features/employeeType";

const _EmployeeType = () => {
  const { data: ListEmployeeType, isLoading: LoadingListEmployeeType } = useGetListEmployeeTypeQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [deleteListEmployeeType, { isLoading: isLoadingDeleteListEmployeeType }] = useDeleteEmployeeTypeMutation();
  const [hideListEmployeeType, { isLoading: isLoadingHideListEmployeeType }] = useHideEmployeeTypeMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<EmployeeTypeDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setFilteredInfo(filters);
  };
  const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
    setSelectedRowKeys(newSelectedRowKeys);
  };
  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange
  };
  const menuAction = (record: EmployeeTypeDTO) => {
    return (
      <Menu>
        <Menu.Item
          key="1"
          icon={<EditOutlined />}
          onClick={() => {
            setId(record.id);
            setIsOpenModal(true);
          }}
        >
          Chỉnh sửa
        </Menu.Item>
      </Menu>
    );
  };
  const columns: ColumnsType<EmployeeTypeDTO> = [
    {
      title: "Tên",
      dataIndex: "typeName",
      key: "typeName"
    },
    {
      title: "Mã",
      dataIndex: "typeCode",
      key: "typeCode"
    },
    {
      title: "Trạng thái",
      dataIndex: "isActive",
      key: "isActive",
      render: (text) => (!text ? <Tag color={"error"}>Khóa</Tag> : <Tag color={"success"}>Hoạt động</Tag>)
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListEmployeeType?.listPayload?.map((item) => ({
        text: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm"),
        value: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm")
      })),
      filteredValue: filteredInfo?.createdDate || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.createdDate.toString().startsWith(value),
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Hiệu chỉnh",
      dataIndex: "Action",
      key: "Action",
      fixed: "right",
      render: (_, record) => (
        <Dropdown overlay={menuAction(record)} trigger={["click"]} placement={"bottomCenter"}>
          <SettingOutlined style={{ fontSize: 20 }} />
        </Dropdown>
      ),
      width: "8%"
    }
  ];
  const propsTable: TableProps<EmployeeTypeDTO> = {
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
    rowSelection: rowSelection,
    dataSource: ListEmployeeType?.listPayload,
    onChange: handleChange,
    loading: LoadingListEmployeeType,
    pagination: {
      total: ListEmployeeType?.totalElement,
      pageSize: 10,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className={"unit"}>
      <ModalContent
        visible={isOpenModal}
        setVisible={setIsOpenModal}
        title={id ? "Chỉnh sửa loại giảng viên" : "Thêm mới loại giảng viên"}
        width={"600px"}
      >
        <NewAndUpdateEmployeeType setVisible={setIsOpenModal} id={id} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh mục loại giảng viên </Typography.Title>
          <Divider />
          <Space
            style={{
              marginBottom: 16
            }}
            wrap
          >
            <Button
              onClick={() => {
                setId(undefined);
                setIsOpenModal(true);
              }}
              icon={<PlusCircleOutlined />}
              type="primary"
            >
              Thêm mới
            </Button>
            <Popconfirm
              title="Bạn có chắc chắn không ?"
              okText="Có"
              disabled={!(selectedRowKeys.length > 0)}
              cancelText="Không"
              onConfirm={async () => {
                try {
                  await hideListEmployeeType({ listId: selectedRowKeys as string[], isHide: true });
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                type="default"
                disabled={!(selectedRowKeys.length > 0)}
                loading={isLoadingHideListEmployeeType}
                icon={<SafetyCertificateOutlined />}
              >
                Kích hoạt {selectedRowKeys.length} mục
              </Button>
            </Popconfirm>
            <Popconfirm
              title="Bạn có chắc chắn không ?"
              okText="Có"
              disabled={!(selectedRowKeys.length > 0)}
              cancelText="Không"
              onConfirm={async () => {
                try {
                  await hideListEmployeeType({ listId: selectedRowKeys as string[], isHide: false });
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                danger
                type="default"
                disabled={!(selectedRowKeys.length > 0)}
                loading={isLoadingHideListEmployeeType}
                icon={<StopOutlined />}
              >
                Khóa {selectedRowKeys.length} mục
              </Button>
            </Popconfirm>
            <Popconfirm
              title="Bạn có chắc chắn không ?"
              okText="Có"
              cancelText="Không"
              disabled={!(selectedRowKeys.length > 0)}
              onConfirm={async () => {
                try {
                  const result = await deleteListEmployeeType({ id: selectedRowKeys as string[] }).unwrap();
                  if (result.success) return setSelectedRowKeys([]);
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                danger
                type="primary"
                loading={isLoadingDeleteListEmployeeType}
                disabled={!(selectedRowKeys.length > 0)}
                icon={<DeleteOutlined />}
              >
                Xóa {selectedRowKeys.length} mục
              </Button>
            </Popconfirm>
          </Space>
          <Card bordered={false} className="criclebox">
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
};
export const EmployeeType = WithErrorBoundaryCustom(_EmployeeType);
