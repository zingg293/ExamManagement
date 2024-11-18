import { Card, Col, Row, Typography } from "antd";
import { InformationCompany } from "~/globalVariable";
import { newLogoDPD } from "@admin/asset/logo";

export function CompanyIntroduction() {
  const { Title, Text, Paragraph } = Typography;
  return (
    <Row gutter={[24, 0]}>
      <Col xs={24} md={12} sm={24} lg={12} xl={14} className="mb-24">
        <Card bordered={false} className="criclebox h-full">
          <Row>
            <Col xs={24} md={12} sm={24} lg={12} xl={14} className="mobile-24">
              <div className="h-full col-content p-20">
                <div className="ant-muse">
                  <p>CHÀO MỪNG QUÝ GIẢNG VIÊN ĐẾN VỚI </p>
                  <p>QUẢN LÝ LỊCH GÁC THI  </p>
                  <Text>Được xây dựng bởi </Text>
                  <Title level={5}>{InformationCompany.name}</Title>
                  <Paragraph className="lastweek mb-36">
                    D20CNTT01
                  </Paragraph>
                </div>
                <div className="card-footer">
                  <a className="icon-move-right" href="#pablo">
                    {/* Read More */}
                    {/* {<RightOutlined />} */}
                  </a>
                </div>
              </div>
            </Col>
            <Col
              xs={24}
              md={12}
              sm={24}
              lg={12}
              xl={10}
              className="col-img"
              style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center"
              }}
            >
              <div className="ant-cret text-right">
                <img
                  src={newLogoDPD}
                  alt=""
                  className="border10"
                  style={{ objectFit: "cover", width: 150, height: 108 }}
                />
              </div>
            </Col>
          </Row>
        </Card>
      </Col>
      <Col xs={24} md={12} sm={24} lg={12} xl={10} className="mb-24">
        <Card bordered={false} className="criclebox card-info-2 h-full">
          <div className="gradent h-full col-content">
            <div className="card-content">
              <Title level={5}>Làm việc với những gì tốt nhất</Title>
              <p>Sáng tạo, Trách nhiệm, Kỉ luật</p>
            </div>
            {/*<div className="card-footer">*/}
            {/*  <a className="icon-move-right">*/}
            {/*     Read More */}
            {/*     <RightOutlined /> */}
            {/*  </a>*/}
            {/*</div>*/}
          </div>
        </Card>
      </Col>
    </Row>
  );
}
