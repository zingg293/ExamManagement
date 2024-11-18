import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Col, Descriptions, Row, Spin } from "antd";
import { getFileInternRequest, useGetInternRequestByIdQuery } from "@API/services/InternRequestApis.service";
import { useGetUnitByIdQuery } from "@API/services/UnitApis.service";
import { useGetCategoryPositionByIdQuery } from "@API/services/CategoryPositionApis.service";
import { useGetEmployeeByIdQuery } from "@API/services/Employee.service";
import { DownloadButton } from "@admin/components";

interface ViewDetailInternRequestProps {
  idInternRequest: string;
}

function _ViewDetailInternRequest(props: ViewDetailInternRequestProps) {
  const { idInternRequest } = props;
  const { data: dataInternRequest, isLoading: isLoadingInternRequest } = useGetInternRequestByIdQuery(
    { idInternRequest },
    { skip: !idInternRequest }
  );
  const internRequest = dataInternRequest?.payload;
  const { data: dataUnit } = useGetUnitByIdQuery(
    {
      id: internRequest?.idUnit || ""
    },
    {
      skip: !internRequest?.idUnit
    }
  );
  const { data: dataCategoryPosition } = useGetCategoryPositionByIdQuery(
    {
      idCategoryPosition: internRequest?.idPosition || ""
    },
    {
      skip: !internRequest?.idPosition
    }
  );
  // employee
  const { data: dataEmployee } = useGetEmployeeByIdQuery(
    {
      idEmployee: internRequest?.idEmployee || ""
    },
    {
      skip: !internRequest?.idEmployee
    }
  );

  return (
    <div className="ViewDetailInternRequest">
      <Spin spinning={isLoadingInternRequest} size={"large"}>
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
              <Descriptions.Item label="Tên vị trí">{internRequest?.positionName}</Descriptions.Item>
              <Descriptions.Item label="Vị trí" span={2}>
                {dataCategoryPosition?.payload?.positionName}
              </Descriptions.Item>
              <Descriptions.Item label="Nhân viên">
                {dataEmployee?.payload?.name} - {dataEmployee?.payload?.code}
              </Descriptions.Item>
              <Descriptions.Item label="File đính kèm">
                {internRequest?.attachments && (
                  <DownloadButton
                    downloadUrl={getFileInternRequest(
                      internRequest?.id + "." + internRequest?.attachments?.split(".").pop()
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

export const ViewDetailInternRequest = WithErrorBoundaryCustom(_ViewDetailInternRequest);
