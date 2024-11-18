// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { Button, Card, Col, Divider, Form, Row, Select, Space, TableProps, Tooltip, Typography } from "antd";
import { InternRequestDTO } from "@models/internRequestDTO";
import dayjs from "dayjs";
import { HandleError } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { GetFIleEmployee, useGetListEmployeeQuery } from "@API/services/Employee.service";
import { LazyLoadImage } from "react-lazy-load-image-component";
import { avatar } from "@admin/asset/icon";
import { useState } from "react";
import { useLazyFilterListInternRequestQuery } from "@API/services/InternRequestApis.service";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";
import { useGetListCategoryPositionAvailableQuery } from "@API/services/CategoryPositionApis.service";
import { SearchOutlined } from "@ant-design/icons";

function _ManageInternRequest() {
  const [FilterInternRequest, { data: ListOvertime, isLoading: LoadingListOvertime }] =
    useLazyFilterListInternRequestQuery();
  const { data: ListEmployee, isLoading: LoadingListEmployee } = useGetListEmployeeQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitAvailableQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const { data: ListCategoryPosition, isLoading: LoadingListCategoryPosition } =
    useGetListCategoryPositionAvailableQuery({
      pageNumber: 0,
      pageSize: 0
    });
  const handleFilter = async (values: any) => {
    try {
      if (values.formDate) values.formDate = dayjs(values.formDate).format("DD/MM/YYYY HH:mm");
      if (values.toDate) values.toDate = dayjs(values.toDate).format("DD/MM/YYYY HH:mm");
      await FilterInternRequest({
        ...values,
        pageNumber: 0,
        pageSize: 0
      }).unwrap();
    } catch (e: any) {
      await HandleError(e);
    }
  };

  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<InternRequestDTO>["onChange"] = (pagination, filters) => {
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<InternRequestDTO> = [
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
      dataIndex: "unitName",
      key: "unitName"
    },
    {
      title: "Vị trí",
      dataIndex: "positionName",
      key: "positionName"
    },
    {
      title: "Ghi chú",
      dataIndex: "description",
      key: "description"
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
  const propsTable: TableProps<InternRequestDTO> = {
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
    <div className="ManageInternRequest">
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Quản lý thử việc </Typography.Title>
          <Divider />
          <Card bordered={false} className="criclebox">
            <Space
              style={{
                marginBottom: 16
              }}
              wrap
            >
              <Form layout="vertical" onFinish={handleFilter}>
                <Space>
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
                  <Form.Item name="idPosition" label="Vị trí">
                    <Select
                      allowClear
                      showSearch
                      options={ListCategoryPosition?.listPayload?.map((user) => {
                        return { label: user.positionName, value: user.id };
                      })}
                      loading={LoadingListCategoryPosition}
                      optionFilterProp={"label"}
                      placeholder={"Chọn vị trí"}
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

export const ManageInternRequest = WithErrorBoundaryCustom(_ManageInternRequest);
