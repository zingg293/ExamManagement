import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Col, Descriptions, Row, Spin } from "antd";
import { getFileCandidate, useGetCandidateByIdQuery } from "@API/services/CandidateApis.service";
import { useGetCategoryDistrictByIdQuery } from "@API/services/CategorryDistrict.service";
import { useGetCategoryWardByIdQuery } from "@API/services/CategoryWardApis.service";
import { useGetCategoryCityByIdQuery } from "@API/services/CategoryCity.service";
import dayjs from "dayjs";
import { DownloadButton } from "@admin/components";

interface DetailCandidateProps {
  idCandidate: string;
}

function _DetailCandidate(props: DetailCandidateProps) {
  const { idCandidate } = props;
  const { data: dataCandidate, isLoading: isLoadingCandidate } = useGetCandidateByIdQuery(
    { idCandidate },
    { skip: !idCandidate }
  );
  const detailCandidate = dataCandidate?.payload;
  const { data: dataCategoryDistrict } = useGetCategoryDistrictByIdQuery(detailCandidate?.idDistrict || "", {
    skip: !detailCandidate?.idDistrict
  });
  const { data: dataCategoryWard } = useGetCategoryWardByIdQuery(
    {
      idCategoryWard: detailCandidate?.idWard || ""
    },
    {
      skip: !detailCandidate?.idWard
    }
  );
  const { data: dataCategoryCity } = useGetCategoryCityByIdQuery(
    {
      idCategoryCity: detailCandidate?.idCity || ""
    },
    {
      skip: !detailCandidate?.idCity
    }
  );

  return (
    <div className="DetailCandidate">
      <Spin spinning={isLoadingCandidate} size={"large"}>
        <Row>
          <Col span={24}>
            <Descriptions bordered layout={"vertical"}>
              <Descriptions.Item label="Họ và tên">{detailCandidate?.name}</Descriptions.Item>
              <Descriptions.Item label="Ngày sinh">
                {dayjs(detailCandidate?.birthday).format("DD-MM-YYYY")}
              </Descriptions.Item>
              <Descriptions.Item label="Giới tính">{detailCandidate?.sex ? "nam" : "nữ"}</Descriptions.Item>
              <Descriptions.Item label="Email">{detailCandidate?.email}</Descriptions.Item>
              <Descriptions.Item label="Phone" span={2}>
                {detailCandidate?.phone}
              </Descriptions.Item>

              <Descriptions.Item label="Địa chỉ" span={3}>
                {detailCandidate?.address}
              </Descriptions.Item>
              <Descriptions.Item label="Xã">{dataCategoryWard?.payload?.wardName}</Descriptions.Item>
              <Descriptions.Item label="Huyện">{dataCategoryDistrict?.payload?.districtName}</Descriptions.Item>
              <Descriptions.Item label="Tỉnh">{dataCategoryCity?.payload?.cityName}</Descriptions.Item>
              <Descriptions.Item label="Ghi chú" span={3}>
                {detailCandidate?.note}
              </Descriptions.Item>
              <Descriptions.Item label="File đính kèm" span={3}>
                {detailCandidate?.file && (
                  <DownloadButton
                    downloadUrl={getFileCandidate(detailCandidate.id + "." + detailCandidate?.file?.split(".")?.at(1))}
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

export const DetailCandidate = WithErrorBoundaryCustom(_DetailCandidate);
