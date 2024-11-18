import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Col, Descriptions, Row, Spin, Tag } from "antd";
import { getFileOnLeave, useGetOnLeaveByIdQuery } from "@API/services/OnLeaveApis.service";
import { useGetUnitByIdQuery } from "@API/services/UnitApis.service";
import { useGetEmployeeByIdQuery } from "@API/services/Employee.service";
import { useGetUserByIdQuery } from "@API/services/UserApis.service";
import { DownloadButton } from "@admin/components";
import dayjs from "dayjs";

interface ViewDetailOnLeaveProps {
  idOnLeave: string;
}

function _ViewDetailOnLeave(props: ViewDetailOnLeaveProps) {
  const { idOnLeave } = props;
  const { data: dataOnLeave, isLoading: isLoadingOnLeave } = useGetOnLeaveByIdQuery(
    { idOnLeave },
    { skip: !idOnLeave }
  );
  const internRequest = dataOnLeave?.payload;
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
    <div className="ViewDetailOnLeave">
      <Spin spinning={isLoadingOnLeave} size={"large"}>
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
                {dataOnLeave?.payload?.attachments && (
                  <DownloadButton
                    downloadUrl={getFileOnLeave(
                      dataOnLeave?.payload.id + "." + dataOnLeave?.payload?.attachments?.split(".")?.at(1)
                    )}
                  />
                )}
              </Descriptions.Item>
              <Descriptions.Item label="Thời gian">
                <Tag color={"green-inverse"}>{dayjs(internRequest?.fromDate).format("DD/MM/YYYY hh:ss")}</Tag> {" => "}
                <Tag color={"red-inverse"}>{dayjs(internRequest?.toDate).format("DD/MM/YYYY hh:ss")}</Tag>
              </Descriptions.Item>
              <Descriptions.Item label={"Nghỉ phép"}>
                {dataOnLeave?.payload?.unPaidLeave ? (
                  <Tag color={"green-inverse"}>Nghỉ có lương</Tag>
                ) : (
                  <Tag color={"red-inverse"}>Nghỉ không lương</Tag>
                )}
              </Descriptions.Item>
            </Descriptions>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const ViewDetailOnLeave = WithErrorBoundaryCustom(_ViewDetailOnLeave);
