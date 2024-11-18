import { useEffect, useState } from "react";
import { Table } from "antd";

// interface RowProps extends React.HTMLAttributes<HTMLTableRowElement> {
//   "data-row-key": string;
// }
//
// const Row = ({ children, ...props }: RowProps) => {
//   const { attributes, listeners, setNodeRef, setActivatorNodeRef, transform, transition, isDragging } = useSortable({
//     id: props["data-row-key"]
//   });
//
//   const style: React.CSSProperties = {
//     ...props.style,
//     transform: CSS.Transform.toString(transform && { ...transform, scaleY: 1 }),
//     transition,
//     ...(isDragging ? { position: "relative", zIndex: 999 } : {})
//   };
//
//   return (
//     <tr {...props} ref={setNodeRef} style={style} {...attributes}>
//       {React.Children.map(children, (child) => {
//         if ((child as React.ReactElement).key === "Action") {
//           return React.cloneElement(child as React.ReactElement, {
//             children: (
//               <Space size={"middle"}>
//                 {child}
//                 <MenuOutlined
//                   ref={setActivatorNodeRef}
//                   style={{ touchAction: "none", cursor: "move", fontSize: 20 }}
//                   {...listeners}
//                 />
//               </Space>
//             )
//           });
//         } else return child;
//       })}
//     </tr>
//   );
// };
const DragDropTable = ({ ...rest }) => {
  const [data, setData] = useState([]);
  useEffect(() => {
    if (!rest.dataSource?.length) return setData([]);
    setData(rest.dataSource);
  }, [rest.dataSource]);
  const temp = {
    ...rest,
    dataSource: data
  };
  // const onDragEnd = ({ active, over }: DragEndEvent) => {
  //   if (active.id !== over?.id) {
  //     if (!data?.length) return;
  //     setData((prev: any) => {
  //       const activeIndex = prev.findIndex((i: any) => i?.id === active?.id);
  //       const overIndex = prev.findIndex((i: any) => i?.id === over?.id);
  //       return arrayMove(prev, activeIndex, overIndex);
  //     });
  //   }
  // };

  // return (
  //   <DndContext modifiers={[restrictToVerticalAxis]} onDragEnd={onDragEnd}>
  //     <SortableContext items={data?.map((i: any) => i?.id)} strategy={verticalListSortingStrategy}>
  //       <Table
  //         components={{
  //           body: {
  //             row: Row
  //           }
  //         }}
  //         {...temp}
  //       />
  //     </SortableContext>
  //   </DndContext>
  // );
  return (
    <Table
      // components={{
      //   body: {
      //     row: Row
      //   }
      // }}
      {...temp}
    />
  );
};

export default DragDropTable;
