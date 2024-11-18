import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useDeleteCategoryPositionMutation,
  useGetListCategoryPositionQuery,
  useHideCategoryPositionMutation
} from "@API/services/CategoryPositionApis.service";
import React, { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import {
  Button,
  Card,
  Col,
  Divider,
  Dropdown,
  Menu,
  Popconfirm,
  Row,
  Select,
  Space,
  TableProps,
  Tag,
  Typography
} from "antd";
import { CategoryPositionDTO } from "@models/categoryPositionDTO";
import { DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined } from "@ant-design/icons";
import { HandleError, ModalContent } from "@admin/components";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { NewAndUpdateCategoryPosition } from "@admin/features/CategoryPosition";

function _CategoryPosition() {
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const { data: ListCategoryPosition, isLoading: LoadingListCategoryPosition } = useGetListCategoryPositionQuery({
    pageSize: pagination.pageSize,
    pageNumber: pagination.pageNumber
  });
  const [deleteListCategoryPosition, { isLoading: isLoadingDeleteListCategoryPosition }] =
    useDeleteCategoryPositionMutation();
  const [HideCategoryPosition, { isLoading: isLoadingUpdateStatusCategoryPosition }] =
    useHideCategoryPositionMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<CategoryPositionDTO>["onChange"] = (pagination, filters) => {
    setPagination({ pageNumber: pagination.current || 1, pageSize: pagination.pageSize || 10 });
    setFilteredInfo(filters);
  };
  const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
    setSelectedRowKeys(newSelectedRowKeys);
  };
  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange
  };
  const menuAction = (record: CategoryPositionDTO) => {
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
  const columns: ColumnsType<CategoryPositionDTO> = [
    {
      title: "Tên",
      dataIndex: "positionName",
      key: "positionName"
    },
    {
      title: "Mô tả",
      dataIndex: "description",
      key: "description"
    },
    {
      title: "Trạng thái",
      dataIndex: "isActive",
      key: "isActive",
      render: (text, record) => (
        <Select
          showSearch
          style={{ width: 200 }}
          bordered={false}
          defaultValue={text}
          loading={isLoadingUpdateStatusCategoryPosition}
          onChange={async (value) => {
            try {
              await HideCategoryPosition({
                isHide: value,
                listId: [record.id]
              });
            } catch (e: any) {
              await HandleError(e);
            }
          }}
          options={[
            { label: <Tag color="blue">Đã hiện</Tag>, value: true },
            { label: <Tag color="error">Đã ẩn</Tag>, value: false }
          ]}
        />
      )
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListCategoryPosition?.listPayload?.map((item) => ({
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
  const propsTable: TableProps<CategoryPositionDTO> = {
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
    dataSource: ListCategoryPosition?.listPayload,
    onChange: handleChange,
    loading: LoadingListCategoryPosition,
    pagination: {
      total: ListCategoryPosition?.totalElement,
      pageSize: 6,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className={"CategoryPosition"}>
      <ModalContent
        visible={isOpenModal}
        setVisible={() => setIsOpenModal(false)}
        title={id ? "Chỉnh sửa vị trí chức vụ" : "Thêm mới vị trí chức vụ"}
        width={"800px"}
        afterClose={() => setId(undefined)}
      >
        <NewAndUpdateCategoryPosition id={id} setVisible={setIsOpenModal} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách vị trí chức vụ </Typography.Title>
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
                  const result = await deleteListCategoryPosition({
                    listId: selectedRowKeys as string[]
                  }).unwrap();
                  if (result.success) return setSelectedRowKeys([]);
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                danger
                type="primary"
                loading={isLoadingDeleteListCategoryPosition}
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

export const CategoryPosition = WithErrorBoundaryCustom(_CategoryPosition);
