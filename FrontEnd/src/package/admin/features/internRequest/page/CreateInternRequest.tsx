import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Card, Col, Row } from "antd";
import { NewAndUpdateInternRequest } from "@admin/features/internRequest";
import { useNavigate } from "react-router-dom";

function _CreateInternRequest() {
  const navigation = useNavigate();
  return (
    <div className="CreateInternRequest">
      <Row>
        <Col span={24}>
          <Card title="Tạo phiếu yêu cầu thử việc" bordered={false}>
            <NewAndUpdateInternRequest AfterSave={() => navigation("/admin/MENU_YEU_CAU_THUC_TAP")} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}
export const CreateInternRequest = WithErrorBoundaryCustom(_CreateInternRequest);
