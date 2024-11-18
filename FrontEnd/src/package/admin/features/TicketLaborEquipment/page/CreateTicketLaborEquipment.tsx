import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useNavigate } from "react-router-dom";
import { Card, Col, Row } from "antd";
import { NewAndUpdateTicketLaborEquipment } from "@admin/features/TicketLaborEquipment";

function _CreateTicketLaborEquipment() {
  const navigation = useNavigate();
  return (
    <div className="CreateTicketLaborEquipment">
      <Row>
        <Col span={24}>
          <Card title="Tạo phiếu trang thiết bị lao động" bordered={false}>
            <NewAndUpdateTicketLaborEquipment
              setVisible={() => navigation("/admin/MENU_PHIEU_YEU_CAU_TRANG_THIET_BI_LAO_DONG")}
            />
          </Card>
        </Col>
      </Row>
    </div>
  );
}
export const CreateTicketLaborEquipment = WithErrorBoundaryCustom(_CreateTicketLaborEquipment);
