import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Col, Descriptions, Row, Spin } from "antd";
import { getFileRequestToHired, useGetRequestToHiredByIdQuery } from "@API/services/RequestToHiredApis.service";
import { DownloadButton } from "@admin/components";
import { useGetUnitByIdQuery } from "@API/services/UnitApis.service";
import { useGetUserByIdQuery } from "@API/services/UserApis.service";
import { useGetCategoryVacanciesByIdQuery } from "@API/services/CategoryVacanciesApis.service";

interface IDetailRequestToHiredProps {
  idRequestToHired: string;
}

function _DetailRequestToHired(props: IDetailRequestToHiredProps) {
  const { idRequestToHired } = props;
  const { data: dataRequestToHired, isLoading: isLoadingRequestToHired } = useGetRequestToHiredByIdQuery(
    {
      idRequestToHired: idRequestToHired
    },
    { skip: !idRequestToHired }
  );
  const requestToHired = dataRequestToHired?.payload;
  const { data: dataUnit } = useGetUnitByIdQuery(
    {
      id: requestToHired?.idUnit || ""
    },
    {
      skip: !requestToHired?.idUnit
    }
  );
  const { data: dataUser } = useGetUserByIdQuery(
    { id: requestToHired?.createdBy || "" },
    { skip: !requestToHired?.createdBy }
  );
  const { data: dataCategoryVacancies } = useGetCategoryVacanciesByIdQuery(
    { idCategoryVacancies: requestToHired?.idCategoryVacancies || "" },
    { skip: !requestToHired?.idCategoryVacancies }
  );

  return (
    <div className="DetailRequestToHired">
      <Spin spinning={isLoadingRequestToHired} size={"large"}>
        <Row>
          <Col span={24}>
            <Descriptions bordered layout={"vertical"}>
              <Descriptions.Item label="Lý do" span={3}>
                {requestToHired?.reason}
              </Descriptions.Item>
              <Descriptions.Item label="Số lượng">{requestToHired?.quantity}</Descriptions.Item>
              <Descriptions.Item label="Vị trí">{dataCategoryVacancies?.payload?.positionName}</Descriptions.Item>
              <Descriptions.Item label="Bằng cấp">{dataCategoryVacancies?.payload?.degree}</Descriptions.Item>
              <Descriptions.Item label="Kinh nghiệm">{dataCategoryVacancies?.payload?.numExp}</Descriptions.Item>
              <Descriptions.Item label="Phòng ban">
                {dataUnit?.payload?.unitName} - {dataUnit?.payload?.unitCode}
              </Descriptions.Item>
              <Descriptions.Item label="Ngưởi tạo">{dataUser?.payload?.fullname}</Descriptions.Item>
              <Descriptions.Item label="File đính kèm">
                {requestToHired?.filePath && (
                  <DownloadButton
                    downloadUrl={getFileRequestToHired(
                      requestToHired.id + "." + requestToHired?.filePath?.split(".")?.at(1)
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

export const DetailRequestToHired = WithErrorBoundaryCustom(_DetailRequestToHired);
