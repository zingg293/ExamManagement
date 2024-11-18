import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import React, { useState } from "react";
import { ColumnsType } from "antd/lib/table/interface";
import { Button, Card, Col, Divider, Dropdown, Menu, Popconfirm, Row, Space, TableProps, Tag, Typography } from "antd";
import { DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import { HandleError, ModalContent } from "@admin/components";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { NewAndUpdateExamSubject } from "@admin/features/ExamSubject";
import { ExamSubjectDTO } from "@models/ExamSubjectDTO";
import { useDeleteExamSubjectMutation } from "@API/services/ExamSubject.service";
import { useGetListExamSubjectQuery } from "@API/services/ExamSubject.service";
import { useGetListExaminationQuery } from "@API/services/Examination.service";
import { useGetListExaminationTypeQuery } from "@API/services/ExaminationType.service";
// import { formatMoney } from "~/units";

function _ExamSubject() {
  const { data: ListExamSubject, isLoading: LoadingListExamSubject } = useGetListExamSubjectQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [deleteListExamSubject, { isLoading: isLoadingDeleteListExamSubject }] = useDeleteExamSubjectMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  //#region Table config
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const { data: ListExamination } = useGetListExaminationQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListExaminationType } = useGetListExaminationTypeQuery({
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
  const menuAction = (record: ExamSubjectDTO) => {
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
  const columns: ColumnsType<ExamSubjectDTO> = [
    {
      title: "Tên",
      dataIndex: "examSubjectName",
      key: "examSubjectName"
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
      title: "Loại Kì Thi",
      dataIndex: "idExamType",
      key: "idExamType",
      render: (idExamType) => {
        const ExaminationType = ListExaminationType?.listPayload?.find((item) => item.id === idExamType);
        return ExaminationType ? ExaminationType.examTypeName : null;
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
  const propsTable: TableProps<ExamSubjectDTO> = {
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
    dataSource: ListExamSubject?.listPayload,
    // onChange: handleChange,
    loading: LoadingListExamSubject,
    pagination: {
      total: ListExamSubject?.totalElement,
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
        <NewAndUpdateExamSubject setVisible={setIsOpenModal} id={id} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh mục môn thi </Typography.Title>
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
                  const result = await deleteListExamSubject({
                    idExamSubject: selectedRowKeys as string[]
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
                loading={isLoadingDeleteListExamSubject}
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
export const ExamSubject = WithErrorBoundaryCustom(_ExamSubject);
