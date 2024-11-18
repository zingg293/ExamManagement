// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { App, Button, Card, Col, Divider, Form, Row, Select, Space, TableProps, Tag, Tooltip, Typography } from "antd";
import { LaborEquipmentUnitDTO } from "@models/laborEquipmentUnitDTO";
import dayjs from "dayjs";
import { HandleError } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { GetFIleEmployee, useGetListEmployeeQuery } from "@API/services/Employee.service";
import { LazyLoadImage } from "react-lazy-load-image-component";
import { avatar } from "@admin/asset/icon";
import { useLazyFilterLaborEquipmentUnitModelQuery } from "@API/services/LaborEquipmentUnitApis.service";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";
import { PlusOutlined, SearchOutlined } from "@ant-design/icons";
import { NewLaborEquipmentUnit } from "@admin/features/TicketLaborEquipment";

function _ManageTicketLaborEquipment() {
  const { modal } = App.useApp();
  const [FilterLaborEquipmentUnit, { data: ListOvertime, isLoading: LoadingListOvertime }] =
    useLazyFilterLaborEquipmentUnitModelQuery();
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
      await FilterLaborEquipmentUnit({
        ...values,
        pageNumber: 0,
        pageSize: 0
      }).unwrap();
    } catch (e: any) {
      await HandleError(e);
    }
  };
  const handleNewLaborEquipmentUnit = () => {
    modal.info({
      title: "Thêm mới thiết bị",
      content: <NewLaborEquipmentUnit />,
      width: 800,
      footer: null,
      closable: true
    });
  };

  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<LaborEquipmentUnitDTO>["onChange"] = (pagination, filters) => {
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<LaborEquipmentUnitDTO> = [
    {
      title: "Mã",
      dataIndex: "equipmentCode",
      key: "equipmentCode",
      width: 125
    },
    {
      title: "Thiết bị",
      render: (_, record: any) => {
        return record?.categoryLaborEquipment?.name + " - " + record?.categoryLaborEquipment?.code;
      }
    },
    {
      title: "Trạng thái",
      dataIndex: "status",
      key: "status",
      render: (text) => {
        const status = {
          0: <Tag color="green-inverse">Đang sử dụng</Tag>,
          1: <Tag color="red-inverse">Đã hỏng</Tag>,
          2: <Tag color="purple-inverse">Thu hồi - nhập kho</Tag>
        };
        const number = text as keyof typeof status;
        return status[number];
      }
    },
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
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListOvertime?.listPayload?.map((item) => ({
        text: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm"),
        value: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm")
      })),
      filteredValue: filteredInfo?.createdDate || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.createdDate.toString().startsWith(value),
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    }
  ];
  const propsTable: TableProps<LaborEquipmentUnitDTO> = {
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
    dataSource: ListOvertime?.listPayload,
    onChange: handleChange,
    loading: LoadingListOvertime,
    pagination: {
      total: ListOvertime?.totalElement,
      pageSize: 10,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className="ManageTicketLaborEquipment">
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Quản lý thiết bị lao động </Typography.Title>
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
                  <Form.Item name="status" label="Loại">
                    <Select
                      allowClear
                      showSearch
                      options={[
                        { label: <Tag color={"green-inverse"}>Đang sử dụng</Tag>, value: 0 },
                        { label: <Tag color={"red-inverse"}>Đã hỏng</Tag>, value: 1 },
                        { label: <Tag color={"purple-inverse"}>Thu hồi - nhập kho</Tag>, value: 2 }
                      ]}
                      optionFilterProp={"label"}
                      placeholder={"Chọn loại trạng thái"}
                    />
                  </Form.Item>
                  <Form.Item label={"Tìm kiếm"}>
                    <Button type="primary" htmlType="submit" icon={<SearchOutlined />}>
                      Tìm kiếm
                    </Button>
                  </Form.Item>
                </Space>
              </Form>
              <Button type="primary" icon={<PlusOutlined />} onClick={() => handleNewLaborEquipmentUnit()}>
                Thêm mới thiết bị
              </Button>
            </Space>
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const ManageTicketLaborEquipment = WithErrorBoundaryCustom(_ManageTicketLaborEquipment);
