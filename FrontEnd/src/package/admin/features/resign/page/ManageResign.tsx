// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useState } from "react";
import {
  Avatar,
  Button,
  Card,
  Col,
  Divider,
  Form,
  Modal,
  Row,
  Select,
  Space,
  TableProps,
  Tag,
  Tooltip,
  Typography
} from "antd";
import { CheckCircleOutlined, RetweetOutlined, SettingOutlined, UserSwitchOutlined } from "@ant-design/icons";
import { ColumnsType, FilterValue } from "antd/lib/table/interface";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import {
  GetFIleEmployee,
  useGetEmployeeResignedQuery,
  useUpdateTypeOfEmployeeMutation
} from "@API/services/Employee.service";
import { LazyLoadImage } from "react-lazy-load-image-component";
import { avatar } from "@admin/asset/icon";
import { EmployeeDTO } from "@models/employeeDTO";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { useGetListEmployeeTypeQuery } from "@API/services/EmployeeType.service";
import { HandleError } from "@admin/components";

function _ManageResign() {
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10
  });
  const { data: ListEmployeeResign, isLoading: LoadingListEmployeeResign } = useGetEmployeeResignedQuery({
    pageSize: pagination.pageSize,
    pageNumber: pagination.pageNumber
  });
  const { data: ListUnit } = useGetListUnitQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListEmployeeType, isLoading: LoadingListEmployeeType } = useGetListEmployeeTypeQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [updateTypeOfEmployee, { isLoading: LoadingUpdateTypeOfEmployee }] = useUpdateTypeOfEmployeeMutation();

  const [modal, contextHolder] = Modal.useModal();

  const handleUpdateTypeOfEmployee = async (value: any) => {
    try {
      await updateTypeOfEmployee(value);
    } catch (error: any) {
      await HandleError(error);
    }
  };
  const handleUpdateEmployeeType = (Employee: EmployeeDTO) => {
    const unit = ListUnit?.listPayload?.find((item) => item.id === Employee.idUnit);
    return modal.info({
      title: "Cập nhật nhân viên",
      icon: <UserSwitchOutlined />,
      width: 800,
      content: (
        <Card>
          <Form layout="vertical" onFinish={handleUpdateTypeOfEmployee}>
            <Form.Item>
              <Space direction={"horizontal"}>
                {Employee.avatar && <Avatar shape={"square"} src={GetFIleEmployee(Employee.id + ".jpg")} size={120} />}
                <Space direction={"vertical"}>
                  <Typography.Text>{Employee.name} </Typography.Text>
                  <Typography.Text>{Employee.email}</Typography.Text>
                  <Tag color={"blue-inverse"}>
                    {unit?.unitName} - {unit?.unitCode}
                  </Tag>
                </Space>
              </Space>
            </Form.Item>
            <Form.Item name={"idEmployee"} hidden initialValue={Employee.id} />
            <Form.Item
              label="Loại nhân viên"
              name={"idTypeOfEmployee"}
              rules={[
                {
                  required: true,
                  message: "Vui lòng chọn loại nhân viên"
                }
              ]}
              initialValue={Employee.typeOfEmployee}
            >
              <Select
                showSearch
                optionFilterProp={"label"}
                loading={LoadingListEmployeeType}
                options={ListEmployeeType?.listPayload?.map((item) => ({
                  label: <Tag color={"magenta-inverse"}>{item.typeName}</Tag>,
                  value: item.id
                }))}
              />
            </Form.Item>
            <Form.Item>
              <Space
                style={{
                  width: "100%",
                  justifyContent: "flex-end"
                }}
              >
                <Button
                  type="default"
                  htmlType="reset"
                  loading={LoadingUpdateTypeOfEmployee}
                  icon={<RetweetOutlined />}
                >
                  Xóa
                </Button>
                <Button
                  type="primary"
                  htmlType="submit"
                  loading={LoadingUpdateTypeOfEmployee}
                  icon={<CheckCircleOutlined />}
                >
                  Lưu
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </Card>
      ),
      footer: null,
      closable: true
    });
  };

  //#region Table config
  const [filteredInfo, setFilteredInfo] = useState<Record<string, FilterValue | null>>({});
  const handleChange: TableProps<EmployeeDTO>["onChange"] = (pagination, filters) => {
    // pagination = pagination || {};
    setPagination({ pageNumber: pagination.current || 1, pageSize: pagination.pageSize || 10 });
    setFilteredInfo(filters);
  };
  const columns: ColumnsType<EmployeeDTO> = [
    {
      title: "Nhân viên",
      dataIndex: "id",
      key: "id",
      render: (text, record) => {
        return (
          <Tooltip title={record?.name + "-" + record?.email}>
            <Space size={3}>
              <LazyLoadImage
                alt={`avatar-${record?.name}`}
                effect="blur"
                width={24}
                height={24}
                style={{ objectFit: "cover", borderRadius: "50%" }}
                placeholderSrc={GetFIleEmployee(record?.id + ".jpg")}
                src={record?.avatar ? GetFIleEmployee(record?.id + ".jpg") : avatar}
              />{" "}
              {record?.name} - {record?.email}
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
          <Tooltip title={`${unit?.unitName} - ${unit?.unitCode}`}>
            <Tag color={"blue-inverse"}>{`${unit?.unitName} - ${unit?.unitCode}`}</Tag>
          </Tooltip>
        );
      }
    },
    {
      title: "Loại nhân viên",
      dataIndex: "typeOfEmployee",
      key: "typeOfEmployee",
      render: (text) => {
        const type = ListEmployeeType?.listPayload?.find((item) => item.id === text);
        return (
          <Tooltip title={type?.typeName}>
            <Tag color={"magenta-inverse"}>{type?.typeName}</Tag>
          </Tooltip>
        );
      }
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      filters: ListEmployeeResign?.listPayload?.map((item) => ({
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
      fixed: "right",
      width: "100px",
      render: (_, record) => (
        <Button type="text" size={"small"} onClick={() => handleUpdateEmployeeType(record)}>
          <SettingOutlined style={{ fontSize: 20 }} />
        </Button>
      )
    }
  ];
  const propsTable: TableProps<EmployeeDTO> = {
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
    dataSource: ListEmployeeResign?.listPayload,
    onChange: handleChange,
    loading: LoadingListEmployeeResign,
    pagination: {
      total: ListEmployeeResign?.totalElement,
      pageSize: 10,
      pageSizeOptions: ["10", "20", "30", "50", "100", "200"],
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small"
  };
  //#endregion
  return (
    <div className={"ManageResign"}>
      {contextHolder}
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Quản lý thôi việc</Typography.Title>
          <Divider />
          <Space
            style={{
              marginBottom: 16
            }}
            wrap
          ></Space>
          <Card bordered={false} className="criclebox">
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const ManageResign = WithErrorBoundaryCustom(_ManageResign);
