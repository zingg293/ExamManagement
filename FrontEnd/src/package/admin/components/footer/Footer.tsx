import { Layout, Row, Col } from "antd";
import { HeartFilled } from "@ant-design/icons";
import { InformationCompany } from "~/globalVariable";

export function Footer() {
  const { Footer: AntFooter } = Layout;

  return (
    <AntFooter>
      <Row className="just">
        <Col xs={24} md={12} lg={12}>
          <div className="copyright">
            {InformationCompany.yearCopyRight}, made with
            {<HeartFilled />} by
            <span
              style={{
                color: JSON.parse(localStorage.getItem("setting") || "")?.PrimaryColor,
                margin: "0px 5px",
                fontWeight: 700
              }}
            >
              {InformationCompany.englishName}
            </span>
            for a better web.
          </div>
        </Col>
        <Col xs={24} md={12} lg={12}>
          <div className="footer-menu">
            <ul>
              {/* <li className="nav-item">
                <a
                  href="#pablo"
                  className="nav-link text-muted"
                  target="_blank"
                >
                  NOTE
                </a>
              </li>
              <li className="nav-item">
                <a
                  href="#pablo"
                  className="nav-link text-muted"
                  target="_blank"
                >
                  About Us
                </a>
              </li>
              <li className="nav-item">
                <a
                  href="#pablo"
                  className="nav-link text-muted"
                  target="_blank"
                >
                  Blog
                </a>
              </li>
              <li className="nav-item">
                <a
                  href="#pablo"
                  className="nav-link pe-0 text-muted"
                  target="_blank"
                >
                  License
                </a>
              </li> */}
            </ul>
          </div>
        </Col>
      </Row>
    </AntFooter>
  );
}
