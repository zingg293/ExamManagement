import {
  AuditOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined,
  DeleteOutlined,
  EditOutlined,
  SafetyCertificateOutlined,
  SettingOutlined,
  StopOutlined,
  UserAddOutlined
} from "@ant-design/icons";
import { Button, Card, Col, Divider, Dropdown, Menu, Popconfirm, Row, Space, TableProps, Tag, Typography } from "antd";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import React, { useState } from "react";
import { UserDTO } from "~/models/userDto";
import WithErrorBoundaryCustom from "~/units/errorBounDary/WithErrorBoundaryCustom";
import { NewAndUpdateUser, RoleUser } from "@admin/features/userManage";
import {
  useGetListUserQuery,
  useLockUserAccountByListMutation,
  useRemoveUserByListMutation
} from "@API/services/UserApis.service";
import dayjs from "dayjs";
import { HandleError, ModalContent } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { DrawerContent } from "@admin/components/DrawerContent/DrawerContent";

const _UserManage: React.FC = () => {
  const [isOpenModalAuthorization, setIsOpenModalAuthorization] = useState(false);
  const [isOpenModalNewAndUpdateUser, setIsOpenModalNewAndUpdateUser] = useState(false);
  const [idUserSelected, setIdUserSelected] = useState<string | undefined>(undefined);
  const [removeUserByList, { isLoading: isLoadingRemoveUser }] = useRemoveUserByListMutation();
  const [lockUserByList, { isLoading: isLoadingLockUser }] = useLockUserAccountByListMutation();
  const { data, isLoading } = useGetListUserQuery({
    pageSize: 0,
    pageNumber: 0
  });

  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<UserDTO>["onChange"] = (pagination, filters) => {
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
  const menuAction = (record: UserDTO) => {
    return (
      <Menu>
        <Menu.Item
          key="1"
          icon={<EditOutlined />}
          onClick={() => {
            setIdUserSelected(record.id);
            setIsOpenModalNewAndUpdateUser(true);
          }}
        >
          Chỉnh sửa
        </Menu.Item>
        <Menu.Item
          key="2"
          icon={<AuditOutlined />}
          onClick={() => {
            setIdUserSelected(record.id);
            setIsOpenModalAuthorization(true);
          }}
        >
          Phân quyền tài khoản
        </Menu.Item>
      </Menu>
    );
  };
  const columns: ColumnsType<UserDTO> = [
    {
      title: "Họ và tên",
      dataIndex: "fullname",
      key: "fullname",
      filters: data?.listPayload?.map((item) => ({
        text: item.fullname,
        value: item.fullname
      })),
      filteredValue: filteredInfo?.fullname || null,
      filterSearch: true,
      width: 200,
      onFilter: (value: any, record) => record.fullname.startsWith(value),
      render: (text) => (
        <Typography.Text>
          {<UserAddOutlined />} {text}
        </Typography.Text>
      )
    },
    {
      title: "Mã tài khoản",
      dataIndex: "userCode",
      key: "userCode",
      filters: data?.listPayload?.map((item) => ({
        text: item.userCode,
        value: item.userCode
      })),
      filteredValue: filteredInfo?.userCode || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.userCode.startsWith(value)
    },
    {
      title: "Email",
      dataIndex: "email",
      key: "email",
      filters: data?.listPayload?.map((item) => ({
        text: item.email,
        value: item.email
      })),
      filteredValue: filteredInfo?.email || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.email.startsWith(value),
      render: (text) => (
        <Typography.Link href={`mailto:${text}`} target={"_blank"}>
          {text}
        </Typography.Link>
      )
    },
    {
      title: "Phone",
      dataIndex: "phone",
      key: "phone",
      filters: data?.listPayload?.map((item) => ({
        text: item.phone,
        value: item.phone
      })),
      filteredValue: filteredInfo?.phone || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.phone.startsWith(value)
    },
    {
      title: "Địa chỉ",
      dataIndex: "address",
      key: "address",
      filters: data?.listPayload?.map((item) => ({
        text: item.address,
        value: item.address
      })),
      filteredValue: filteredInfo?.address || null,
      filterSearch: true,
      onFilter: (value: any, record) => record?.address?.startsWith(value)
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: data?.listPayload?.map((item) => ({
        text: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm"),
        value: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm")
      })),
      filteredValue: filteredInfo?.createdDate || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.createdDate.toString().startsWith(value),
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Trạng thái",
      dataIndex: "isLocked",
      key: "isLocked",
      render: (text) =>
        !text ? (
          <Tag color={"success"}>
            <CheckCircleOutlined /> Hoạt động
          </Tag>
        ) : (
          <Tag color={"error"}>
            <CloseCircleOutlined /> Không
          </Tag>
        )
    },
    {
      title: "Hiệu chỉnh",
      dataIndex: "Action",
      key: "Action",
      fixed: "right",
      render: (_, record) => (
        <Dropdown overlay={menuAction(record)} trigger={["click"]}>
          <SettingOutlined style={{ fontSize: 20 }} />
        </Dropdown>
      ),
      width: "8%"
    }
  ];
  const propsTable: TableProps<UserDTO> = {
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
    dataSource: data?.listPayload,
    onChange: handleChange,
    loading: isLoading,
    pagination: {
      total: data?.totalElement,
      pageSize: 10,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className="UserManage">
      <DrawerContent
        onClose={() => setIsOpenModalNewAndUpdateUser(false)}
        open={isOpenModalNewAndUpdateUser}
        title={!idUserSelected ? "Thêm tài khoản mới" : "chỉnh sửa tài khoản"}
        width={"65%"}
      >
        <NewAndUpdateUser setVisible={setIsOpenModalNewAndUpdateUser} idUser={idUserSelected} />
      </DrawerContent>
      <ModalContent
        visible={isOpenModalAuthorization}
        setVisible={setIsOpenModalAuthorization}
        title={"Phần quyền tài khoản"}
        width={"600px"}
      >
        <RoleUser setVisible={setIsOpenModalAuthorization} id={idUserSelected} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách tài khoản </Typography.Title>
          <Divider />

          <Space
            style={{
              marginBottom: 16
            }}
            wrap
          >
            <Button
              onClick={() => {
                setIsOpenModalNewAndUpdateUser(true);
                setIdUserSelected(undefined);
              }}
              icon={<UserAddOutlined />}
              type="primary"
            >
              Thêm tài khoản mới
            </Button>
            <Popconfirm
              title="Bạn có chắc chắn không ?"
              okText="Có"
              disabled={!(selectedRowKeys.length > 0)}
              cancelText="Không"
              onConfirm={async () => {
                try {
                  await lockUserByList({ listId: selectedRowKeys as string[], isLock: false });
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                type="default"
                disabled={!(selectedRowKeys.length > 0)}
                loading={isLoadingLockUser}
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
                  await lockUserByList({ listId: selectedRowKeys as string[], isLock: true });
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                danger
                type="default"
                disabled={!(selectedRowKeys.length > 0)}
                loading={isLoadingLockUser}
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
                  const result = await removeUserByList({ listId: selectedRowKeys as string[] }).unwrap();
                  if (result.success) return setSelectedRowKeys([]);
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                danger
                type="primary"
                loading={isLoadingRemoveUser}
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
export const UserManage = WithErrorBoundaryCustom(_UserManage);
