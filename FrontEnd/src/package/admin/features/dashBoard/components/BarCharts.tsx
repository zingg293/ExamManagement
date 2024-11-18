import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
// import { Column } from "@ant-design/plots";
// import { useStatisticNumberofDegreeByTypeQuery } from "@API/services/Degree.service";
import { useEffect, useState } from "react";
// import { generateDocument } from "@admin/components/generateDocument/generateDocument";
// import ThongKeVanBangTheoLoaiVanBang from "@admin/asset/files/ThongKeVanBangTheoLoaiVanBang.docx";
import { Button } from "antd";

interface DataChart {
  type: string;
  count: number;
}
function _Barchart() {
//   const { data: ListStatisticNumberofDegreeByType, refetch: refetchListStatisticNumberofDegreeByType } =
//     useStatisticNumberofDegreeByTypeQuery({
//       pageSize: 0,
//       pageNumber: 0
//     });
  const [newDatax, setNewDatax] = useState<DataChart[]>([]);
//   useEffect(() => {
//     if (!ListStatisticNumberofDegreeByType) {
//       refetchListStatisticNumberofDegreeByType();
//       return;
//     }
//     if (!ListStatisticNumberofDegreeByType.listPayload) {
//       return;
//     }
//     const updatedData: DataChart[] = [...newDatax];
//     ListStatisticNumberofDegreeByType.listPayload.forEach((item) => {
//       updatedData.push({
//         type: item.degreeTypeName || "Không xác định",
//         count: item.numberOfDegrees || 0
//       });
//     });
//     setNewDatax(updatedData);
//   }, [ListStatisticNumberofDegreeByType, refetchListStatisticNumberofDegreeByType]);

  const config = {
    data: newDatax,
    xField: "type",
    yField: "count",
    label: {
      //position: "middle",
      style: {
        fill: "#FFFFFF",
        opacity: 0.6
      }
    },
    xAxis: {
      tickCount: 10,
      label: {
        autoHide: true,
        autoRotate: false
      }
    },
    meta: {
      type: {
        alias: "Loại"
      },
      count: {
        alias: "Số lượng"
      }
    }
  };
  const handlePrinToWord = () => {
    // const xxx = {
    //   table: ListStatisticNumberofDegreeByType?.listPayload?.map((item, index) => ({
    //     index: index + 1,
    //     degreeTypeName: item.degreeTypeName,
    //     numberOfDegrees: item.numberOfDegrees
    //   }))
    // };
    // generateDocument(ThongKeVanBangTheoLoaiVanBang, `Thống kê số lương văn bằng theo loại văn bằng`, xxx);
  };
  return (
    <>
      
      <Button
        onClick={() => {
          handlePrinToWord();
        }}
      >
        {" "}
        Tải thống kê{" "}
      </Button>
    </>
  );
}

export const Barchart = WithErrorBoundaryCustom(_Barchart);
