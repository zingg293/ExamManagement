import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import React, { useState } from "react";
import { ColumnsType } from "antd/lib/table/interface";
import { Button, Card, Col, Divider, Dropdown, Menu, Popconfirm, Row, Space, TableProps, Typography } from "antd";
import { DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import { HandleError, ModalContent } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import {NewAndUpdateCategorySalaryLevel } from "@admin/features/CategorySalaryLevel";
import {CategorySalaryLevelDTO } from "@models/CategorySalaryLevelDTO";
import {useDeleteCategorySalaryLevelMutation} from "@API/services/CategorySalaryLevel.service";
import {useGetListCategorySalaryLevelQuery} from "@API/services/CategorySalaryLevel.service";
import {formatMoney} from "~/units";
import {useGetListCategorySalaryScaleQuery} from "@API/services/CategorySalaryScale.service";

function _CategorySalaryLevel() {
  const { data: ListCategorySalaryLevel, isLoading: LoadingListCategorySalaryLevel } = useGetListCategorySalaryLevelQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListCategorySalaryScale } = useGetListCategorySalaryScaleQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [deleteListCategorySalaryLevel, { isLoading: isLoadingDeleteListCategorySalaryLevel }] = useDeleteCategorySalaryLevelMutation();
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



  const menuAction = (record: CategorySalaryLevelDTO) => {
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
  const columns: ColumnsType<CategorySalaryLevelDTO> = [
    {
      title: "Tên",
      dataIndex: "nameSalaryLevel",
      key: "nameSalaryLevel"
    },
    {
      title: "Số tiền",
      dataIndex: "amount",
      key: "amount",
      render: (text: number) => formatMoney(text)
    },
    {
      title: "Ngạch Lương",
      dataIndex: "idSalaryScale",
      key: "idSalaryScale",
      render: (text) => ListCategorySalaryScale?.listPayload?.find((x) => x.id === text)?.nameSalaryScale
    },
    {
      title: "Loại bậc lương",
      dataIndex: "isCoefficient",
      key: "isCoefficient",
      render: (text) => (text ? "Theo hệ số" : "Không theo hệ số"),
    },    {
      title: "Hệ số đóng BHXH",
      dataIndex: "socialSecurityContributionRate",
      key: "socialSecurityContributionRate",
    },
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
  const propsTable: TableProps<CategorySalaryLevelDTO> = {
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
    dataSource: ListCategorySalaryLevel?.listPayload,
    // onChange: handleChange,
    loading: LoadingListCategorySalaryLevel,
    pagination: {
      total: ListCategorySalaryLevel?.totalElement,
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
        <NewAndUpdateCategorySalaryLevel setVisible={setIsOpenModal} id={id} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh mục lớp học </Typography.Title>
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
                  const result = await deleteListCategorySalaryLevel({ idCategorySalaryLevel: selectedRowKeys as string[] }).unwrap();
                  if (result.success) return setSelectedRowKeys([]);
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                danger
                type="primary"
                loading={isLoadingDeleteListCategorySalaryLevel}
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
export const CategorySalaryLevel = WithErrorBoundaryCustom(_CategorySalaryLevel);
