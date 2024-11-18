import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
// import { Pie } from "@ant-design/plots";
// import { useStatisticAcademicAchievementQuery } from "@API/services/Degree.service";
import { useEffect } from "react";
import { Button } from "antd";
// import { generateDocument } from "@admin/components/generateDocument/generateDocument";
// import ThongKeVanBangTheoHocLuc from "@admin/asset/files/ThongKeVanBangTheoHocLuc.docx";

function _PieCharts() {
//   const { data: ListStatisticAcademicAchievement, refetch: refetchListStatisticAcademicAchievement } =
//     useStatisticAcademicAchievementQuery({
//       pageSize: 0,
//       pageNumber: 0
//     });

//   useEffect(() => {
//     if (!ListStatisticAcademicAchievement) {
//       refetchListStatisticAcademicAchievement();
//       return;
//     }
//     if (!ListStatisticAcademicAchievement.listPayload) {
//       return;
//     }
//   }, [ListStatisticAcademicAchievement, refetchListStatisticAcademicAchievement]);

  const config = {
    //data: ListStatisticAcademicAchievement?.listPayload || [],
    angleField: "total",
    colorField: "academicAchievement",
    label: {
      text: "academicAchievement",
      style: {
        fontWeight: "bold"
      }
    },
    legend: {
      color: {
        title: false,
        position: "left",
        rowPadding: 5
      }
    }
  };
  const handlePrinToWord = () => {
    // const xxx = {
    //   table: ListStatisticAcademicAchievement?.listPayload?.map((item, index) => ({
    //     index: index + 1,
    //     academicAchievement: item.academicAchievement,
    //     total: item.total
    //   }))
    // };
    // generateDocument(ThongKeVanBangTheoHocLuc, `Thống kê số lương văn bằng theo học lực`, xxx);
  };
  return (

      
      <Button
        onClick={() => {
          handlePrinToWord();
        }}
      >
        {" "}
        Tải thống kê{" "}
      </Button>
    
  );
}

export const PieCharts = WithErrorBoundaryCustom(_PieCharts);
