import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Card, Col, Row, Spin, Tree, Typography } from "antd";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { ApartmentOutlined } from "@ant-design/icons";
import { UnitDTO } from "@models/unitDto";

function arrayToTree(menuItems: UnitDTO[]) {
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
      if (item.parentId === null) {
        // Top-level item
        menuTree.push(menuItem);
      } else {
        // Child item
        const parentItem = map.get(item.parentId);
        if (parentItem) {
          parentItem.children.push(menuItem);
        }
      }
    }
  }
  return menuTree;
}

export const renderTreeNodes = (nodes: UnitDTO[]) => {
  return nodes.map((node) => {
    if (node.children && node.children.length > 0) {
      return (
        <Tree.TreeNode
          title={<Typography.Text strong>{node.unitName}</Typography.Text>}
          key={node.id}
          icon={<ApartmentOutlined />}
        >
          {renderTreeNodes(node.children)}
        </Tree.TreeNode>
      );
    }
    return (
      <Tree.TreeNode
        title={<Typography.Text strong>{node.unitName}</Typography.Text>}
        key={node.id}
        icon={<ApartmentOutlined />}
      />
    );
  });
};

function _DepartmentChart() {
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitQuery({ pageSize: 0, pageNumber: 0 });
  const data = arrayToTree(ListUnit?.listPayload ?? []);
  return (
    <div className="DepartmentChart">
      <Spin spinning={LoadingListUnit}>
        <Row
          style={{
            width: "calc(100vh - 64px)"
          }}
        >
          <Col span={24}>
            <Card>
              <Tree showIcon showLine>
                {renderTreeNodes(data)}
              </Tree>
            </Card>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const DepartmentChart = WithErrorBoundaryCustom(_DepartmentChart);
