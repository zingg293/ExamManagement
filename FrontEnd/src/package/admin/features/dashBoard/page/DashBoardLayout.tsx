import { Col, Row, Divider, Statistic, Card } from "antd";
import { CompanyIntroduction } from "@admin/components";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
// import { useStatisticNumberofDegreeQuery } from "@API/services/Degree.service";
import { useEffect } from "react";
import { Barchart } from "@admin/features/dashBoard/components/BarCharts";
import { PieCharts } from "@admin/features/dashBoard/components/PieCharts";
// import { ToDoListDragAndDrop } from "@admin/features/dashBoard/page/ToDoListDragAndDrop";

function _DashBoardLayout() {
  // const { data: StatisticNumberofDegree, refetch: refetchStatisticNumberofDegree } = useStatisticNumberofDegreeQuery(
  //   {}
  // );
  // const numberNotGive =
  //   StatisticNumberofDegree && StatisticNumberofDegree.payload && StatisticNumberofDegree.payload.numberNotGive;
  // const numberGived =
  //   StatisticNumberofDegree && StatisticNumberofDegree.payload && StatisticNumberofDegree.payload.numberGived;

  // useEffect(() => {
  //   if (!StatisticNumberofDegree) {
  //     refetchStatisticNumberofDegree();
  //   }
  // }, [StatisticNumberofDegree, refetchStatisticNumberofDegree]);
  return (
    <>
      {/* Dòng mới với 4 cột căn giữa */}
      <Row justify="center" align="middle">
        <Col xs={24} sm={12} md={8} lg={8} xl={8}>
          <Card>
            <Statistic
              title="Tổng số giảng viên"
              //value={numberNotGive && numberGived ? numberNotGive + numberGived : "Loading..."}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} md={8} lg={8} xl={8}>
          <Card>
            <Statistic
              title="Tổng số lịch gác thi"
              // value={
              //   // StatisticNumberofDegree &&
              //   // StatisticNumberofDegree.payload &&
              //   // StatisticNumberofDegree.payload.numberGived
              // }
            />{" "}
          </Card>
        </Col>
        <Col xs={24} sm={12} md={8} lg={8} xl={8}>
          <Card>
            <Statistic
              title="Tổng số người dùng"
              // value={
              //   StatisticNumberofDegree &&
              //   StatisticNumberofDegree.payload &&
              //   StatisticNumberofDegree.payload.numberNotGive
              // }
            />
          </Card>
        </Col>
      </Row>
      <Divider />
      <Row justify="center" align="middle">
        <Col xs={24} sm={12} md={12} lg={12} xl={12}>
          <Card title="Số lượng bằng đã cấp theo loại">
            <Barchart />
          </Card>
        </Col>
        <Col xs={24} sm={12} md={12} lg={12} xl={12}>
          <Card title="Thống kê theo học lực">
            <PieCharts />
          </Card>
        </Col>
      </Row>
      <Divider />
      <Row gutter={[10, 0]}>
        {/*<Col xs={24} md={24} sm={24} lg={24} xl={24} className="mb-24">*/}
        {/*<ToDoListDragAndDrop />*/}
        {/*</Col>*/}
        <Col xs={24} md={24} sm={24} lg={24} xl={24}>
          <CompanyIntroduction />
        </Col>
      </Row>
    </>
  );
}

export const DashBoardLayout = WithErrorBoundaryCustom(_DashBoardLayout);
