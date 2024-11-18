import { Button, Col, Form, Input, Row, Space, Spin, Tag, TreeSelect } from "antd";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect } from "react";
import { useGetRoleByIdQuery, useInsertRoleMutation, useUpdateRoleMutation } from "@API/services/Role.service";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useGetListNavigationQuery } from "@API/services/Navigation.service";
import { Navigation } from "@models/navigationDTO";

export const renderTreeNodes = (nodes: Navigation[]): any => {
  return nodes.map((node) => {
    if (node.children && node.children.length > 0) {
      return {
        title: <Tag> {node.menuName}</Tag>,
        value: node.id,
        key: node.id,
        children: renderTreeNodes(node.children)
      };
    }
    return {
      title: <Tag> {node.menuName}</Tag>,
      value: node.id,
      key: node.id
    };
  });
};

export function arrayToTree(menuItems: Navigation[]) {
  const map = new Map();

  // First, create a map with the Id as the key
  for (const item of menuItems) {
    map.set(item.id, { ...item, children: [] });
  }

  // Then, iterate through the items to build the tree
  const menuTree = [];
  for (const item of menuItems) {
    const menuItem = map.get(item.id);
    if (menuItem) {
      if (item.idParent === null) {
        // Top-level item
        menuTree.push(menuItem);
      } else {
        // Child item
        const parentItem = map.get(item.idParent);
        if (parentItem) {
          parentItem.children.push(menuItem);
        }
      }
    }
  }
  return menuTree;
}

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateRole(props: IProps) {
  const { setVisible, id } = props;
  const { data: Role, isLoading: LoadingRole } = useGetRoleByIdQuery(
    { id: id! },
    {
      skip: !id
    }
  );
  const { data: ListNavigation, isLoading: isLoadingNavigation } = useGetListNavigationQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const arrayTreeNavigation = arrayToTree(ListNavigation?.listPayload ?? []);
  const [newRole, { isLoading: LoadingInsertRole }] = useInsertRoleMutation();
  const [updateRole, { isLoading: LoadingUpdateRole }] = useUpdateRoleMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (Role && id) {
      formRef.setFieldsValue({
        roleName: Role?.payload?.role?.roleName,
        numberRole: Role?.payload?.role?.numberRole,
        listIdNavigation: Role?.payload?.navigation.map((x) => x.id),
        id: Role?.payload?.role?.id
      });
    } else {
      formRef.resetFields();
    }
  }, [Role, formRef, id]);
  const onfinish = async (values: any) => {
    try {
      const result = id
        ? await updateRole({
            role: values
          }).unwrap()
        : await newRole({ role: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="NewAndUpdateRole">
      <Spin spinning={LoadingRole}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 5 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item
                label="Tên"
                name={"roleName"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập tên"
                  }
                ]}
              >
                <Input />
              </Form.Item>
              <Form.Item label="Danh sách menu" name={"listIdNavigation"}>
                <TreeSelect
                  multiple
                  loading={isLoadingNavigation}
                  showSearch
                  treeNodeFilterProp={"title"}
                  maxTagCount={"responsive"}
                  treeLine={true}
                  treeData={renderTreeNodes(arrayTreeNavigation)}
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
                    loading={LoadingInsertRole || LoadingUpdateRole}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertRole || LoadingUpdateRole}
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

export const NewAndUpdateRole = WithErrorBoundaryCustom(_NewAndUpdateRole);
