import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { NewAndUpdatePromotionTransfer } from "@admin/features/promotionTransfer";
import { Card, Col, Row } from "antd";
import { useNavigate } from "react-router-dom";

function _CreatePromotionTransfer() {
  const navigation = useNavigate();
  return (
    <div className="CreatePromotionTransfer">
      <Row>
        <Col span={24}>
          <Card title="Tạo phiếu bổ nhiệm và điều chuyển" bordered={false}>
            <NewAndUpdatePromotionTransfer
              setVisible={() => navigation("/admin/MENU_DANH_SACH_BO_NHIEM_DIEU_CHUYEN")}
            />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const CreatePromotionTransfer = WithErrorBoundaryCustom(_CreatePromotionTransfer);
