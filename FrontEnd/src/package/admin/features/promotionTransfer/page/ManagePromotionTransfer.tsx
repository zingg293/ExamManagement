// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { ColumnsType } from "antd/lib/table/interface";
import { App, Button, Card, Col, Divider, Form, Row, Select, Space, TableProps, Tag, Tooltip, Typography } from "antd";
import dayjs from "dayjs";
import { HandleError } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { GetFIleEmployee, useGetListEmployeeQuery } from "@API/services/Employee.service";
import { LazyLoadImage } from "react-lazy-load-image-component";
import { avatar } from "@admin/asset/icon";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";
import { InfoCircleOutlined, SearchOutlined } from "@ant-design/icons";
import { useLazyFilterPromotionTransferQuery } from "@API/services/PromotionTransferApis.service";
import { PromotionTransferDTO } from "@models/promotionTransferDTO";
import { DetailPromotionTransfer } from "@admin/features/promotionTransfer";

function _ManagePromotionTransfer() {
  const { modal } = App.useApp();
  const [FilterPromotionTransfer, { data: ListPromotionTransfer, isLoading: LoadingListPromotionTransfer }] =
    useLazyFilterPromotionTransferQuery();
  const { data: ListEmployee, isLoading: LoadingListEmployee } = useGetListEmployeeQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitAvailableQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const handleFilter = async (values: any) => {
    try {
      await FilterPromotionTransfer({
        ...values,
        pageNumber: 0,
        pageSize: 0
      }).unwrap();
    } catch (e: any) {
      await HandleError(e);
    }
  };
  const handleViewDetail = (id: string) => {
    modal.info({
      title: "Chi tiết yêu cầu",
      icon: <InfoCircleOutlined />,
      width: 1100,
      content: <DetailPromotionTransfer idPromotionTransfer={id} />,
      footer: null,
      closable: true,
      style: { top: 20 }
    });
  };

  //#region Table config
  const columns: ColumnsType<PromotionTransferDTO> = [
    {
      title: "Nhân viên",
      dataIndex: "idEmployee",
      key: "idEmployee",
      render: (text) => {
        const employee = ListEmployee?.listPayload?.find((item) => item.id === text);
        return (
          <Tooltip title={employee?.name + "-" + employee?.email}>
            <Space size={3}>
              <LazyLoadImage
                alt={`avatar-${employee?.name}`}
                effect="blur"
                width={24}
                height={24}
                style={{ objectFit: "cover", borderRadius: "50%" }}
                placeholderSrc={GetFIleEmployee(employee?.id + ".jpg")}
                src={employee?.avatar ? GetFIleEmployee(employee?.id + ".jpg") : avatar}
              />{" "}
              {employee?.name} - {employee?.email}
            </Space>
          </Tooltip>
        );
      }
    },
    {
      title: "Phòng ban",
      dataIndex: "idUnit",
      key: "idUnit",
      render: (text) => {
        const unit = ListUnit?.listPayload?.find((item) => item.id === text);
        return (
          <Tooltip title={unit?.unitName + "-" + unit?.unitCode}>
            <Tag color="blue-inverse">
              {unit?.unitName} - {unit?.unitCode}
            </Tag>
          </Tooltip>
        );
      }
    },
    {
      title: "Chi tiết",
      render: (_, record) => (
        <Button type="link" onClick={() => handleViewDetail(record.id)}>
          Xem chi tiết
        </Button>
      )
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    }
  ];
  const propsTable: TableProps<PromotionTransferDTO> = {
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
    dataSource: ListPromotionTransfer?.listPayload,
    loading: LoadingListPromotionTransfer,
    pagination: {
      total: ListPromotionTransfer?.totalElement,
      pageSize: 10,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className="ManagePromotionTransfer">
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Quản lý bổ nhiệm và điều chuyển </Typography.Title>
          <Divider />
          <Card bordered={false} className="criclebox">
            <Space
              wrap
              style={{
                width: "100%",
                justifyContent: "space-between"
              }}
            >
              <Form layout="vertical" onFinish={handleFilter}>
                <Space wrap>
                  <Form.Item name="idEmployee" label="Nhân viên">
                    <Select
                      allowClear
                      showSearch
                      options={ListEmployee?.listPayload?.map((user) => {
                        return { label: user.name + " - " + user.email, value: user.id };
                      })}
                      loading={LoadingListEmployee}
                      optionFilterProp={"label"}
                      placeholder={"Chọn nhân viên"}
                    />
                  </Form.Item>
                  <Form.Item name="idUnit" label="Phòng ban">
                    <Select
                      allowClear
                      showSearch
                      options={ListUnit?.listPayload?.map((user) => {
                        return { label: user.unitName + " - " + user.unitCode, value: user.id };
                      })}
                      loading={LoadingListUnit}
                      optionFilterProp={"label"}
                      placeholder={"Chọn phòng ban"}
                    />
                  </Form.Item>
                  <Form.Item label={"Tìm kiếm"}>
                    <Button type="primary" htmlType="submit" icon={<SearchOutlined />}>
                      Tìm kiếm
                    </Button>
                  </Form.Item>
                </Space>
              </Form>
            </Space>
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const ManagePromotionTransfer = WithErrorBoundaryCustom(_ManagePromotionTransfer);
