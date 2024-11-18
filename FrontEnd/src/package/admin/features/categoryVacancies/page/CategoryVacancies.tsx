import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useDeleteCategoryVacanciesMutation,
  useGetListCategoryVacanciesQuery,
  useUpdateStatusCategoryVacanciesMutation
} from "@API/services/CategoryVacanciesApis.service";
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
import { CategoryVacanciesDTO } from "@models/categoryVacanciesDTO";
import { DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import { HandleError } from "@admin/components";
import { NewAndUpdateCategoryVacancies } from "@admin/features/categoryVacancies";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { DrawerContent } from "@admin/components/DrawerContent/DrawerContent";

function _CategoryVacancies() {
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const { data: ListCategoryVacancies, isLoading: LoadingListCategoryVacancies } = useGetListCategoryVacanciesQuery({
    pageSize: pagination.pageSize,
    pageNumber: pagination.pageNumber
  });
  const [deleteListCategoryVacancies, { isLoading: isLoadingDeleteListCategoryVacancies }] =
    useDeleteCategoryVacanciesMutation();
  const [updateStatusCategoryVacancies, { isLoading: isLoadingUpdateStatusCategoryVacancies }] =
    useUpdateStatusCategoryVacanciesMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<CategoryVacanciesDTO>["onChange"] = (pagination, filters) => {
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
  const menuAction = (record: CategoryVacanciesDTO) => {
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
  const columns: ColumnsType<CategoryVacanciesDTO> = [
    {
      title: "Tên",
      dataIndex: "positionName",
      key: "positionName"
    },
    {
      title: "Năm kinh nghiệm",
      dataIndex: "numExp",
      key: "numExp"
    },
    {
      title: "Bằng cấp",
      dataIndex: "degree",
      key: "degree"
    },
    {
      title: "Trạng thái",
      dataIndex: "status",
      key: "status",
      render: (text, record) => (
        <Select
          showSearch
          style={{ width: 200 }}
          bordered={false}
          defaultValue={text}
          loading={isLoadingUpdateStatusCategoryVacancies}
          onChange={async (value) => {
            try {
              await updateStatusCategoryVacancies({ idCategoryVacancy: record.id, status: value });
            } catch (e: any) {
              await HandleError(e);
            }
          }}
          options={[
            { label: <Tag color="blue">Đang chờ phê duyệt</Tag>, value: 0 },
            { label: <Tag color="success">Đã duyệt</Tag>, value: 1 },
            { label: <Tag color="warning">Từ chối</Tag>, value: 2 },
            { label: <Tag color="error">Hủy vị trí tuyển dụng này</Tag>, value: 3 }
          ]}
        />
      )
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListCategoryVacancies?.listPayload?.map((item) => ({
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
  const propsTable: TableProps<CategoryVacanciesDTO> = {
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
    dataSource: ListCategoryVacancies?.listPayload,
    onChange: handleChange,
    loading: LoadingListCategoryVacancies,
    pagination: {
      total: ListCategoryVacancies?.totalElement,
      pageSize: 6,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className={"CategoryVacancies"}>
      <DrawerContent
        open={isOpenModal}
        onClose={() => setIsOpenModal(false)}
        title={id ? "Chỉnh sửa vị trí ứng tuyển" : "Thêm mới vị trí ứng tuyển"}
        width={"1200px"}
      >
        <NewAndUpdateCategoryVacancies setVisible={setIsOpenModal} id={id} />
      </DrawerContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách vị trí ứng tuyển </Typography.Title>
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
                  const result = await deleteListCategoryVacancies({
                    idCategoryVacancies: selectedRowKeys as string[]
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
                loading={isLoadingDeleteListCategoryVacancies}
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

export const CategoryVacancies = WithErrorBoundaryCustom(_CategoryVacancies);
