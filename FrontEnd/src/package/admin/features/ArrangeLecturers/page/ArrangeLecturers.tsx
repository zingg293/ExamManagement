import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import React, { useState } from "react";
import { ColumnsType } from "antd/lib/table/interface";
import {
 // Button,
  Card,
  Col,
  Divider,
  Dropdown,
  Menu,
  message,
  //Popconfirm,
  Row,
  Space,
  TableProps,
  Tag,
  Typography
} from "antd";
import {
  //DeleteOutlined,
  EditOutlined,
  // PlusCircleOutlined,
  SettingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import {
 // HandleError,
  ModalContent } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { NewAndUpdateArrangeLecturers } from "@admin/features/ArrangeLecturers";
import { useDeleteArrangeLecturersMutation } from "@API/services/ArrangeLecturers.service";
import { useGetListTestScheduleQuery } from "@API/services/TestSchedule.service";
import { useGetListExaminationQuery } from "@API/services/Examination.service";
import { useGetListExamSubjectQuery } from "@API/services/ExamSubject.service";
import { useGetListEMSQuery } from "@API/services/EMS.service";
import { EMSDTO } from "@models/EMSDTO";

function _ArrangeLecturers() {
  // const { data: ListArrangeLecturers, isLoading: LoadingListArrangeLecturers } = useGetListArrangeLecturersQuery({
  //   pageSize: 0,
  //   pageNumber: 0
  // });
  const { data: ListEMS, isLoading: LoadingListEMS } = useGetListEMSQuery({
    pageSize: 0,
    pageNumber: 0
  });
  //const [deleteListArrangeLecturers, { isLoading: isLoadingDeleteListArrangeLecturers }] =
    useDeleteArrangeLecturersMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const { data: ListTestSchedule } = useGetListTestScheduleQuery({
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
  const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
    setSelectedRowKeys(newSelectedRowKeys);
  };
  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange
  };
  const menuAction = (record: EMSDTO) => {
    return (
      <Menu>
        <Menu.Item
          key="1"
          icon={<EditOutlined />}
          onClick={() => {
            setId(record.id);
            message.success("Thành công");
          }}
        >
          Đăng ký
        </Menu.Item>
        <Menu.Item
          key="1"
          icon={<EditOutlined />}
          onClick={() => {
            console.log("Đăng ký thành công");
            message.success("Thành công");
          }}
        >
          Xếp lịch
        </Menu.Item>
      </Menu>
    );
  };
  const columns: ColumnsType<EMSDTO> = [
    {
      title: "Tên",
      dataIndex: "emsName",
      key: "emsName"
    },
    {
      title: "Trạng thái",
      dataIndex: "isActive",
      key: "isActive",
      render: (text) => (!text ? <Tag color={"error"}>Ẩn</Tag> : <Tag color={"success"}>Hiển thị</Tag>)
    },
    {
      title: "Số lượng sinh viên dự thi",
      dataIndex: "numberOfStudents",
      key: "numberOfStudents",
      render: (text) => <Tag color={"purple"}>{text}</Tag>
    },
    {
      title: "Số lượng giảng viên",
      dataIndex: "numberOfLecturers",
      key: "numberOfLecturers",
      render: (text) => <Tag color={"red"}>{text}</Tag>
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
      render: (idExam) => {
        const ExamSubject = ListExamSubject?.listPayload?.find((item) => item.id === idExam);
        return ExamSubject ? ExamSubject.examSubjectName : null;
      }
    },
    {
      title: "Lịch Thi",
      dataIndex: "idTestSchedule",
      key: "idTestSchedule",
      render: (idExam) => {
        const TestSchedule = ListTestSchedule?.listPayload?.find((item) => item.id === idExam);
        return TestSchedule ? TestSchedule.testScheduleName : null;
      }
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
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
  const propsTable: TableProps<EMSDTO> = {
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
    dataSource: ListEMS?.listPayload,
    // onChange: handleChange,
    loading: LoadingListEMS,
    pagination: {
      total: ListEMS?.totalElement,
      pageSize: 10,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} itArrangeLecturers`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className={"unit"}>
      <ModalContent
        visible={isOpenModal}
        setVisible={setIsOpenModal}
        title={id ? "Chỉnh sửa  " : "Thêm mới"}
        width={"600px"}
      >
        <NewAndUpdateArrangeLecturers setVisible={setIsOpenModal} id={id} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Đăng kí và xếp lịch gác thi</Typography.Title>
          <Divider />
          <Space
            style={{
              marginBottom: 16
            }}
            wrap
          >
            {/*<Button*/}
            {/*  onClick={() => {*/}
            {/*    setId(undefined);*/}
            {/*    setIsOpenModal(true);*/}
            {/*  }}*/}
            {/*  icon={<PlusCircleOutlined />}*/}
            {/*  type="primary"*/}
            {/*>*/}
            {/*  Thêm mới*/}
            {/*</Button>*/}
            {/*<Popconfirm*/}
            {/*  title="Bạn có chắc chắn không ?"*/}
            {/*  okText="Có"*/}
            {/*  cancelText="Không"*/}
            {/*  disabled={!(selectedRowKeys.length > 0)}*/}
            {/*  onConfirm={async () => {*/}
            {/*    try {*/}
            {/*      const result = await deleteListArrangeLecturers({*/}
            {/*        idArrangeLecturers: selectedRowKeys as string[]*/}
            {/*      }).unwrap();*/}
            {/*      if (result.success) return setSelectedRowKeys([]);*/}
            {/*    } catch (e: any) {*/}
            {/*      await HandleError(e);*/}
            {/*    }*/}
            {/*  }}*/}
            {/*>*/}
            {/*  /!*<Button*!/*/}
            {/*  /!*  danger*!/*/}
            {/*  /!*  type="primary"*!/*/}
            {/*  /!*  loading={isLoadingDeleteListArrangeLecturers}*!/*/}
            {/*  /!*  disabled={!(selectedRowKeys.length > 0)}*!/*/}
            {/*  /!*  icon={<DeleteOutlined />}*!/*/}
            {/*  /!*>*!/*/}
            {/*  /!*  Xóa {selectedRowKeys.length} mục*!/*/}
            {/*  /!*</Button>*!/*/}
            {/*</Popconfirm>*/}
          </Space>
          <Card bordered={false} className="criclebox">
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}
export const ArrangeLecturers = WithErrorBoundaryCustom(_ArrangeLecturers);
