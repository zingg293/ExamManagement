import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import React, { useState } from "react";
import { ColumnsType } from "antd/lib/table/interface";
import { Button, Card, Col, Divider, Dropdown, Menu, Popconfirm, Row, Space, TableProps, Tag, Typography } from "antd";
import { DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import { HandleError, ModalContent } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { NewAndUpdateTestSchedule } from "@admin/features/TestSchedule";
import { TestScheduleDTO } from "@models/TestScheduleDTO";
import { useDeleteTestScheduleMutation } from "@API/services/TestSchedule.service";
import { useGetListTestScheduleQuery } from "@API/services/TestSchedule.service";
import { useGetListExaminationQuery } from "@API/services/Examination.service";
import { useGetListExamSubjectQuery } from "@API/services/ExamSubject.service";
// import { formatMoney } from "~/units";

function _TestSchedule() {
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
  const [deleteListTestSchedule, { isLoading: isLoadingDeleteListTestSchedule }] = useDeleteTestScheduleMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);

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
            setId(record.id);
            setIsOpenModal(true);
          }}
        >
          Chỉnh sửa
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
      <ModalContent
        visible={isOpenModal}
        setVisible={setIsOpenModal}
        title={id ? "Chỉnh sửa  " : "Thêm mới"}
        width={"600px"}
      >
        <NewAndUpdateTestSchedule setVisible={setIsOpenModal} id={id} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách lịch thi </Typography.Title>
          <Divider />
          <Space
            style={{
              marginBottom: 16
            }}
            wrap
          >
            <Button
              onClick={() => {
                setId(undefined);
                setIsOpenModal(true);
              }}
              icon={<PlusCircleOutlined />}
              type="primary"
            >
              Thêm mới
            </Button>
            <Popconfirm
              title="Bạn có chắc chắn không ?"
              okText="Có"
              cancelText="Không"
              disabled={!(selectedRowKeys.length > 0)}
              onConfirm={async () => {
                try {
                  const result = await deleteListTestSchedule({
                    idTestSchedule: selectedRowKeys as string[]
                  }).unwrap();
                  if (result.success) return setSelectedRowKeys([]);
                } catch (e: any) {
                  await HandleError(e);
                }
              }}
            >
              <Button
                danger
                type="primary"
                loading={isLoadingDeleteListTestSchedule}
                disabled={!(selectedRowKeys.length > 0)}
                icon={<DeleteOutlined />}
              >
                Xóa {selectedRowKeys.length} mục
              </Button>
            </Popconfirm>
          </Space>
          <Card bordered={false} className="criclebox">
            <DragAndDropTable {...propsTable} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}
export const TestSchedule = WithErrorBoundaryCustom(_TestSchedule);
