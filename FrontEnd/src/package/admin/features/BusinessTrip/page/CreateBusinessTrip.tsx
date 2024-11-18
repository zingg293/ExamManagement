import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Card, Col, Row } from "antd";
import { NewAndUpdateBusinessTrip } from "@admin/features/BusinessTrip";
import { useNavigate } from "react-router-dom";

function _CreateBusinessTrip() {
  const navigation = useNavigate();
  return (
    <div className="CreateBusinessTrip">
      <Row>
        <Col span={24}>
          <Card title="Tạo phiếu đăng ký công tác" bordered={false}>
            <NewAndUpdateBusinessTrip AfterSave={() => navigation("/admin/MENU_DANH_SACH_DANG_KY_CONG_TAC")} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const CreateBusinessTrip = WithErrorBoundaryCustom(_CreateBusinessTrip);
