import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Card, Col, Row } from "antd";
import { NewAndUpdateOnLeave } from "@admin/features/onLeave";
import { useNavigate } from "react-router-dom";

function _CreateOnLeave() {
  const navigation = useNavigate();
  return (
    <div className="CreateOnLeave">
      <Row>
        <Col span={24}>
          <Card title="Tạo phiếu đăng ký ngày nghỉ" bordered={false}>
            <NewAndUpdateOnLeave AfterSave={() => navigation("/admin/MENU_DANH_SACH_DANG_KY_NGAY_NGHI")} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const CreateOnLeave = WithErrorBoundaryCustom(_CreateOnLeave);
