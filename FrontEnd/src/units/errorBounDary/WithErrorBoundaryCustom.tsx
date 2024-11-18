import { ToolOutlined } from "@ant-design/icons";
import { Button, Card, Col, Image, Row, Space, Typography } from "antd";
import React from "react";
import { withErrorBoundary } from "react-error-boundary";
import { useNavigate } from "react-router-dom";
import { RandomIllustration } from "@admin/asset/Illustrations";

interface ErrorBoundaryProps {
  error: Error;
}

function ErrorFallback({ error }: ErrorBoundaryProps): JSX.Element {
  const navigate = useNavigate();

  const handleRepair = () => {
    navigate(-1);
    setTimeout(() => navigate(1), 100);
  };

  return (
    <Row>
      <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
        <Card bordered={false} className="criclebox h-full">
          <Row gutter={16}>
            <Col xs={24} sm={24} md={24} lg={12} xl={12}>
              <Space direction="vertical" style={{ width: "100%" }} align="center">
                <Typography.Title level={3}>Oops! Có gì đó không ổn.</Typography.Title>
                <Typography.Text strong>Đừng lo lắng, chúng tôi sẽ xử lý nhanh nhất cho bạn.</Typography.Text>
                <Typography.Paragraph type="danger">{error.message}</Typography.Paragraph>
                <Button icon={<ToolOutlined />} onClick={handleRepair} type="primary">
                  Sửa chữa
                </Button>
              </Space>
            </Col>
            <Col xs={24} sm={24} md={24} lg={12} xl={12}>
              <Image preview={false} src={RandomIllustration()} width={"100%"} />
            </Col>
          </Row>
        </Card>
      </Col>
    </Row>
  );
}

/**
 * HOC WithErrorBoundaryCustom
 * @param {React.FC} component
 * @returns {React.FC}
 */
export default function WithErrorBoundaryCustom<T>(component: React.FC<T>): React.FC<T> {
  return withErrorBoundary(component, {
    FallbackComponent: ErrorFallback
  });
}
