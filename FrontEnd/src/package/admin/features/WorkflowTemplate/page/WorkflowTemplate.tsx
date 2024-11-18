import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import React, { useState } from "react";
import { HandleError, ModalContent } from "@admin/components";
import { NewAndUpdateWorkFlow } from "@admin/features/WorkflowTemplate/components/NewAndUpdateWorkFlow";
import { Button, Card, Col, Divider, Dropdown, Menu, Popconfirm, Row, Space, TableProps, Typography } from "antd";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { ColumnsType } from "antd/lib/table/interface";
import { WorkflowTemplateDTO } from "@models/workflowTemplateDTO";
import { AuditOutlined, DeleteOutlined, EditOutlined, PlusCircleOutlined, SettingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import {
  useDeleteWorkflowTemplateMutation,
  useGetListWorkflowTemplateQuery
} from "@API/services/WorkflowTemplateApis.service";

function _WorkflowTemplate() {
  const { data: ListWorkFlowTemplate, isLoading: isLoadingWorkFlowTemplate } = useGetListWorkflowTemplateQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [DeleteWorkflowTemplate, { isLoading: isLoadingDeleteWorkflowTemplate }] = useDeleteWorkflowTemplateMutation();
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
  const menuAction = (record: WorkflowTemplateDTO) => {
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
  const columns: ColumnsType<WorkflowTemplateDTO> = [
    {
      title: "Tên",
      dataIndex: "workflowName",
      key: "workflowName",
      render: (text) => (
        <Space>
          <AuditOutlined />
          <Typography.Text strong>{text}</Typography.Text>
        </Space>
      )
    },
    {
      title: "Mã",
      dataIndex: "workflowCode",
      key: "workflowCode"
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      onFilter: (value: any, record) => record.createdDate.toString().startsWith(value),
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
  const propsTable: TableProps<WorkflowTemplateDTO> = {
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
    dataSource: ListWorkFlowTemplate?.listPayload,
    loading: isLoadingWorkFlowTemplate,
    rowSelection,
    pagination: {
      total: ListWorkFlowTemplate?.totalElement,
      pageSize: 10,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "middle"
  };
  //#endregion
  return (
    <div className="WorkflowTemplate">
      <ModalContent
        visible={isOpenModal}
        setVisible={setIsOpenModal}
        title={id ? "Chỉnh sửa quy trình" : "Thêm mới quy trình"}
        afterClose={() => setId(undefined)}
      >
        <NewAndUpdateWorkFlow idWorkflowTemplate={id} setIsOpenModal={setIsOpenModal} />
      </ModalContent>
      <Row gutter={[24, 0]}>
        <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
          <Typography.Title level={2}> Danh sách quy trình </Typography.Title>
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
                  const result = await DeleteWorkflowTemplate({
                    idWorkflowTemplate: selectedRowKeys as string[]
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
                loading={isLoadingDeleteWorkflowTemplate}
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

export const WorkflowTemplate = WithErrorBoundaryCustom(_WorkflowTemplate);
