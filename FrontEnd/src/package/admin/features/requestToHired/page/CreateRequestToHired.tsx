import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { NewAndUpdateRequestToHired } from "@admin/features/requestToHired";
import { Card, Col, Row } from "antd";
import { useNavigate } from "react-router-dom";

function _CreateRequestToHired() {
  const navigation = useNavigate();
  return (
    <div className="CreateRequestToHired">
      <Row>
        <Col span={24}>
          <Card title="Tạo phiếu yêu cầu tuyển dụng" bordered={false}>
            <NewAndUpdateRequestToHired setVisible={() => navigation("/admin/MENU_YEU_CAU_TUYEN_DUNG")} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const CreateRequestToHired = WithErrorBoundaryCustom(_CreateRequestToHired);
