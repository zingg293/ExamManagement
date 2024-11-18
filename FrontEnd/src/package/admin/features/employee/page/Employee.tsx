// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import WithErrorBoundaryCustom from "~/units/errorBounDary/WithErrorBoundaryCustom";
import {
  App,
  Button,
  Card,
  Col,
  Divider,
  Dropdown,
  Form,
  Input,
  Menu,
  Popconfirm,
  Row,
  Select,
  Space,
  TableProps,
  Typography
} from "antd";
import React, { useState } from "react";
import { ColumnsType } from "antd/lib/table/interface";
import { EmployeeDTO } from "@models/employeeDTO";
import {
  DeleteOutlined,
  EditOutlined,
  HeartOutlined,
  PlusCircleOutlined,
  SearchOutlined,
  SettingOutlined,
  SketchOutlined,
  UserOutlined
} from "@ant-design/icons";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import {
  GetFIleEmployee,
  useDeleteEmployeeMutation,
  useLazyGetListEmployeeByConditionQuery
} from "@API/services/Employee.service";
import { HandleError, ModalContent } from "@admin/components";
import {
  DetailEmployee,
  EmployeeAllowance,
  EmployeeBenefit,
  NewAndUpdateEmployee,
  PositionEmployee
} from "@admin/features/employee";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { avatar } from "@admin/asset/icon";
import { DrawerContent } from "@admin/components/DrawerContent/DrawerContent";
import { useGetListEmployeeTypeQuery } from "@API/services/EmployeeType.service";
import { LazyLoadImage } from "react-lazy-load-image-component";

const _Employee = () => {
  const { modal } = App.useApp();
  const [GetListEmployeeByCondition, { data: ListEmployee, isLoading: LoadingListEmployee }] =
    useLazyGetListEmployeeByConditionQuery();
  const { data: ListEmployeeType, isLoading: LoadingListEmployeeType } = useGetListEmployeeTypeQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [deleteListEmployee, { isLoading: isLoadingDeleteListEmployee }] = useDeleteEmployeeMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [isOpenAllowance, setIsOpenAllowance] = useState<boolean>(false);
  const [isOpenBenefit, setIsOpenBenefit] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);

  const handleViewDetail = (id: string) => {
    return modal.info({
      title: "Thông tin nhân viên",
      width: "80%",
      content: <DetailEmployee idEmployee={id} />,
      footer: null,
      closable: true,
      icon: <UserOutlined />
    });
  };
  const handlePositionEmployee = (idEmployee: string) => {
    return modal.info({
      title: "Chức vụ của nhân viên",
      width: "80%",
      content: <PositionEmployee idEmployee={idEmployee} />,
      footer: null,
      closable: true,
      icon: <UserOutlined />
    });
  };

  const handleFilter = async (values: any) => {
    try {
      Object.keys(values).forEach((key) => {
        if (!values[key]) delete values[key];
      });
      await GetListEmployeeByCondition({
        ...values,
        pageNumber: 0,
        pageSize: 0
      }).unwrap();
    } catch (e: any) {
      await HandleError(e);
    }
  };

  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);

  const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
    setSelectedRowKeys(newSelectedRowKeys);
  };
  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange
  };
  const menuAction = (record: EmployeeDTO) => {
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
        <Menu.Item
          key="2"
          icon={<HeartOutlined />}
          onClick={() => {
            setId(record.id);
            setIsOpenAllowance(true);
          }}
        >
          Phụ cấp
        </Menu.Item>
        <Menu.Item
          key="3"
          icon={<SketchOutlined />}
          onClick={() => {
            setId(record.id);
            setIsOpenBenefit(true);
          }}
        >
          Phúc lợi
        </Menu.Item>
        <Menu.Item
          key="4"
          icon={<UserOutlined />}
          onClick={() => {
            handlePositionEmployee(record.id);
          }}
        >
          Chức vụ của nhân viên
        </Menu.Item>
      </Menu>
    );
  };

  const columns: ColumnsType<EmployeeDTO> = [
    {
      title: "Tên",
      dataIndex: "name",
      key: "name",
      render: (text, record) => (
        <Space size={3}>
          <LazyLoadImage
            alt={`avatar-${record.name}`}
            effect="blur"
            width={24}
            height={24}
            style={{ objectFit: "cover", borderRadius: "50%" }}
            placeholderSrc={GetFIleEmployee(record.id + ".jpg")}
            src={record.avatar ? GetFIleEmployee(record.id + ".jpg") : avatar}
          />{" "}
          {text}
        </Space>
      )
    },
    {
      title: "Mã",
      dataIndex: "code",
      key: "code"
    },
    {
      title: "Sđt",
      dataIndex: "phone",
      key: "phone"
    },
    {
      title: "Loại",
      dataIndex: "typeOfEmployee",
      key: "typeOfEmployee",
      render: (text) => ListEmployeeType?.listPayload?.find((item) => item.id === text)?.typeName
    },
    {
      title: "Phòng ban",
      dataIndex: "idUnit",
      key: "idUnit",
      render: (text) => ListUnit?.listPayload?.find((item) => item.id === text)?.unitName
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Chi tiết",
      render: (_, record) => (
        <Button type="link" onClick={() => handleViewDetail(record.id)}>
          Chi tiết
        </Button>
      ),
      width: 100
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
  const propsTable: TableProps<EmployeeDTO> = {
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
    dataSource: ListEmployee?.listPayload,
    loading: LoadingListEmployee,
    pagination: {
      total: ListEmployee?.totalElement,
      pageSize: 10,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className={"unit"}>
      <DrawerContent
        open={isOpenModal}
        onClose={() => setIsOpenModal(false)}
        title={id ? "Chỉnh sửa Nhân viên" : "Thêm mới Nhân viên"}
        width={"55%"}
      >
        <NewAndUpdateEmployee setVisible={setIsOpenModal} id={id} />
      </DrawerContent>
      <ModalContent
        visible={isOpenAllowance}
        setVisible={setIsOpenAllowance}
        title={"Phụ cấp nhân viên"}
        width={"600px"}
      >
        <EmployeeAllowance setVisible={setIsOpenAllowance} id={id} />
      </ModalContent>
      <ModalContent visible={isOpenBenefit} setVisible={setIsOpenBenefit} title={"Phúc lợi nhân viên"} width={"600px"}>
        <EmployeeBenefit setVisible={setIsOpenBenefit} id={id} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách nhân viên </Typography.Title>
          <Divider />

          <Card bordered={false} className="criclebox">
            <Space
              style={{
                marginBottom: 16,
                justifyContent: "space-between",
                width: "100%"
              }}
              wrap
            >
              <Form layout="vertical" onFinish={handleFilter}>
                <Space>
                  <Form.Item name="code" label="Mã">
                    <Input placeholder={"Nhập mã nhân viên"} />
                  </Form.Item>
                  <Form.Item name="phone" label="Số điện thoại">
                    <Input placeholder={"Nhập số điện thoại"} />
                  </Form.Item>
                  <Form.Item name="idUnit" label="Phòng ban">
                    <Select
                      allowClear
                      showSearch
                      options={ListUnit?.listPayload?.map((user) => {
                        return { label: user.unitName, value: user.id };
                      })}
                      loading={LoadingListUnit}
                      optionFilterProp={"label"}
                      placeholder={"Chọn phòng ban"}
                    />
                  </Form.Item>
                  <Form.Item name="typeOfEmployee" label="Loại nhân viên">
                    <Select
                      allowClear
                      showSearch
                      options={ListEmployeeType?.listPayload?.map((user) => {
                        return { label: user.typeName, value: user.id };
                      })}
                      loading={LoadingListEmployeeType}
                      optionFilterProp={"label"}
                      placeholder={"Chọn loại nhân viên"}
                    />
                  </Form.Item>
                  <Form.Item label={"Tìm kiếm"}>
                    <Button type="primary" htmlType="submit" icon={<SearchOutlined />}>
                      Tìm kiếm
                    </Button>
                  </Form.Item>
                </Space>
              </Form>
              <Space wrap>
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
                      const result = await deleteListEmployee({ idEmployee: selectedRowKeys as string[] }).unwrap();
                      if (result.success) return setSelectedRowKeys([]);
                    } catch (e: any) {
                      await HandleError(e);
                    }
                  }}
                >
                  <Button
                    danger
                    type="primary"
                    loading={isLoadingDeleteListEmployee}
                    disabled={!(selectedRowKeys.length > 0)}
                    icon={<DeleteOutlined />}
                    hidden
                  >
                    Xóa {selectedRowKeys.length} mục
                  </Button>
                </Popconfirm>
              </Space>
            </Space>
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
};
export const Employee = WithErrorBoundaryCustom(_Employee);
