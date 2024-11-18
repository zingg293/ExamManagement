// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { GetFIleEmployee, useGetListEmployeeQuery } from "@API/services/Employee.service";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { useGetListBusinessTripEmployeeByIdBusinessTripQuery } from "@API/services/BusinessTripEmployeeApis.service";
import { Card, Col, Row, Space, Spin, TableProps, Tag, Typography } from "antd";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { LazyLoadImage } from "react-lazy-load-image-component";
import { avatar } from "@admin/asset/icon";
import dayjs from "dayjs";
import { BusinessTripEmployeeDTO } from "@models/businessTripEmployeeDTO";
import { CheckCircleOutlined } from "@ant-design/icons";

interface IProps {
  idBusinessTrip: string;
}

function _ViewDetailBusinessTripEmployee(props: IProps) {
  const { idBusinessTrip } = props;
  const { data: dataEmployee } = useGetListEmployeeQuery(
    { pageNumber: 0, pageSize: 0 },
    {
      skip: !idBusinessTrip
    }
  );
  const { data: dataUnit } = useGetListUnitQuery(
    { pageNumber: 0, pageSize: 0 },
    {
      skip: !idBusinessTrip
    }
  );
  const { data: dataBusinessTripEmployee, isLoading: isLoadingBusinessTripEmployee } =
    useGetListBusinessTripEmployeeByIdBusinessTripQuery(
      {
        idBusinessTrip
      },
      {
        skip: !idBusinessTrip
      }
    );
  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<BusinessTripEmployeeDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<BusinessTripEmployeeDTO> = [
    {
      title: "STT",
      width: 50,
      render: (text, record, index) => index + 1
    },
    {
      title: "Trưởng nhóm",
      dataIndex: "captain",
      key: "captain",
      render: (text) =>
        text ? (
          <Tag color={"blue"}>
            <CheckCircleOutlined
              style={{
                fontSize: 24
              }}
            />
          </Tag>
        ) : (
          ""
        ),
      width: 120
    },
    {
      title: "Nhân viên",
      dataIndex: "idEmployee",
      key: "idEmployee",
      render: (text) => {
        const employee = dataEmployee?.listPayload?.find((item) => item.id === text);
        return (
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
            <Typography.Text>{employee?.name}</Typography.Text>
          </Space>
        );
      }
    },
    {
      title: "Mã nhân viên",
      dataIndex: "idEmployee",
      key: "employeeCode",
      render: (text) => dataEmployee?.listPayload?.find((item) => item.id === text)?.code
    },
    {
      title: "Phòng ban",
      dataIndex: "idEmployee",
      render: (text) => {
        const user = dataEmployee?.listPayload?.find((item) => item.id === text)?.idUnit;
        const unit = dataUnit?.listPayload?.find((item) => item.id === user);
        return unit?.unitName;
      }
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: dataBusinessTripEmployee?.listPayload?.map((item) => ({
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
      width: "6%"
    }
  ];
  const propsTable: TableProps<BusinessTripEmployeeDTO> = {
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
    dataSource: dataBusinessTripEmployee?.listPayload,
    onChange: handleChange,
    loading: isLoadingBusinessTripEmployee,
    pagination: {
      total: dataBusinessTripEmployee?.totalElement,
      pageSize: 6,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className="ViewDetaailBusinessTripEmployee">
      <Spin spinning={isLoadingBusinessTripEmployee} size={"large"}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Card bordered={false} className="criclebox">
              <DragAndDropTable {...propsTable} />
            </Card>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const ViewDetailBusinessTripEmployee = WithErrorBoundaryCustom(_ViewDetailBusinessTripEmployee);
