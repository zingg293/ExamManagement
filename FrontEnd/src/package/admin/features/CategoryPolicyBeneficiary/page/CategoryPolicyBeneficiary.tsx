import React, {useState} from 'react';
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useDeleteCategoryPolicyBeneficiaryMutation,
  useGetListCategoryPolicyBeneficiaryQuery
} from "@API/services/CategoryPolicyBeneficiary.service";
import {CategoryPolicyBeneficiaryDTO} from "@models/CategoryPolicyBeneficiaryDTO";
import {Button, Card, Col, Divider, Dropdown, Menu, Popconfirm, Row, Space, TableProps, Typography} from "antd";
import {DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined} from "@ant-design/icons";
import {ColumnsType} from "antd/lib/table/interface";
import dayjs from "dayjs";
import {HandleError, ModalContent} from "@admin/components";
import { NewAndUpdateCategoryPolicyBeneficiary } from "@admin/features/CategoryPolicyBeneficiary";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
const _CategoryPolicyBeneficiary = () => {
  const { data: ListCategoryPolicyBeneficiary,
    isLoading: LoadingCategoryPolicyBeneficiary } = useGetListCategoryPolicyBeneficiaryQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [deleteListCategoryPolicyBeneficiary, { isLoading: isLoadingDeleteListCategoryPolicyBeneficiary }] = useDeleteCategoryPolicyBeneficiaryMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  // const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  // const handleChange: TableProps<CategoryNationalityDTO>["onChange"] = (pagination, filters) => {
  //   // pagination = pagination || {};
  //   setFilteredInfo(filters);
  // };
  const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
    setSelectedRowKeys(newSelectedRowKeys);
  };
  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange
  };
  const menuAction = (record: CategoryPolicyBeneficiaryDTO) => {
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
  const columns: ColumnsType<CategoryPolicyBeneficiaryDTO> = [
    {
      title: "Tên",
      dataIndex: "namePolicybeneficiary",
      key: "namePolicybeneficiary"
    },
    // {
    //   title: "Số tiền",
    //   dataIndex: "amount",
    //   key: "amount",
    //   render: (text: number) => formatMoney(text)
    // },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      // filters: ListCategoryNationality?.listPayload?.map((item) => ({
      //   text: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm"),
      //   value: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm")
      // })),
      // filteredValue: filteredInfo?.createdDate || null,
      // filterSearch: true,
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
  const propsTable: TableProps<CategoryPolicyBeneficiaryDTO> = {
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
    dataSource: ListCategoryPolicyBeneficiary?.listPayload,
    // onChange: handleChange,
    loading: LoadingCategoryPolicyBeneficiary,
    pagination: {
      total: ListCategoryPolicyBeneficiary?.totalElement,
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
        title={id ? "Chỉnh sửa  " : "Thêm mới"}
        width={"600px"}
      >
        <NewAndUpdateCategoryPolicyBeneficiary setVisible={setIsOpenModal} id={id} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh mục hình thức thi </Typography.Title>
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
                  const result = await deleteListCategoryPolicyBeneficiary({ idCategoryPolicyBeneficiary: selectedRowKeys as string[] }).unwrap();
                  if (result.success) return setSelectedRowKeys([]);
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                danger
                type="primary"
                loading={isLoadingDeleteListCategoryPolicyBeneficiary}
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
export const CategoryPolicyBeneficiary = WithErrorBoundaryCustom(_CategoryPolicyBeneficiary);