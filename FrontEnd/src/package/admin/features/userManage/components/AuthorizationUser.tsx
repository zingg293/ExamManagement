import { DataNavigate } from "@admin/components/sideNav/DataNavigate";
import { Button, Card, Checkbox, Col, Descriptions, Form, Row, Space, Spin, Typography } from "antd";
import { dashboard, desk } from "@admin/components/sideNav/svg";
import { CheckCircleOutlined, RollbackOutlined } from "@ant-design/icons";
import { useEffect, useState } from "react";
import {
  useAddListRoleUserMutation,
  useGetAllRoleByIdUserCurrentQuery,
  useGetAllRoleQuery,
  useGetUserByIdQuery
} from "@API/services/UserApis.service";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";

interface IProps {
  id?: string;
  setVisible: (value: boolean) => void;
}

function _AuthorizationUser(props: IProps) {
  const { id, setVisible } = props;
  const { data: listRoleUser, isLoading: LoadingListRoleUser } = useGetAllRoleByIdUserCurrentQuery(
    {
      IdUser: id!,
      pageNumber: 0,
      pageSize: 0
    },
    { skip: !id }
  );
  const { data: dataUser, isLoading: loadingDataUser } = useGetUserByIdQuery({ id: id }, { skip: id === "" });
  const { data: listRole, isLoading: LoadingListRole } = useGetAllRoleQuery(
    {
      pageNumber: 0,
      pageSize: 0
    },
    { skip: id === "" }
  );
  const [AddListRoleUser, { isLoading: LoadingAddListRoleUser }] = useAddListRoleUserMutation();
  const data = DataNavigate();
  const [form] = Form.useForm();
  const [postLength, setPostLength] = useState(0);
  useEffect(() => {
    if (listRoleUser?.listPayload) {
      const ListRole = form.getFieldsValue();
      const listRoleNameUser = listRoleUser?.listPayload.map((item) => item.roleName);
      // loop and set value is true
      const listRoleName = Object.keys(ListRole).map((item) => {
        if (listRoleNameUser.includes(item)) {
          return { [item]: true };
        }
        return { [item]: false };
      });
      form.setFieldsValue(Object.assign({}, ...listRoleName));
    }
  }, [listRoleUser?.listPayload, form, postLength]);
  const handleAddRoleForUser = async (values: any) => {
    try {
      const listRoleNameIsTrue = Object.keys(values).filter((item) => values[item] === true);
      const filterGetListRole = listRole?.listPayload?.filter((item) => listRoleNameIsTrue.includes(item.roleName));
      const filterGetIdRole = filterGetListRole?.map((item) => item.id);
      const res = await AddListRoleUser({
        IdUser: id!,
        listRole: filterGetIdRole || []
      }).unwrap();
      if (res) {
        setVisible(false);
        form.resetFields();
      }
    } catch (e) {
      console.log(e);
    }
  };
  const onCheckAllChange = (e: any) => {
    const getFieldsValue = form.getFieldsValue();
    const arrValue = Object.keys(getFieldsValue).map((key) => {
      return { [key]: e.target.checked };
    });
    const arrToObject = arrValue.reduce((acc, cur) => {
      const key = Object.keys(cur)[0];
      acc[key] = cur[key];
      return acc;
    }, {});
    form.setFieldsValue(arrToObject);
  };
  const onCheckByGroupRole = (e: any, ListGroupRole: string[]) => {
    const getFieldsValue = form.getFieldsValue();
    const arrValue = Object.keys(getFieldsValue).map((key) => {
      if (ListGroupRole.includes(key)) {
        return { [key]: e.target.checked };
      }
      return { [key]: getFieldsValue[key] };
    });
    const arrToObject = arrValue.reduce((acc, cur) => {
      const key = Object.keys(cur)[0];
      acc[key] = cur[key];
      return acc;
    }, {});
    form.setFieldsValue(arrToObject);
  };

  return (
    <div className="AuthorizationUser">
      <Spin spinning={loadingDataUser}>
        <Row style={{ marginBottom: 24 }}>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Descriptions
              title={
                <Typography.Title level={4} style={{ marginBottom: 0 }}>
                  Thông tin người dùng
                </Typography.Title>
              }
              bordered
            >
              <Descriptions.Item label="Họ và tên" span={3}>
                <Typography.Text>{dataUser?.payload?.fullname}</Typography.Text>
              </Descriptions.Item>
              <Descriptions.Item label="Mã" span={3}>
                <Typography.Text>{dataUser?.payload?.userCode}</Typography.Text>
              </Descriptions.Item>
              <Descriptions.Item label="Email">
                <Typography.Text>{dataUser?.payload?.email}</Typography.Text>
              </Descriptions.Item>
              <Descriptions.Item label="Số điện thoại">
                <Typography.Text>{dataUser?.payload?.phone}</Typography.Text>
              </Descriptions.Item>
            </Descriptions>
          </Col>
        </Row>
      </Spin>
      <Spin spinning={LoadingListRoleUser || LoadingListRole}>
        <Form layout={"vertical"} form={form} onFinish={handleAddRoleForUser} preserve={false}>
          <Row gutter={[24, 24]}>
            <Col xs={24} sm={24} md={24} lg={24} xl={24}>
              <Space size={"large"} wrap>
                <Card key={"Chọn tất Cả"} bordered={true} className="criclebox">
                  <Form.Item name={"Chọn tất Cả"} valuePropName="checked" style={{ marginBottom: 0 }}>
                    <Checkbox defaultChecked={false} onChange={onCheckAllChange}>
                      <Space wrap={true}>
                        <span className="anticon anticon-code icon ant-menu-item-icon">
                          {
                            <CheckCircleOutlined
                              style={{
                                fontSize: 20,
                                color: "blue"
                              }}
                            />
                          }
                        </span>
                        <Typography.Text strong style={{ color: "blue" }}>
                          Chọn tất Cả
                        </Typography.Text>
                      </Space>
                    </Checkbox>
                  </Form.Item>
                </Card>
                <Card key={"Trang chủ"} bordered={true} className="criclebox">
                  <Form.Item name={"Trang chủ"} valuePropName="checked" style={{ marginBottom: 0 }}>
                    <Checkbox>
                      <Space>
                        <span className="anticon anticon-code icon ant-menu-item-icon">{dashboard("green")}</span>
                        <Typography.Text strong>Trang chủ</Typography.Text>
                      </Space>
                    </Checkbox>
                  </Form.Item>
                </Card>
                <Card key={"Bàn làm việc"} bordered={true} className="criclebox">
                  <Form.Item name={"Bàn làm việc"} valuePropName="checked" style={{ marginBottom: 0 }}>
                    <Checkbox>
                      <Space>
                        <span className="anticon anticon-code icon ant-menu-item-icon">{desk("green")}</span>
                        <Typography.Text strong>Bàn làm việc</Typography.Text>
                      </Space>
                    </Checkbox>
                  </Form.Item>
                </Card>
              </Space>
            </Col>
            {data.map((item: any) => (
              <>
                <Col key={item.key} xs={24} sm={24} md={24} lg={12} xl={12}>
                  <Card bordered={true} className="criclebox">
                    <Checkbox
                      defaultChecked={false}
                      onChange={(e) =>
                        onCheckByGroupRole(
                          e,
                          item.children.map((x: any) => x.name)
                        )
                      }
                    >
                      <Typography.Title level={3} className="color-52-71-103">
                        {item.title}
                      </Typography.Title>
                    </Checkbox>
                    <Space style={{ width: "100%", padding: 10 }} size={"large"} wrap>
                      {item.children.map((child: any) => (
                        <Card key={child.key} bordered={true} className="criclebox">
                          <Form.Item
                            name={child.name}
                            valuePropName="checked"
                            style={{
                              marginBottom: 0
                            }}
                          >
                            <Checkbox>
                              <Space wrap>
                                <span className="anticon anticon-code icon ant-menu-item-icon">{child?.icon}</span>
                                <Typography.Text strong>{child?.name}</Typography.Text>
                              </Space>
                            </Checkbox>
                          </Form.Item>
                        </Card>
                      ))}
                    </Space>
                  </Card>
                </Col>
              </>
            ))}
          </Row>
          <Form.Item>
            <Space style={{ width: "100%", justifyContent: "end", marginTop: 24 }}>
              <Button
                loading={LoadingListRoleUser}
                icon={<RollbackOutlined />}
                onClick={() => setPostLength((prevState) => prevState + 1)}
              >
                Khôi phục bạn đầu
              </Button>
              <Button loading={LoadingAddListRoleUser} type="primary" htmlType="submit" icon={<CheckCircleOutlined />}>
                Lưu
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Spin>
    </div>
  );
}

export const AuthorizationUser = WithErrorBoundaryCustom(_AuthorizationUser);
