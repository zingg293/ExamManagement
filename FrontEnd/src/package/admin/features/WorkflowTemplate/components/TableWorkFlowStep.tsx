import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Card, Popconfirm, Space, TableProps, Tag, Typography } from "antd";
import DragAndDropTable from "@admin/components/DragAndDropTable/DragAndDropTable";
import { ColumnsType } from "antd/lib/table/interface";
import { WorkflowStepDTO } from "@models/workflowStepDTO";
import dayjs from "dayjs";
import { useState } from "react";
import {
  useDeleteWorkflowStepMutation,
  useGetListWorkflowStepByIdTemplateQuery
} from "@API/services/WorkflowStepApis.service";
import { HandleError } from "@admin/components";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { useGetListRoleQuery } from "@API/services/Role.service";
import { ClusterOutlined, SolutionOutlined } from "@ant-design/icons";
import { NewAndUpdateWorkFlowStep } from "@admin/features/WorkflowTemplate";
import { DrawerContent } from "@admin/components/DrawerContent/DrawerContent";

interface IProps {
  idWorkflowTemplate?: string;
}

function _TableWorkFlowStep(props: IProps) {
  const { idWorkflowTemplate } = props;
  const { data: ListUnit } = useGetListUnitQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListRole } = useGetListRoleQuery({ pageSize: 0, pageNumber: 0 });
  const { data: ListWorkflowStep, isLoading: LoadingListWorkflowStep } = useGetListWorkflowStepByIdTemplateQuery(
    { idTemplate: idWorkflowTemplate!, pageSize: 0, pageNumber: 0 },
    { skip: !idWorkflowTemplate }
  );
  const [DeleteWorkflowStep, { isLoading: LoadingDeleteWorkflowStep }] = useDeleteWorkflowStepMutation();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [id, setId] = useState<string | undefined>(undefined);
  const handleDeleteWorkflowStep = async (id: string) => {
    try {
      await DeleteWorkflowStep({
        idWorkflowStep: [id]
      }).unwrap();
    } catch (e: any) {
      await HandleError(e);
    }
  };
  //#region Table config
  const columns: ColumnsType<WorkflowStepDTO> = [
    {
      title: "STT",
      dataIndex: "stt",
      key: "stt",
      render: (text, record, index) => index + 1,
      width: "5%"
    },
    {
      title: "Tên",
      dataIndex: "stepName",
      key: "stepName"
    },
    {
      title: "Quyền",
      dataIndex: "idRoleAssign",
      key: "idRoleAssign",
      render: (text) => (
        <Space>
          <SolutionOutlined />
          <Typography.Text>{ListRole?.listPayload?.find((x) => x.id === text)?.roleName}</Typography.Text>
        </Space>
      )
    },
    {
      title: "Phòng ban",
      dataIndex: "idUnitAssign",
      key: "idUnitAssign",
      render: (text) => (
        <Space>
          <ClusterOutlined />
          <Typography.Text>{ListUnit?.listPayload?.find((x) => x.id === text)?.unitName}</Typography.Text>
        </Space>
      )
    },
    {
      title: "Phòng ban quản lý trực tiếp duyệt",
      dataIndex: "isDirectUnit",
      key: "isDirectUnit",
      render: (text) => {
        return text ? <Tag color={"green-inverse"}>Đồng ý</Tag> : <Tag color={"red-inverse"}>Không đồng ý</Tag>;
      }
    },
    {
      title: "Thứ tự",
      dataIndex: "order",
      key: "order"
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdDate",
      key: "createdDate",
      render: (text) => dayjs(text).format("DD-MM-YYYY HH:mm")
    },
    {
      title: "Hiệu chỉnh",
      dataIndex: "ActionOne",
      render: (_, record) => (
        <Space>
          <Button
            type="link"
            onClick={() => {
              setId(record.id);
              setIsOpenModal(true);
            }}
          >
            Chỉnh sửa
          </Button>
          <Popconfirm
            title={`Bạn có chắc chắn muốn xóa ${record.stepName} không?`}
            onConfirm={() => handleDeleteWorkflowStep(record.id)}
          >
            <Button type="link" danger loading={LoadingDeleteWorkflowStep}>
              Xóa
            </Button>
          </Popconfirm>
        </Space>
      )
    },
    {
      dataIndex: "Action",
      key: "Action",
      fixed: "right",
      width: "5%"
    }
  ];
  const propsTable: TableProps<WorkflowStepDTO> = {
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
    dataSource: ListWorkflowStep?.listPayload,
    loading: LoadingListWorkflowStep,
    pagination: {
      total: ListWorkflowStep?.totalElement,
      pageSize: 10,
      showSizeChanger: false,
      showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
    },
    size: "small",
    footer: () => (
      <Button
        type="primary"
        onClick={() => {
          setIsOpenModal(true);
        }}
      >
        Thêm mới
      </Button>
    )
  };
  //#endregion
  return (
    <div className="TableWorkFlowStep">
      <DrawerContent
        open={isOpenModal}
        onClose={() => setIsOpenModal(false)}
        title={id ? "Chỉnh sửa quy trình các bước" : "Thêm mới quy trình các bước"}
        afterOpenChange={(visible) => {
          if (!visible) {
            setId(undefined);
          }
        }}
        width={"auto"}
      >
        <NewAndUpdateWorkFlowStep
          setVisible={setIsOpenModal}
          idWorkFlowStep={id}
          idWorkFlowTemplate={idWorkflowTemplate}
        />
      </DrawerContent>
      <Card bordered={false} className="criclebox" title={"Quy trình các bước"}>
        <DragAndDropTable {...propsTable} />
      </Card>
    </div>
  );
}

export const TableWorkFlowStep = WithErrorBoundaryCustom(_TableWorkFlowStep);
