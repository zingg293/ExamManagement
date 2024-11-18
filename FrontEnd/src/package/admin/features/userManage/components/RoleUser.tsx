import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useGetUserAndRoleQuery, useInsertListRoleUserMutation } from "@API/services/UserApis.service";
import { Button, Col, Form, Row, Select, Space, Spin } from "antd";
import { useGetListRoleQuery } from "@API/services/Role.service";
import { HandleError } from "@admin/components";
import { useEffect } from "react";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _RoleUser(props: IProps) {
  const { setVisible, id } = props;
  const { data: ListRoleByUser, isLoading: isLoadingListRoleByUser } = useGetUserAndRoleQuery(
    { id: id! },
    { skip: !id }
  );
  const { data: ListRole, isLoading: isLoadingListRole } = useGetListRoleQuery({ pageSize: 0, pageNumber: 0 });
  const [insertListRoleUser, { isLoading: isLoadingInsertListRoleUser }] = useInsertListRoleUserMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    if (ListRoleByUser?.payload && id) {
      formRef.setFieldsValue({
        idUser: id,
        idsRole: ListRoleByUser?.payload?.roles?.map((unit) => unit.id)
      });
    } else {
      formRef.resetFields();
    }
  }, [ListRoleByUser?.payload, formRef, id]);
  const onfinish = async (values: any) => {
    try {
      const result = await insertListRoleUser({
        idUser: values.idUser,
        idsRole: values.idsRole
      }).unwrap();

      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="RoleUser">
      <Spin spinning={isLoadingListRoleByUser}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"idUser"} hidden />
              <Form.Item label="Quyền" name={"idsRole"}>
                <Select
                  mode={"multiple"}
                  loading={isLoadingListRole}
                  options={ListRole?.listPayload?.map((unit) => {
                    return {
                      label: unit.roleName,
                      value: unit.id
                    };
                  })}
                  showSearch
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item>
                <Space
                  style={{
                    width: "100%",
                    justifyContent: "flex-end"
                  }}
                >
                  <Button
                    type="default"
                    htmlType="reset"
                    loading={isLoadingInsertListRoleUser}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={isLoadingInsertListRoleUser}
                    icon={<CheckCircleOutlined />}
                    style={{
                      float: "right"
                    }}
                  >
                    Lưu
                  </Button>
                </Space>
              </Form.Item>
            </Form>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const RoleUser = WithErrorBoundaryCustom(_RoleUser);
