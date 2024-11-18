// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useGetListLaborEquipmentUnitByUnitAndEmployeeQuery } from "@API/services/LaborEquipmentUnitApis.service";
import { useGetListCategoryLaborEquipmentQuery } from "@API/services/CategoryLaborEquipmentApis.service";
import { GetFIleEmployee, useGetListEmployeeQuery } from "@API/services/Employee.service";
import { Button, Card, Col, Row, Space, Spin, TableProps, Tag } from "antd";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import React, { useEffect, useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import dayjs from "dayjs";
import { LaborEquipmentUnitDTO } from "@models/laborEquipmentUnitDTO";
import { LazyLoadImage } from "react-lazy-load-image-component";
import { avatar } from "@admin/asset/icon";
import { CheckCircleOutlined } from "@ant-design/icons";
import {
  useGetTicketLaborEquipmentByIdQuery,
  useUpdateTicketLaborEquipmentDetailMutation
} from "@API/services/TicketLaborEquipmentApis.service";
import { HandleError } from "@admin/components";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";

interface IProps {
  setVisible: (value: boolean) => void;
  idUnit?: string;
  idTicketLaborEquipment?: string;
  idEmployee?: string;
  type: number;
}

function _TicketLaborEquipmentDetailFixAndOther(props: IProps) {
  const { setVisible, idTicketLaborEquipment } = props;
  const { data: ListLaborEquipmentUnit, isLoading: isLoadingListLaborEquipmentUnit } =
    useGetListLaborEquipmentUnitByUnitAndEmployeeQuery({
      pageSize: 0,
      pageNumber: 0
    });
  const { data: ListCategoryLaborEquipment } = useGetListCategoryLaborEquipmentQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListEmployee } = useGetListEmployeeQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: TicketLaborEquipment, isLoading: LoadingTicketLaborEquipment } = useGetTicketLaborEquipmentByIdQuery(
    { idTicketLaborEquipment: idTicketLaborEquipment! },
    {
      skip: !idTicketLaborEquipment
    }
  );
  const { data: ListUnit } = useGetListUnitQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [UpdateTicketLaborEquipmentDetail, { isLoading: isLoadingUpdateTicketLaborEquipmentDetail }] =
    useUpdateTicketLaborEquipmentDetailMutation();
  useEffect(() => {
    if (TicketLaborEquipment?.payload) {
      const listCode = TicketLaborEquipment?.payload?.ticketLaborEquipmentDetail?.map((item) => item.equipmentCode);
      setSelectedRowKeys(listCode);
    }
  }, [TicketLaborEquipment?.payload]);

  const handleSaveLaborEquipmentUnitToTicketLaborEquipmentDetail = async () => {
    try {
      const filter = ListLaborEquipmentUnit?.listPayload
        ?.filter((item) => selectedRowKeys.includes(item.equipmentCode))
        ?.map((x) => ({
          ...x,
          id: "00000000-0000-0000-0000-000000000000",
          quantity: 1,
          idTicketLaborEquipment: idTicketLaborEquipment
        }));
      if (filter?.length === 0) return;

      const res = await UpdateTicketLaborEquipmentDetail(filter).unwrap();
      if (res.success) {
        setVisible(false);
        setSelectedRowKeys([]);
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<LaborEquipmentUnitDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setFilteredInfo(filters);
  };
  const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
    setSelectedRowKeys(newSelectedRowKeys);
  };
  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange
  };
  const columns: ColumnsType<LaborEquipmentUnitDTO> = [
    {
      title: "Mã",
      dataIndex: "equipmentCode",
      key: "equipmentCode",
      filters: ListLaborEquipmentUnit?.listPayload?.map((item) => ({
        text: item.equipmentCode,
        value: item.equipmentCode
      })),
      filteredValue: filteredInfo?.equipmentCode || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.equipmentCode.toString().startsWith(value)
    },
    {
      title: "Thiết bị",
      dataIndex: "idCategoryLaborEquipment",
      key: "idCategoryLaborEquipment",
      render: (text) => ListCategoryLaborEquipment?.listPayload?.find((item) => item.id === text)?.name
    },
    {
      title: "Nhân viên",
      dataIndex: "idEmployee",
      key: "idEmployee",
      render: (text) => {
        const employee = ListEmployee?.listPayload?.find((item) => item.id === text);
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
            {employee?.name}
          </Space>
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
      title: "Phòng ban",
      dataIndex: "idUnit",
      key: "idUnit",
      render: (text) => ListUnit?.listPayload?.find((item) => item.id === text)?.unitName
    },
    {
      title: "Trạng thái",
      dataIndex: "status",
      key: "status",
      render: (text) => {
        switch (text) {
          case 0:
            return <Tag color="blue-inverse">Đang sử dụng</Tag>;
          case 1:
            return <Tag color="red-inverse">Đã hỏng</Tag>;
          case 2:
            return <Tag color="purple-inverse">Thu hồi - nhập kho</Tag>;
        }
      }
    },

    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListLaborEquipmentUnit?.listPayload?.map((item) => ({
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
    rowKey: (record) => record.equipmentCode,
    columns: columns.map((item) => ({
      ellipsis: true,
      with: 150,
      ...item
    })),
    rowSelection: rowSelection,
    dataSource: ListLaborEquipmentUnit?.listPayload,
    onChange: handleChange,
    loading: isLoadingListLaborEquipmentUnit,
    pagination: {
      total: ListLaborEquipmentUnit?.totalElement,
      pageSize: 6,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion

  return (
    <div className="ConfirmCompleteTicketLaborEquipment">
      <Spin spinning={isLoadingListLaborEquipmentUnit || LoadingTicketLaborEquipment} size={"large"}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Space
              style={{
                marginBottom: 16,
                width: "100%",
                justifyContent: "flex-end"
              }}
            >
              <Button
                type="primary"
                onClick={async () => await handleSaveLaborEquipmentUnitToTicketLaborEquipmentDetail()}
                icon={<CheckCircleOutlined />}
                disabled={selectedRowKeys.length === 0}
                loading={isLoadingUpdateTicketLaborEquipmentDetail}
              >
                Xác nhận chọn {selectedRowKeys.length} thiết bị
              </Button>
            </Space>
            <Card bordered={false} className="criclebox">
              <DragAndDropTable {...propsTable} />
            </Card>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const TicketLaborEquipmentDetailFixAndOther = WithErrorBoundaryCustom(_TicketLaborEquipmentDetailFixAndOther);
