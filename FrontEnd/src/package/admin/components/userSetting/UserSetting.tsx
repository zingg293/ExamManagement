import {
  AppstoreOutlined,
  FileSearchOutlined,
  RestOutlined,
  SafetyCertificateOutlined,
  ToolOutlined,
  UserOutlined
} from "@ant-design/icons";
import { Anchor, Avatar, Card, Col, Divider, Image, Row, Space, Spin, Typography } from "antd";
import bg from "src/styles/images/bg-profile.jpg";
import "./style.css";
import { useGetUserQuery } from "@API/services/UserApis.service";
import { useBreakPoint } from "@hooks/useBreakpoint";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { BasicInfo } from "@admin/components/userSetting/features/BasicInfo";
import { ChangePassword } from "@admin/components/userSetting/features/ChangePassword";
import { DeleteAccount } from "@admin/components/userSetting/features/DeleteAccount";

function _UserSetting() {
  const breakpoint = useBreakPoint();
  const { data: user, isLoading } = useGetUserQuery({ fetch: false });
  return (
    <Spin spinning={isLoading} tip="Loading...">
      <div className="UserSetting">
        <Image height={300} src={bg} width={"100%"} preview={false} style={{ objectFit: "cover", borderRadius: 12 }} />
        <Row gutter={[24, 0]}>
          {breakpoint.isDesktop && (
            <Col xs={24} sm={24} md={24} lg={6} xl={6} className="mb-24">
              <Anchor targetOffset={window.innerHeight / 4}>
                <Card bordered={false} className="criclebox h-full">
                  <Space direction="vertical" className="w-full">
                    <Anchor.Link
                      href="#Profile"
                      title={
                        <Space>
                          <AppstoreOutlined /> Profile
                        </Space>
                      }
                      className="anchor-link"
                    />
                    <Anchor.Link
                      href="#Basic-infor"
                      title={
                        <Space>
                          <FileSearchOutlined /> Thông tin cơ bản
                        </Space>
                      }
                      className="anchor-link"
                    />
                    <Anchor.Link
                      href="#Change-password"
                      title={
                        <Space>
                          <ToolOutlined />
                          Thay đổi mật khẩu
                        </Space>
                      }
                      className="anchor-link"
                    />
                    {/* <Anchor.Link
                      href="#2FA"
                      title={
                        <Space>
                          <SafetyCertificateOutlined /> 2FA
                        </Space>
                      }
                      className="anchor-link"
                    /> */}
                    <Anchor.Link
                      href="#Delete-account"
                      title={
                        <Space>
                          <RestOutlined /> Xóa tài khoản
                        </Space>
                      }
                      className="anchor-link"
                    />
                  </Space>
                </Card>
              </Anchor>
            </Col>
          )}
          <Col xs={24} sm={24} md={24} lg={18} xl={18} className="mb-24">
            <Row gutter={[24, 0]}>
              <Col
                xs={24}
                sm={24}
                md={24}
                lg={24}
                xl={24}
                className="mb-24"
                id="Profile"
                style={{ marginTop: "-60px" }}
              >
                <Card bordered={false} className="criclebox h-full">
                  <Space direction="horizontal" align="start" size={"large"}>
                    <Avatar shape="square" size={74} icon={<UserOutlined />} />
                    <div className="UserSetting-info">
                      <Typography.Title level={4}>{user?.payload?.data.fullname}</Typography.Title>
                      <Typography.Text type="secondary">{user?.payload?.data.email}</Typography.Text>
                    </div>
                  </Space>
                </Card>
              </Col>
              <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24" id="Basic-infor">
                <BasicInfo />
              </Col>
              <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24" id="Change-password">
                <ChangePassword email={user?.payload?.data.email} />
              </Col>
              {/* <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24" id="2FA">
                <Card bordered={false} className="criclebox h-full">
                  <Typography.Title level={4}>Two-factor authentication</Typography.Title>
                  <Divider />
                  <Typography.Text type="secondary">
                    Xác thực hai yếu tố (2FA) thêm một lớp bảo mật bổ sung vào tài khoản của bạn bằng cách yêu cầu nhiều
                    hơn một mật khẩu để đăng nhập. Khi 2FA được bật, bạn sẽ được nhắc nhập mã ngoài tên người dùng và
                    mật khẩu khi đăng nhập .
                  </Typography.Text>
                  <Typography.Title level={4} className="mt-24">
                    Đang cập nhật.
                  </Typography.Title>
                </Card>
              </Col> */}

              <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24" id="Delete-account">
                <DeleteAccount idUser={user?.payload?.data.id} />
              </Col>
            </Row>
          </Col>
        </Row>
      </div>
    </Spin>
  );
}

export const UserSetting = WithErrorBoundaryCustom(_UserSetting);
