import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Card, Col, Row } from "antd";
import { NewAndUpdateOvertime } from "@admin/features/overTime";
import { useNavigate } from "react-router-dom";

function _CreateOvertime() {
  const navigation = useNavigate();
  return (
    <div className="CreateOvertime">
      <Row>
        <Col span={24}>
          <Card title="Tạo phiếu đăng ký gác thi" bordered={false}>
            <NewAndUpdateOvertime AfterSave={() => navigation("/admin/MENU_DANG_KY_LICH_TANG_CA")} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const CreateOvertime = WithErrorBoundaryCustom(_CreateOvertime);
