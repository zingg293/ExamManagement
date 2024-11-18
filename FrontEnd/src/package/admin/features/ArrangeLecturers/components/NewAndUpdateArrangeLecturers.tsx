import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import React, { useState } from "react";
import { ColumnsType } from "antd/lib/table/interface";
import { Card, Col, Divider, Dropdown, Menu, message, Row, TableProps, Tag, Typography } from "antd";
import { EditOutlined, SettingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { TestScheduleDTO } from "@models/TestScheduleDTO";
import { useGetListTestScheduleQuery } from "@API/services/TestSchedule.service";
import { useGetListExaminationQuery } from "@API/services/Examination.service";
import { useGetListExamSubjectQuery } from "@API/services/ExamSubject.service";
//import { useInsertDOEMSMutation } from "@API/services/DOEMS.service";
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
// eslint-disable-next-line @typescript-eslint/no-unused-vars
import { DOEMSDTO } from "@models/DOEMSDTO";
interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}
function _NewAndUpdateArrangeLecturers(props: IProps) {
  const { data: ListTestSchedule, isLoading: LoadingListTestSchedule } = useGetListTestScheduleQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListExamination } = useGetListExaminationQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListExamSubject } = useGetListExamSubjectQuery({
    pageSize: 0,
    pageNumber: 0
  });
  //  const [insertDOEMS] = useInsertDOEMSMutation();
  // const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  // const { setVisible, id } = props;
  const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
    setSelectedRowKeys(newSelectedRowKeys);
  };
  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange
  };
  const menuAction = (record: TestScheduleDTO) => {
    return (
      <Menu>
        <Menu.Item
          key="1"
          icon={<EditOutlined />}
          onClick={() => {
            // const DOEMSModel: DOEMSDTO = {
            //   id: "00000000-0000-0000-0000-000000000000", // Chắc chắn rằng giá trị id là chuỗi GUID hợp lệ
            //   isActive: false,
            //   createdDate: new Date(),
            //   idEMS: record.id,
            //   idLecturer: "00000000-0000-0000-0000-000000000000",
            //   idRoom: "00000000-0000-0000-0000-000000000000",
            //   fromDate: new Date(),
            //   toDate: new Date(),
            //   idStudyGroup: "00000000-0000-0000-0000-000000000000",
            //   idDOTS: "00000000-0000-0000-0000-000000000000",
            //   isDeleted: false,
            //   doUseLabRoom: true,
            //   note: "your_note_value",
            //   idTeamcode: "00000000-0000-0000-0000-000000000000"
            // };
            // setId(record.id);
            // const result = insertDOEMS(DOEMSModel).unwrap(); // Truyền DOEMSModel trực tiếp
            // if (result.success) {
            message.success("Đăng ký lịch thành công");
            // }
          }}
        >
          Đăng ký
        </Menu.Item>
      </Menu>
    );
  };
  const columns: ColumnsType<TestScheduleDTO> = [
    {
      title: "Tên",
      dataIndex: "testScheduleName",
      key: "testScheduleName"
    },
    {
      title: "Trạng thái",
      dataIndex: "isActive",
      key: "isActive",
      render: (text) => (!text ? <Tag color={"error"}>Ẩn</Tag> : <Tag color={"success"}>Hiển thị</Tag>)
    },
    {
      title: "Kì Thi",
      dataIndex: "idExam",
      key: "idExam",
      render: (idExam) => {
        const Examination = ListExamination?.listPayload?.find((item) => item.id === idExam);
        return Examination ? Examination.examName : null;
      }
    },
    {
      title: "Môn Thi",
      dataIndex: "idExamSubject",
      key: "idExamSubject",
      render: (idExamSubject) => {
        const ExamSubject = ListExamSubject?.listPayload?.find((item) => item.id === idExamSubject);
        return ExamSubject ? ExamSubject.examSubjectName : null;
      }
    },
    {
      title: "Từ ngày",
      dataIndex: "fromDate",
      key: "fromDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Đến ngày",
      dataIndex: "toDate",
      key: "toDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Tổ chức thi cuối kì",
      dataIndex: "organizeFinalExams",
      key: "organizeFinalExams",
      render: (text) => (!text ? <Tag color={"error"}>Không</Tag> : <Tag color={"success"}>Có</Tag>)
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Ghi chú",
      dataIndex: "note",
      key: "note"
    },
    {
      title: "Hiệu chỉnh",
      dataIndex: "Action",
      key: "Action",
      fixed: "right",
      render: (_, record) => (
        <Dropdown overlay={menuAction(record)} trigger={["click"]} placement={"bottomCenter"}>
          <SettingOutlined style={{ fontSize: 20 }} />
        </Dropdown>
      ),
      width: "8%"
    }
  ];
  const propsTable: TableProps<TestScheduleDTO> = {
    scroll: {
      x: 800
    },
    bordered: true,
    rowKey: (record) => record.id,
    columns: columns.map((item) => ({
      ellipsis: true,
      with: 150,
      ...item
    })),
    rowSelection: rowSelection,
    dataSource: ListTestSchedule?.listPayload,
    // onChange: handleChange,
    loading: LoadingListTestSchedule,
    pagination: {
      total: ListTestSchedule?.totalElement,
      pageSize: 10,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className={"unit"}>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Đăng ký lịch thi </Typography.Title>
          <Divider />
          <Card bordered={false} className="criclebox">
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export const NewAndUpdateArrangeLecturers = WithErrorBoundaryCustom(_NewAndUpdateArrangeLecturers);
