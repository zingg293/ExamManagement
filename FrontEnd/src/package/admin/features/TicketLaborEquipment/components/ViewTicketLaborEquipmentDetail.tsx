// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Card, Col, Row, Space, Spin, TableProps, Tooltip } from "antd";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { useGetListCategoryLaborEquipmentQuery } from "@API/services/CategoryLaborEquipmentApis.service";
import { GetFIleEmployee, useGetListEmployeeQuery } from "@API/services/Employee.service";
import { useGetTicketLaborEquipmentByIdQuery } from "@API/services/TicketLaborEquipmentApis.service";
import { useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { LazyLoadImage } from "react-lazy-load-image-component";
import { avatar } from "@admin/asset/icon";
import dayjs from "dayjs";
import { TicketLaborEquipmentDetailDTO } from "@models/ticketLaborEquipmentDetailDTO";

interface IProps {
  setVisible: (value: boolean) => void;
  idTicketLaborEquipment?: string;
}

function _ViewTicketLaborEquipmentDetail(props: IProps) {
  const { idTicketLaborEquipment } = props;
  const { data: TicketLaborEquipment, isLoading: LoadingTicketLaborEquipment } = useGetTicketLaborEquipmentByIdQuery(
    { idTicketLaborEquipment: idTicketLaborEquipment! },
    {
      skip: !idTicketLaborEquipment
    }
  );
  const { data: ListCategoryLaborEquipment } = useGetListCategoryLaborEquipmentQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListEmployee } = useGetListEmployeeQuery({
    pageSize: 0,
    pageNumber: 0
  });

  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<TicketLaborEquipmentDetailDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<TicketLaborEquipmentDetailDTO> = [
    {
      title: "STT",
      width: 50,
      render: (text, record, index) => index + 1
    },
    {
      title: "Mã",
      dataIndex: "equipmentCode",
      key: "equipmentCode"
    },
    {
      title: "Thiết bị",
      dataIndex: "idCategoryLaborEquipment",
      key: "idCategoryLaborEquipment",
      render: (text) => ListCategoryLaborEquipment?.listPayload?.find((item) => item.id === text)?.name
    },
    {
      title: "Số lượng",
      dataIndex: "quantity",
      key: "quantity",
      width: 80
    },
    {
      title: "Nhân viên",
      dataIndex: "idEmployee",
      key: "idEmployee",
      render: (text) => {
        const employee = ListEmployee?.listPayload?.find((item) => item.id === text);
        return (
          <Tooltip title={employee?.name + " - " + employee.code}>
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
              {employee?.name} - {employee?.code}
            </Space>
          </Tooltip>
        );
      }
    },
    {
      title: "Mã nhân viên",
      dataIndex: "idEmployee",
      key: "employeeCode",
      render: (text) => ListEmployee?.listPayload?.find((item) => item.id === text)?.code
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: TicketLaborEquipment?.listPayload?.map((item) => ({
        text: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm"),
        value: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm")
      })),
      filteredValue: filteredInfo?.createdDate || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.createdDate.toString().startsWith(value),
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    }
  ];
  const propsTable: TableProps<TicketLaborEquipmentDetailDTO> = {
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
    dataSource: TicketLaborEquipment?.payload.ticketLaborEquipmentDetail,
    onChange: handleChange,
    loading: LoadingTicketLaborEquipment,
    pagination: {
      total: TicketLaborEquipment?.payload.ticketLaborEquipmentDetail?.length,
      pageSize: 6,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className="ViewTicketLaborEquipmentDetail">
      <Spin spinning={LoadingTicketLaborEquipment} size={"large"}>
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

export const ViewTicketLaborEquipmentDetail = WithErrorBoundaryCustom(_ViewTicketLaborEquipmentDetail);
