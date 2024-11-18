import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  getFileImageNews,
  useApproveNewsMutation,
  useDeleteNewsMutation,
  useHideNewsMutation,
  useLazyFilterListNewsQuery
} from "@API/services/NewsApis.service";
import React, { useEffect, useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import {
  Avatar,
  Button,
  Card,
  Col,
  DatePicker,
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
  Tag,
  Typography
} from "antd";
import { NewsDTO } from "@models/newsDTO";
import { DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined } from "@ant-design/icons";
import { HandleError, ModalContent } from "@admin/components";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { NewAndUpdateNews } from "@admin/features/news";
import { useGetListCategoryNewsQuery } from "@API/services/CategoryNewsApis.service";
import { useGetListUserQuery } from "@API/services/UserApis.service";

function _News() {
  const [getListNews, { data: ListNews, isLoading: LoadingListNews }] = useLazyFilterListNewsQuery();
  const { data: ListCategoryNews, isLoading: LoadingListCategoryNews } = useGetListCategoryNewsQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListUser } = useGetListUserQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [deleteListNews, { isLoading: isLoadingDeleteListNews }] = useDeleteNewsMutation();
  const [HideNews, { isLoading: isLoadingUpdateStatusNews }] = useHideNewsMutation();
  const [ApproveNews, { isLoading: isLoadingApproveNews }] = useApproveNewsMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  const [formFilter] = Form.useForm();
  useEffect(() => {
    getListNews({
      pageSize: 0,
      pageNumber: 0
    });
  }, [getListNews]);
  const handleFilter = async (values: any) => {
    if (!values.title) delete values.title;
    if (values.createdDateDisplay) values.createdDateDisplay = dayjs(values.createdDateDisplay).format("DD/MM/YYYY");
    await getListNews({
      ...values,
      pageSize: 0,
      pageNumber: 0
    });
  };

  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<NewsDTO>["onChange"] = (pagination, filters) => {
    setFilteredInfo(filters);
  };
  const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
    setSelectedRowKeys(newSelectedRowKeys);
  };
  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange
  };
  const menuAction = (record: NewsDTO) => {
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
  const columns: ColumnsType<NewsDTO> = [
    {
      title: "Thumb",
      render: (text, record) => {
        const resource = getFileImageNews(record.id + "." + record.extensionFile);
        return <Avatar src={resource} shape={"square"} size={80} alt={"thumb"} />;
      },
      width: 100
    },
    {
      title: "Tiêu đề",
      dataIndex: "title",
      key: "title"
    },
    {
      title: "Mô tả",
      dataIndex: "description",
      key: "description"
    },
    {
      title: "Danh mục",
      dataIndex: "idCategoryNews",
      key: "idCategoryNews",
      render: (text) => ListCategoryNews?.listPayload?.find((item) => item.id === text)?.nameCategory
    },
    {
      title: "Ngày hiển thị",
      dataIndex: "createdDateDisplay",
      key: "createdDateDisplay",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
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
          loading={isLoadingUpdateStatusNews}
          onChange={async (value) => {
            try {
              await HideNews({
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
      title: "Duyệt bài",
      dataIndex: "isApproved",
      key: "isApproved",
      render: (text, record) => (
        <Select
          showSearch
          style={{ width: 120 }}
          bordered={true}
          defaultValue={text}
          loading={isLoadingApproveNews}
          onChange={async (value) => {
            try {
              await ApproveNews({
                isApprove: value,
                listId: [record.id]
              });
            } catch (e: any) {
              await HandleError(e);
            }
          }}
          options={[
            { label: <Tag color="green-inverse">Đã duyệt</Tag>, value: true },
            { label: <Tag color="red-inverse">Chưa duyệt</Tag>, value: false }
          ]}
        />
      )
    },
    {
      title: "Người đăng tin",
      dataIndex: "userCreated",
      key: "userCreated",
      render: (text) => {
        const user = ListUser?.listPayload?.find((item) => item.id === text);
        return `${user?.fullname} - ${user?.email}`;
      }
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListNews?.listPayload?.map((item) => ({
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
  const propsTable: TableProps<NewsDTO> = {
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
    dataSource: ListNews?.listPayload,
    onChange: handleChange,
    loading: LoadingListNews,
    pagination: {
      total: ListNews?.totalElement,
      pageSize: 6,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className={"News"}>
      <ModalContent
        visible={isOpenModal}
        setVisible={() => setIsOpenModal(false)}
        title={id ? "Chỉnh sửa tin tức tuyển dụng" : "Thêm mới tin tức tuyển dụng"}
        width={"90%"}
        afterClose={() => setId(undefined)}
      >
        <NewAndUpdateNews id={id} setVisible={setIsOpenModal} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách tin tức tuyển dụng </Typography.Title>
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
                  const result = await deleteListNews({
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
                loading={isLoadingDeleteListNews}
                disabled={!(selectedRowKeys.length > 0)}
                icon={<DeleteOutlined />}
              >
                Xóa {selectedRowKeys.length} mục
              </Button>
            </Popconfirm>
          </Space>
          <Card bordered={false} className="criclebox">
            <Form layout={"vertical"} form={formFilter} onFinish={handleFilter}>
              <Space wrap>
                <Form.Item label="Tiêu đề" name={"title"}>
                  <Input allowClear />
                </Form.Item>
                <Form.Item label="Danh mục" name={"idCategoryNews"}>
                  <Select
                    allowClear
                    loading={LoadingListCategoryNews}
                    showSearch
                    optionFilterProp={"label"}
                    options={ListCategoryNews?.listPayload?.map((x) => {
                      return { label: x.nameCategory, value: x.id };
                    })}
                    placeholder={"Chọn danh mục"}
                  />
                </Form.Item>
                <Form.Item label="Ngày hiển thị" name={"createdDateDisplay"}>
                  <DatePicker allowClear format={"DD/MM/YYYY"} />
                </Form.Item>
                <Form.Item label="Trạng thái" name={"isHide"}>
                  <Select
                    allowClear
                    options={[
                      { label: <Tag color="blue-inverse">Đã hiện</Tag>, value: false },
                      { label: <Tag color="red-inverse">Đã ẩn</Tag>, value: true }
                    ]}
                    placeholder={"Chọn trạng thái"}
                  />
                </Form.Item>
                <Form.Item label="Duyệt bài" name={"isApproved"}>
                  <Select
                    allowClear
                    options={[
                      { label: <Tag color="green-inverse">Đã duyệt</Tag>, value: true },
                      { label: <Tag color="red-inverse">Chưa duyệt</Tag>, value: false }
                    ]}
                    placeholder={"Chọn trạng thái"}
                  />
                </Form.Item>

                <Form.Item label={"" + "Hiệu chỉnh"}>
                  <Button type="primary" htmlType="submit">
                    Tìm kiếm
                  </Button>
                </Form.Item>
              </Space>
            </Form>
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const News = WithErrorBoundaryCustom(_News);
