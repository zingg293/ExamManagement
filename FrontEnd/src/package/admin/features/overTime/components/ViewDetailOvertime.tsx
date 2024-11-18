import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Col, Descriptions, Row, Spin } from "antd";
import { useGetOvertimeByIdQuery } from "@API/services/OvertimeApis.service";
import { useGetUnitByIdQuery } from "@API/services/UnitApis.service";
import { useGetEmployeeByIdQuery } from "@API/services/Employee.service";
import { useGetUserByIdQuery } from "@API/services/UserApis.service";
import dayjs from "dayjs";

interface ViewDetailOvertimeProps {
  idOvertime: string;
}

function _ViewDetailOvertime(props: ViewDetailOvertimeProps) {
  const { idOvertime } = props;
  const { data: dataOvertime, isLoading: isLoadingOvertime } = useGetOvertimeByIdQuery(
    { idOvertime },
    { skip: !idOvertime }
  );
  const DataOvertime = dataOvertime?.payload;
  const { data: dataUnit } = useGetUnitByIdQuery(
    {
      id: DataOvertime?.idUnit || ""
    },
    {
      skip: !DataOvertime?.idUnit
    }
  );
  const { data: dataEmployee } = useGetEmployeeByIdQuery(
    {
      idEmployee: DataOvertime?.idEmployee || ""
    },
    {
      skip: !DataOvertime?.idEmployee
    }
  );
  const { data: dataUser } = useGetUserByIdQuery(
    {
      id: DataOvertime?.idUserRequest || ""
    },
    {
      skip: !DataOvertime?.idUserRequest
    }
  );

  return (
    <div className="ViewDetailOvertime">
      <Spin spinning={isLoadingOvertime} size={"large"}>
        <Row>
          <Col span={24}>
            <Descriptions bordered layout={"vertical"}>
              <Descriptions.Item label="Mô tả" span={3}>
                {DataOvertime?.description}
              </Descriptions.Item>
              <Descriptions.Item label="Tên phòng ban">{DataOvertime?.unitName}</Descriptions.Item>
              <Descriptions.Item label="Phòng ban" span={2}>
                {dataUnit?.payload?.unitName} - {dataUnit?.payload?.unitCode}
              </Descriptions.Item>
              <Descriptions.Item label="Nhân viên">
                {dataEmployee?.payload?.name} - {dataEmployee?.payload?.code}
              </Descriptions.Item>
              <Descriptions.Item label="Ngưởi tạo">
                {dataUser?.payload?.fullname} - {dataUser?.payload?.email}
              </Descriptions.Item>
              <Descriptions.Item label="Thời gian">
                {dayjs(DataOvertime?.fromDate).format("DD/MM/YYYY HH:mm")} -
                {dayjs(DataOvertime?.toDate).format("DD/MM/YYYY HH:mm")}
              </Descriptions.Item>
            </Descriptions>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const ViewDetailOvertime = WithErrorBoundaryCustom(_ViewDetailOvertime);
