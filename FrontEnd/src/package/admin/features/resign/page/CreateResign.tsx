import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Card, Col, Row } from "antd";
import { NewAndUpdateResign } from "@admin/features/resign";
import { useNavigate } from "react-router-dom";

function _CreateResign() {
  const navigation = useNavigate();
  return (
    <div className="CreateResign">
      <Row>
        <Col span={24}>
          <Card title="Tạo phiếu xin thôi việc" bordered={false}>
            <NewAndUpdateResign AfterSave={() => navigation("/admin/MENU_DANH_SACH_DON_XIN_THOI_VIEC")} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const CreateResign = WithErrorBoundaryCustom(_CreateResign);
