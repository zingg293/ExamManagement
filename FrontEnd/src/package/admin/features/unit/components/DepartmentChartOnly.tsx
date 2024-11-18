import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useGetListUnitByIdParentQuery } from "@API/services/UnitApis.service";
import { Card, Col, Row, Spin, Tree } from "antd";
import { renderTreeNodes } from "@admin/features/unit";

interface DepartmentChartOnlyProps {
  id?: string;
}

function _DepartmentChartOnly(props: DepartmentChartOnlyProps) {
  const { id } = props;
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitByIdParentQuery(
    { idParent: id! },
    {
      skip: !id
    }
  );
  return (
    <div className="DepartmentChartOnly">
      <Spin spinning={LoadingListUnit}>
        <Row>
          <Col span={24}>
            <Card>
              <Tree showIcon showLine>
                {renderTreeNodes(ListUnit?.listPayload ?? [])}
              </Tree>
            </Card>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const DepartmentChartOnly = WithErrorBoundaryCustom(_DepartmentChartOnly);
