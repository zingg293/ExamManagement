import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Card, Col, Divider, Row, Typography } from "antd";
import { useGetListCompanyInformationQuery } from "@API/services/CompanyInformationApis.service";
import { NewAndUpdateCompanyInformation } from "@admin/features/companyInformation";

function _CompanyInformation() {
  const { data: CompanyInformation, isLoading: LoadingCompanyInformation } = useGetListCompanyInformationQuery({
    pageNumber: 1,
    pageSize: 1
  });
  return (
    <div className="CompanyInformation">
      <Row>
        <Col span={24}>
          <Card bordered={false} loading={LoadingCompanyInformation}>
            <Typography.Title level={3}>Thông tin tổng quan</Typography.Title>
            <Divider />
            <NewAndUpdateCompanyInformation setVisible={() => false} id={CompanyInformation?.listPayload?.at(0)?.id} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const CompanyInformation = WithErrorBoundaryCustom(_CompanyInformation);
