import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import React, { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { Button, Card, Col, Divider, Dropdown, Menu, Popconfirm, Row, Space, TableProps, Tag, Typography } from "antd";
import { DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import { HandleError, ModalContent } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { NewAndUpdateEmployeeDayOff } from "@admin/features/employeeDayOff";
import { useDeleteEmployeeDayOffMutation, useGetListEmployeeDayOffQuery } from "@API/services/EmployeeDayOff.service";
import { EmployeeDayOffDTO } from "@models/employeeDayOffDto";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";

function _EmployeeDayOff() {
  const { data: ListEmployeeDayOff, isLoading: LoadingListEmployeeDayOff } = useGetListEmployeeDayOffQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListEmployee } = useGetListEmployeeQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [deleteListEmployeeDayOff, { isLoading: isLoadingDeleteListEmployeeDayOff }] =
    useDeleteEmployeeDayOffMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<EmployeeDayOffDTO>["onChange"] = (pagination, filters) => {
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
  const menuAction = (record: EmployeeDayOffDTO) => {
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
  const columns: ColumnsType<EmployeeDayOffDTO> = [
    {
      title: "Nhận viên",
      dataIndex: "idEmployee",
      key: "idEmployee",
      render: (text) => {
        const employee = ListEmployee?.listPayload?.find((item) => item.id === text);
        return `${employee?.name} - ${employee?.code}`;
      }
    },
    {
      title: "Ngày nghỉ",
      dataIndex: "dayOff",
      key: "dayOff",
      render: (text) => dayjs(text).format("DD-MM-YYYY")
    },
    {
      title: "Loại",
      dataIndex: "typeOfDayOff",
      key: "typeOfDayOff",
      render: (text) => {
        switch (text) {
          case 1:
            return <Tag color="green">Nghỉ buổi sáng</Tag>;
          case 2:
            return <Tag color="yellow">Nghỉ buổi chiều</Tag>;
          case 3:
            return <Tag color="blue">Nghỉ cả ngày</Tag>;
          case 4:
            return <Tag color="green-inverse">Tăng ca sáng</Tag>;
          case 5:
            return <Tag color="yellow-inverse">Tăng ca chiều</Tag>;
          case 6:
            return <Tag color="blue-inverse">Tăng ca tối</Tag>;
          default:
            return "";
        }
      }
    },
    {
      title: "Tính phép",
      dataIndex: "onLeave",
      key: "onLeave",
      render: (text) => {
        switch (text) {
          case 0:
            return <Tag color="blue">Nghỉ có phép</Tag>;
          case 1:
            return <Tag color="red">Nghỉ không phép</Tag>;
          case 2:
            return <Tag color="green">Tăng ca</Tag>;
          default:
            return "";
        }
      }
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListEmployeeDayOff?.listPayload?.map((item) => ({
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
  const propsTable: TableProps<EmployeeDayOffDTO> = {
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
    dataSource: ListEmployeeDayOff?.listPayload,
    onChange: handleChange,
    loading: LoadingListEmployeeDayOff,
    pagination: {
      total: ListEmployeeDayOff?.totalElement,
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
        title={id ? "Chỉnh sửa ngày nghỉ nhân viên" : "Thêm mới ngày nghỉ nhân viên"}
        width={"700px"}
      >
        <NewAndUpdateEmployeeDayOff setVisible={setIsOpenModal} id={id} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách ngày nghỉ nhân viên </Typography.Title>
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
              cancelText="Không"
              disabled={!(selectedRowKeys.length > 0)}
              onConfirm={async () => {
                try {
                  const result = await deleteListEmployeeDayOff({ id: selectedRowKeys as string[] }).unwrap();
                  if (result.success) return setSelectedRowKeys([]);
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                danger
                type="primary"
                loading={isLoadingDeleteListEmployeeDayOff}
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
}

export const EmployeeDayOff = WithErrorBoundaryCustom(_EmployeeDayOff);
