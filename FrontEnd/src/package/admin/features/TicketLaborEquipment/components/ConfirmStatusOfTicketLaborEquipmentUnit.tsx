// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useGetTicketLaborEquipmentByIdQuery } from "@API/services/TicketLaborEquipmentApis.service";
import { App, Button, Card, Col, Form, Input, Modal, Row, Select, Space, Spin, TableProps, Tag } from "antd";
import {
  useGetListLaborEquipmentUnitByListEquipmentCodeQuery,
  useUpdateLaborEquipmentUnitByCodeAndStatusMutation
} from "@API/services/LaborEquipmentUnitApis.service";
import { Fragment, useState } from "react";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import { LaborEquipmentUnitDTO } from "@models/laborEquipmentUnitDTO";
import { LazyLoadImage } from "react-lazy-load-image-component";
import { GetFIleEmployee, useGetListEmployeeQuery } from "@API/services/Employee.service";
import { avatar } from "@admin/asset/icon";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { useGetListCategoryLaborEquipmentQuery } from "@API/services/CategoryLaborEquipmentApis.service";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, FileProtectOutlined } from "@ant-design/icons";
import { useUpdateStepWorkFlowMutation } from "@API/services/WorkFlowApis.service";

interface IProps {
  idTicketLaborEquipment: string;
  refetchListMain: () => void;
}

function _ConfirmStatusOfTicketLaborEquipmentUnit(props: IProps) {
  const { modal } = App.useApp();
  const { idTicketLaborEquipment, refetchListMain } = props;
  const {
    data: dataTicketLaborEquipment,
    isLoading: isLoadingTicketLaborEquipment,
    refetch: refetchTicketLaborEquipment
  } = useGetTicketLaborEquipmentByIdQuery(
    {
      idTicketLaborEquipment
    },
    { skip: !idTicketLaborEquipment }
  );
  const ticketLaborEquipmentDetail = dataTicketLaborEquipment?.payload?.ticketLaborEquipmentDetail;
  const isCheckAllEqualTrue = ticketLaborEquipmentDetail?.every((item) => item.isCheck);
  const { data: dataLaborEquipmentUnit, isLoading: isLoadingLaborEquipmentUnit } =
    useGetListLaborEquipmentUnitByListEquipmentCodeQuery(
      {
        listEquipmentCode: ticketLaborEquipmentDetail?.map((item) => item.equipmentCode) || [],
        pageSize: 0,
        pageNumber: 0
      },
      {
        skip: !ticketLaborEquipmentDetail
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
  const { data: ListUnit } = useGetListUnitQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [updateLaborEquipmentUnitByCodeAndStatus, { isLoading: isLoadingUpdateLaborEquipmentUnitByCodeAndStatus }] =
    useUpdateLaborEquipmentUnitByCodeAndStatusMutation();
  const [UpdateStepRequestToHired, { isLoading: isLoadingUpdateStatusRequestToHired }] =
    useUpdateStepWorkFlowMutation();
  const handleUpdateLaborEquipmentUnitByCodeAndStatus = async (equipmentCode: string, status: number) => {
    try {
      const res = await updateLaborEquipmentUnitByCodeAndStatus({
        EquipmentCode: equipmentCode,
        status
      }).unwrap();
      if (res.success) {
        await refetchTicketLaborEquipment();
      }
    } catch (error: any) {
      await HandleError(error);
    }
  };

  const handleUpdateStepRequestToHired = async (idWorkFlowInstance: string) => {
    modal.success({
      title: "Hoàn thành yêu cầu",
      icon: <FileProtectOutlined />,
      width: 600,
      content: (
        <Fragment>
          <p>Bạn có chắc chắn muốn xác nhận yêu cầu này?</p>
          <Form
            layout={"vertical"}
            onFinish={async (values) => {
              await updateStepRequestToHired({
                idWorkFlowInstance,
                isRequestToChange: false,
                isTerminated: false,
                message: values?.message
              });
            }}
          >
            <Form.Item name={"message"}>
              <Input.TextArea placeholder="Ghi chú" allowClear showCount />
            </Form.Item>
            <Form.Item>
              <Space
                style={{
                  width: "100%",
                  justifyContent: "flex-end"
                }}
              >
                <Button type="default" htmlType="button" onClick={() => Modal.destroyAll()}>
                  Hủy
                </Button>
                <Button type="primary" htmlType="submit" loading={isLoadingUpdateStatusRequestToHired}>
                  Lưu
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </Fragment>
      ),
      footer: null,
      closable: true
    });
  };
  const updateStepRequestToHired = async (body: {
    isTerminated: boolean;
    idWorkFlowInstance: string;
    isRequestToChange: boolean;
    message: string;
  }) => {
    try {
      const res = await UpdateStepRequestToHired(body).unwrap();
      if (res.success) {
        Modal.destroyAll();
        refetchListMain();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<LaborEquipmentUnitDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<LaborEquipmentUnitDTO> = [
    {
      title: "STT",
      render: (text, record, index) => index + 1,
      width: 50
    },
    {
      title: (
        <Tag color={isCheckAllEqualTrue ? "blue-inverse" : "gold-inverse"}>
          <CheckCircleOutlined />{" "}
        </Tag>
      ),
      render: (_, record) => {
        const isCheck = ticketLaborEquipmentDetail?.find(
          (item) => item.equipmentCode === record.equipmentCode
        )?.isCheck;
        return isCheck ? (
          <Tag color={"blue-inverse"}>
            <CheckCircleOutlined />{" "}
          </Tag>
        ) : (
          <Tag color={"gold-inverse"}>
            <CheckCircleOutlined />{" "}
          </Tag>
        );
      },
      width: 60
    },
    {
      title: "Mã",
      dataIndex: "equipmentCode",
      key: "equipmentCode",
      filters: dataLaborEquipmentUnit?.listPayload?.map((item) => ({
        text: item.equipmentCode,
        value: item.equipmentCode
      })),
      filteredValue: filteredInfo?.equipmentCode || null,
      filterSearch: true,
      onFilter: (value: any, record) => record?.equipmentCode?.toString().startsWith(value)
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
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: dataLaborEquipmentUnit?.listPayload?.map((item) => ({
        text: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm"),
        value: dayjs(item.createdDate).format("DD-MM-YYYY HH:mm")
      })),
      filteredValue: filteredInfo?.createdDate || null,
      filterSearch: true,
      onFilter: (value: any, record) => record.createdDate.toString().startsWith(value),
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Vui lòng chọn",
      width: 165,
      render: (_, record) => {
        return (
          <Select
            placeholder={"Vui lòng chọn"}
            defaultValue={record.status}
            loading={isLoadingUpdateLaborEquipmentUnitByCodeAndStatus}
            onChange={async (e) => await handleUpdateLaborEquipmentUnitByCodeAndStatus(record.equipmentCode, e)}
            style={{ width: "100%" }}
            options={[
              {
                label: <Tag color="green-inverse">Sửa được</Tag>,
                value: 0
              },
              {
                label: <Tag color="red-inverse">Sửa không được</Tag>,
                value: 1
              }
            ]}
          />
        );
      }
    }
  ];
  const propsTable: TableProps<LaborEquipmentUnitDTO> = {
    scroll: {
      x: 800
    },
    bordered: true,
    columns: columns.map((item) => ({
      ellipsis: true,
      with: 150,
      ...item
    })),
    dataSource: dataLaborEquipmentUnit?.listPayload,
    onChange: handleChange,
    loading: isLoadingLaborEquipmentUnit,
    pagination: {
      total: dataLaborEquipmentUnit?.totalElement,
      pageSize: 6,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className="ConfirmStatusOfTicketLaborEquipmentUnit">
      <Spin spinning={isLoadingTicketLaborEquipment}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Card bordered={false} className="criclebox">
              <Space
                style={{
                  marginBottom: 16,
                  width: "100%",
                  justifyContent: "flex-end"
                }}
              >
                <Button
                  icon={<FileProtectOutlined />}
                  type={"primary"}
                  disabled={!isCheckAllEqualTrue}
                  onClick={async () => {
                    await handleUpdateStepRequestToHired(
                      dataTicketLaborEquipment?.payload?.workflowInstances?.at(0)?.id || ""
                    );
                  }}
                >
                  {!isCheckAllEqualTrue
                    ? `Xác nhận ${ticketLaborEquipmentDetail?.filter((item) => item.isCheck)?.length} / ${
                        ticketLaborEquipmentDetail?.length
                      }`
                    : "Xác nhận hoàn thành"}
                </Button>
              </Space>
              <DragAndDropTable {...propsTable} />
            </Card>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const ConfirmStatusOfTicketLaborEquipmentUnit = WithErrorBoundaryCustom(
  _ConfirmStatusOfTicketLaborEquipmentUnit
);
