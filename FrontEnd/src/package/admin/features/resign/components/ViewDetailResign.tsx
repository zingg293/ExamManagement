import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Col, Descriptions, Row, Spin } from "antd";
import { useGetResignByIdQuery } from "@API/services/ResignApis.service";
import { useGetUnitByIdQuery } from "@API/services/UnitApis.service";
import { useGetEmployeeByIdQuery } from "@API/services/Employee.service";
import { useGetUserByIdQuery } from "@API/services/UserApis.service";
import { DownloadButton } from "@admin/components";
import { getFileBusinessTrip } from "@API/services/BusinessTripApis.service";

interface ViewDetailResignProps {
  idResign: string;
}

function _ViewDetailResign(props: ViewDetailResignProps) {
  const { idResign } = props;
  const { data: dataResign, isLoading: isLoadingResign } = useGetResignByIdQuery({ idResign }, { skip: !idResign });
  const internRequest = dataResign?.payload;
  const { data: dataUnit } = useGetUnitByIdQuery(
    {
      id: internRequest?.idUnit || ""
    },
    {
      skip: !internRequest?.idUnit
    }
  );
  const { data: dataEmployee } = useGetEmployeeByIdQuery(
    {
      idEmployee: internRequest?.idEmployee || ""
    },
    {
      skip: !internRequest?.idEmployee
    }
  );
  const { data: dataUser } = useGetUserByIdQuery(
    {
      id: internRequest?.idUserRequest || ""
    },
    {
      skip: !internRequest?.idUserRequest
    }
  );

  return (
    <div className="ViewDetailResign">
      <Spin spinning={isLoadingResign} size={"large"}>
        <Row>
          <Col span={24}>
            <Descriptions bordered layout={"vertical"}>
              <Descriptions.Item label="Mô tả" span={3}>
                {internRequest?.description}
              </Descriptions.Item>
              <Descriptions.Item label="Tên phòng ban">{internRequest?.unitName}</Descriptions.Item>
              <Descriptions.Item label="Phòng ban" span={2}>
                {dataUnit?.payload?.unitName} - {dataUnit?.payload?.unitCode}
              </Descriptions.Item>
              <Descriptions.Item label="Nhân viên">
                {dataEmployee?.payload?.name} - {dataEmployee?.payload?.code}
              </Descriptions.Item>
              <Descriptions.Item label="Ngưởi tạo">
                {dataUser?.payload?.fullname} - {dataUser?.payload?.email}
              </Descriptions.Item>
              <Descriptions.Item label="File đính kèm">
                {internRequest?.resignForm && (
                  <DownloadButton
                    downloadUrl={getFileBusinessTrip(
                      internRequest?.id + "." + internRequest?.resignForm?.split(".").pop()
                    )}
                  />
                )}
              </Descriptions.Item>
            </Descriptions>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const ViewDetailResign = WithErrorBoundaryCustom(_ViewDetailResign);
