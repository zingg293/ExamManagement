import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  getFileImageCategoryNews,
  useDeleteCategoryNewsMutation,
  useGetListCategoryNewsQuery,
  useHideCategoryNewsMutation,
  useShowCategoryNewsMutation
} from "@API/services/CategoryNewsApis.service";
import React, { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import {
  Avatar,
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
import { CategoryNewsDTO } from "@models/categoryNewsDTO";
import { DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined } from "@ant-design/icons";
import { HandleError, ModalContent } from "@admin/components";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { NewAndUpdateCategoryNews } from "@admin/features/categoryNews";

function _CategoryNews() {
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const { data: ListCategoryNews, isLoading: LoadingListCategoryNews } = useGetListCategoryNewsQuery({
    pageSize: pagination.pageSize,
    pageNumber: pagination.pageNumber
  });
  const [deleteListCategoryNews, { isLoading: isLoadingDeleteListCategoryNews }] = useDeleteCategoryNewsMutation();
  const [HideCategoryNews, { isLoading: isLoadingUpdateStatusCategoryNews }] = useHideCategoryNewsMutation();
  const [ShowCategoryNews, { isLoading: isLoadingUpdateStatusCategoryNewsShow }] = useShowCategoryNewsMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<CategoryNewsDTO>["onChange"] = (pagination, filters) => {
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
  const menuAction = (record: CategoryNewsDTO) => {
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
  const columns: ColumnsType<CategoryNewsDTO> = [
    {
      title: "Thumb",
      render: (text, record) => {
        const resource = getFileImageCategoryNews(record.id + "." + record.avatar?.split(".").pop());
        return <Avatar src={resource} shape={"square"} size={80} alt={"thumb"} />;
      },
      width: 100
    },
    {
      title: "Tên",
      dataIndex: "nameCategory",
      key: "nameCategory"
    },
    {
      title: "Nhóm",
      dataIndex: "categoryGroup",
      key: "categoryGroup",
      width: 100
    },
    {
      title: "Sắp xếp",
      dataIndex: "sort",
      key: "sort",
      width: 100
    },
    {
      title: "Danh mục cha",
      dataIndex: "parentId",
      key: "parentId",
      render: (text) => ListCategoryNews?.listPayload?.find((x) => x.id === text)?.nameCategory
    },
    {
      title: "Trạng thái",
      dataIndex: "isHide",
      key: "isHide",
      render: (text, record) => (
        <Select
          showSearch
          style={{ width: 110 }}
          bordered={true}
          defaultValue={text}
          loading={isLoadingUpdateStatusCategoryNews}
          onChange={async (value) => {
            try {
              await HideCategoryNews({
                isHide: value,
                listId: [record.id]
              });
            } catch (e: any) {
              await HandleError(e);
            }
          }}
          options={[
            { label: <Tag color="blue-inverse">Đã hiện</Tag>, value: false },
            { label: <Tag color="red-inverse">Đã ẩn</Tag>, value: true }
          ]}
        />
      )
    },
    {
      title: "Hiện danh mục con",
      dataIndex: "showChild",
      key: "showChild",
      render: (text, record) => (
        <Select
          showSearch
          style={{ width: 110 }}
          bordered={true}
          defaultValue={text}
          loading={isLoadingUpdateStatusCategoryNewsShow}
          onChange={async (value) => {
            try {
              await ShowCategoryNews({
                listId: [record.id],
                isShow: value
              });
            } catch (e: any) {
              await HandleError(e);
            }
          }}
          options={[
            { label: <Tag color="green-inverse">Đã hiện</Tag>, value: true },
            { label: <Tag color="red-inverse">Đã ẩn</Tag>, value: false }
          ]}
        />
      )
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListCategoryNews?.listPayload?.map((item) => ({
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
  const propsTable: TableProps<CategoryNewsDTO> = {
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
    dataSource: ListCategoryNews?.listPayload,
    onChange: handleChange,
    loading: LoadingListCategoryNews,
    pagination: {
      total: ListCategoryNews?.totalElement,
      pageSize: 6,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className={"CategoryNews"}>
      <ModalContent
        visible={isOpenModal}
        setVisible={() => setIsOpenModal(false)}
        title={id ? "Chỉnh sửa danh mục tin tức tuyển dụng" : "Thêm mới danh mục tin tức tuyển dụng"}
        width={"800px"}
        afterClose={() => setId(undefined)}
      >
        <NewAndUpdateCategoryNews id={id} setVisible={setIsOpenModal} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh mục tin tức tuyển dụng </Typography.Title>
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
                  const result = await deleteListCategoryNews({
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
                loading={isLoadingDeleteListCategoryNews}
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

export const CategoryNews = WithErrorBoundaryCustom(_CategoryNews);
