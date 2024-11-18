import { Button, Card, Col, Form, Image, Input, Layout, Row, Switch, Typography } from "antd";
import { useLocation, useNavigate } from "react-router-dom";
import { setCookie } from "~/units";
import { useState } from "react";
import { LoginOutlined, MailOutlined, UnlockOutlined } from "@ant-design/icons";
import { HandleError } from "@admin/components";
import { Illustration3 } from "@admin/asset/Illustrations";
import { useLoginMutation } from "@API/services/AuthApis.service";
import { InformationCompany } from "~/globalVariable";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";

const { Header, Footer, Content } = Layout;

function _LoginAdminLayout() {
  const location = useLocation();
  const navigate = useNavigate();
  const [result] = useState<any>(null);
  const [login, { isLoading }] = useLoginMutation();

  const onFinish = async (values: { email: string; password: string }) => {
    try {
      const result = await login({ user: values }).unwrap();
      const { from } = location.state || { from: "/admin/dashboard" };
      if (!result) return;
      if (result.success) {
        setCookie("jwt", result.payload.data, 1);
        return navigate(from);
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <Layout className="layout-default layout-signin" style={{ height: "100vh" }}>
      <Header>
        <div className="header-col header-brand" style={{ textAlign: "center" }}>
          <Typography.Title level={3} style={{ lineHeight: 3 }}>
            TRANG NTT 2002 - PHẦN MỀM QUẢN LÝ GÁC THI
          </Typography.Title>
        </div>
        {/* <div className="header-col header-nav">
            <Menu mode="horizontal">
              <Menu.Item>
                <Link to="#">
                  <span> Company</span>
                </Link>
              </Menu.Item>
              <Menu.Item>
                <Link to="#">
                  <span> About Us</span>
                </Link>
              </Menu.Item>
              <Menu.Item>
                <Link to="#">
                  <span> Teams</span>
                </Link>
              </Menu.Item>
              <Menu.Item>
                <Link to="#">
                  <span> Products</span>
                </Link>
              </Menu.Item>
              <Menu.Item>
                <Link to="#">
                  <span> Blogs</span>
                </Link>
              </Menu.Item>
              <Menu.Item>
                <Link to="#">
                  <span> Pricing</span>
                </Link>
              </Menu.Item>
            </Menu>
          </div> */}
      </Header>
      <Content className="signin">
        <Row gutter={[24, 0]} justify="space-around">
          <Col xs={{ span: 24, offset: 0 }} lg={{ span: 7, offset: 2 }} md={{ span: 12 }}>
            <Card bordered={false} className="criclebox">
              <Typography.Title level={1}>Đăng nhập</Typography.Title>
              <Typography.Title
                style={{
                  color: "#8c8c8c",
                  fontWeight: 400,
                  marginBottom: 24
                }}
                level={5}
              >
                Nhập email và mật khẩu của bạn để đăng nhập
              </Typography.Title>
              <Form onFinish={onFinish} layout="vertical" className="row-col signIn-form" size={"large"}>
                <Form.Item
                  name="email"
                  style={{ fontWeight: 600 }}
                  hasFeedback
                  validateStatus={result ? (result.result === "success" ? "success" : "warning") : undefined}
                  rules={[
                    {
                      required: true,
                      message: "Vui lòng nhập tên đăng nhập!"
                    }
                  ]}
                >
                  <Input placeholder="Email" autoComplete="off" prefix={<MailOutlined />} />
                </Form.Item>

                <Form.Item
                  name="password"
                  style={{ fontWeight: 600 }}
                  hasFeedback
                  validateStatus={result ? (result.result === "success" ? "success" : "warning") : undefined}
                  rules={[
                    {
                      required: true,
                      message: "Vui lòng nhập mật khẩu !"
                    }
                  ]}
                >
                  <Input.Password
                    type="Password"
                    placeholder="Mật khẩu"
                    prefix={<UnlockOutlined />}
                    autoComplete="off"
                  />
                </Form.Item>
                <Form.Item>
                  <Switch defaultChecked={true} />
                </Form.Item>

                <Form.Item>
                  <Button
                    type="primary"
                    htmlType="submit"
                    style={{ width: "100%" }}
                    loading={isLoading}
                    icon={<LoginOutlined />}
                  >
                    Đăng nhập
                  </Button>
                </Form.Item>
              </Form>
            </Card>
          </Col>
          <Col className="sign-img" style={{ padding: 12 }} xs={24} lg={12} md={12}>
            <Image
              src={Illustration3}
              alt="background"
              preview={false}
              width={"100%"}
              style={{
                objectFit: "cover",
                margin: "auto"
              }}
            />
          </Col>
        </Row>
      </Content>
      <Footer>
        {/* <Menu mode="horizontal">
            <Menu.Item>Company</Menu.Item>
            <Menu.Item>About Us</Menu.Item>
            <Menu.Item>Teams</Menu.Item>
            <Menu.Item>Products</Menu.Item>
            <Menu.Item>Blogs</Menu.Item>
            <Menu.Item>Pricing</Menu.Item>
          </Menu>
          <Menu mode="horizontal" className="menu-nav-social">
            <Menu.Item>
              <Link to="#">{<DribbbleOutlined />}</Link>
            </Menu.Item>
            <Menu.Item>
              <Link to="#">{<TwitterOutlined />}</Link>
            </Menu.Item>
            <Menu.Item>
              <Link to="#">{<InstagramOutlined />}</Link>
            </Menu.Item>
            <Menu.Item>
              <Link to="#">
                <svg
                  width="18"
                  height="18"
                  xmlns="http://www.w3.org/2000/svg"
                  viewBox="0 0 512 512"
                >
                  <path d="M496 256c0 137-111 248-248 248-25.6 0-50.2-3.9-73.4-11.1 10.1-16.5 25.2-43.5 30.8-65 3-11.6 15.4-59 15.4-59 8.1 15.4 31.7 28.5 56.8 28.5 74.8 0 128.7-68.8 128.7-154.3 0-81.9-66.9-143.2-152.9-143.2-107 0-163.9 71.8-163.9 150.1 0 36.4 19.4 81.7 50.3 96.1 4.7 2.2 7.2 1.2 8.3-3.3.8-3.4 5-20.3 6.9-28.1.6-2.5.3-4.7-1.7-7.1-10.1-12.5-18.3-35.3-18.3-56.6 0-54.7 41.4-107.6 112-107.6 60.9 0 103.6 41.5 103.6 100.9 0 67.1-33.9 113.6-78 113.6-24.3 0-42.6-20.1-36.7-44.8 7-29.5 20.5-61.3 20.5-82.6 0-19-10.2-34.9-31.4-34.9-24.9 0-44.9 25.7-44.9 60.2 0 22 7.4 36.8 7.4 36.8s-24.5 103.8-29 123.2c-5 21.4-3 51.6-.9 71.2C65.4 450.9 0 361.1 0 256 0 119 111 8 248 8s248 111 248 248z"></path>
                </svg>
              </Link>
            </Menu.Item>
            <Menu.Item>
              <Link to="#">{<GithubOutlined />}</Link>
            </Menu.Item>
          </Menu> */}
        <p className="copyright">
          Copyright {InformationCompany.yearCopyRight} by <span>the company {InformationCompany.englishName}</span>.{" "}
        </p>
      </Footer>
    </Layout>
  );
}

export const LoginAdminLayout = WithErrorBoundaryCustom(_LoginAdminLayout);
