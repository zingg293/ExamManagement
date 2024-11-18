import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useUpdateStepWorkFlowMutation } from "@API/services/WorkFlowApis.service";
import {
  DislikeOutlined,
  FileDoneOutlined,
  IssuesCloseOutlined,
  LikeOutlined,
  NodeCollapseOutlined,
  StopOutlined,
  SwapOutlined
} from "@ant-design/icons";
import { Fragment } from "react";
import { Button, Col, Form, Input, Modal, Row, Space, Tag, Typography } from "antd";
import { HandleError } from "@admin/components";
import { useGetUserQuery } from "@API/services/UserApis.service";
import { useGetWorkflowTemplateByIdQuery } from "@API/services/WorkflowTemplateApis.service";

interface HandleActionWorkFlowProps {
  refetchListMain: () => void;
  templateId?: string;
  record: any;
}

function _HandleActionWorkFlow(props: HandleActionWorkFlowProps) {
  const { refetchListMain, templateId, record } = props;
  const [UpdateStepRequestToHired, { isLoading: isLoadingUpdateStatusRequestToHired }] =
    useUpdateStepWorkFlowMutation();
  const { data: User } = useGetUserQuery({ fetch: true });
  const { data: WorkFlowTemplate } = useGetWorkflowTemplateByIdQuery(
    {
      idWorkflowTemplate: templateId!
    },
    { skip: !templateId }
  );
  const staterButtonContent = WorkFlowTemplate?.payload?.startWorkflowButton;
  const completeButtonContent = WorkFlowTemplate?.payload?.defaultCompletedStatus;
  const workFlowInstance = record?.workflowInstances?.at(0);
  const [modal, contextHolder] = Modal.useModal();
  const handleUpdateStepRequestToHired = async (idWorkFlowInstance: string) => {
    modal.success({
      title: "Xác nhận yêu cầu",
      icon: <SwapOutlined />,
      width: 600,
      content: (
        <Fragment>
          <p>Bạn có chắc chắn muốn xác nhận yêu cầu này?</p>
          <p>Yêu cầu sẽ được chuyển đến bước tiếp theo</p>
          <Form
            layout={"vertical"}
            onFinish={async (values) => {
              await updateStepRequestToHired({
                idWorkFlowInstance,
                isRequestToChange: false,
                isTerminated: false,
                message: values?.message
              });
            }}
          >
            <Form.Item name={"message"}>
              <Input.TextArea placeholder="Ghi chú" allowClear showCount />
            </Form.Item>
            <Form.Item>
              <Space
                style={{
                  width: "100%",
                  justifyContent: "flex-end"
                }}
              >
                <Button type="default" htmlType="button" onClick={() => Modal.destroyAll()}>
                  Hủy
                </Button>
                <Button type="primary" htmlType="submit" loading={isLoadingUpdateStatusRequestToHired}>
                  Lưu
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </Fragment>
      ),
      footer: null,
      closable: true
    });
  };
  const handleRequestToChange = async (idWorkFlowInstance: string) => {
    modal.warning({
      title: "Xác nhận yêu cầu thay đổi",
      icon: <IssuesCloseOutlined />,
      width: 600,
      content: (
        <Fragment>
          <p>Bạn có chắc chắn muốn xác nhận yêu cầu này?</p>
          <p>Yêu cầu này sẽ được đề nghị thay đổi</p>
          <Form
            layout={"vertical"}
            onFinish={async (values) => {
              await updateStepRequestToHired({
                idWorkFlowInstance,
                isRequestToChange: true,
                isTerminated: false,
                message: values?.message
              });
            }}
          >
            <Form.Item name={"message"}>
              <Input.TextArea placeholder="Ghi chú" allowClear showCount />
            </Form.Item>
            <Form.Item>
              <Space
                style={{
                  width: "100%",
                  justifyContent: "flex-end"
                }}
              >
                <Button type="default" htmlType="button" onClick={() => Modal.destroyAll()}>
                  Hủy
                </Button>
                <Button type="primary" htmlType="submit" loading={isLoadingUpdateStatusRequestToHired}>
                  Lưu
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </Fragment>
      ),
      footer: null,
      closable: true
    });
  };
  const handleCancelRequestToHired = async (idWorkFlowInstance: string) => {
    modal.error({
      title: "Xác nhận yêu cầu từ chối",
      icon: <StopOutlined />,
      width: 600,
      content: (
        <Fragment>
          <p>Bạn có chắc chắn muốn xác nhận yêu cầu này?</p>
          <p>Yêu cầu sẽ bị từ chối</p>
          <Form
            layout={"vertical"}
            onFinish={async (values) => {
              await updateStepRequestToHired({
                idWorkFlowInstance,
                isRequestToChange: false,
                isTerminated: true,
                message: values?.message
              });
            }}
          >
            <Form.Item name={"message"}>
              <Input.TextArea placeholder="Ghi chú" allowClear showCount />
            </Form.Item>
            <Form.Item>
              <Space
                style={{
                  width: "100%",
                  justifyContent: "flex-end"
                }}
              >
                <Button type="default" htmlType="button" onClick={() => Modal.destroyAll()}>
                  Hủy
                </Button>
                <Button type="primary" htmlType="submit" loading={isLoadingUpdateStatusRequestToHired}>
                  Lưu
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </Fragment>
      ),
      footer: null,
      closable: true
    });
  };
  const updateStepRequestToHired = async (body: {
    isTerminated: boolean;
    idWorkFlowInstance: string;
    isRequestToChange: boolean;
    message: string;
  }) => {
    try {
      const res = await UpdateStepRequestToHired(body).unwrap();
      if (res.success) {
        Modal.destroyAll();
        refetchListMain();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  const Action = () => (
    <Row>
      <Col span={24} hidden={workFlowInstance?.isTerminated || workFlowInstance?.isCompleted}>
        <Space
          direction={"vertical"}
          style={{
            width: "100%"
          }}
        >
          <Button
            type="primary"
            size={"middle"}
            icon={<SwapOutlined />}
            block
            hidden={
              ((workFlowInstance?.currentStep || 0) > 0 && workFlowInstance.createdBy === User?.payload?.data?.id) ||
              workFlowInstance?.isApproved
            }
            onClick={async () => {
              const idWorkFlowInstance = workFlowInstance?.id;
              if (idWorkFlowInstance) await handleUpdateStepRequestToHired(idWorkFlowInstance);
            }}
            style={{
              backgroundColor: record?.currentWorkFlowStep?.statusColor ?? "#19344c"
            }}
          >
            {record?.currentWorkFlowStep?.processWorkflowButton ?? staterButtonContent}
          </Button>
          <Button
            type="primary"
            size={"middle"}
            icon={<FileDoneOutlined />}
            block
            hidden={
              !(
                workFlowInstance?.createdBy === User?.payload?.data?.id &&
                workFlowInstance?.isApproved &&
                workFlowInstance?.currentStep === record.countWorkFlowStep
              )
            }
            onClick={async () => {
              const idWorkFlowInstance = workFlowInstance?.id;
              if (idWorkFlowInstance) await handleUpdateStepRequestToHired(idWorkFlowInstance);
            }}
          >
            {completeButtonContent}
          </Button>
          <Button
            type="primary"
            size={"middle"}
            danger
            icon={<StopOutlined />}
            block
            hidden={
              record?.currentWorkFlowStep?.allowTerminated === false ||
              workFlowInstance?.createdBy === User?.payload?.data?.id
            }
            onClick={async () => {
              const idWorkFlowInstance = workFlowInstance?.id;
              if (idWorkFlowInstance) await handleCancelRequestToHired(idWorkFlowInstance);
            }}
          >
            Từ chối
          </Button>
          <Button
            type="default"
            size={"middle"}
            icon={<IssuesCloseOutlined />}
            block
            hidden={workFlowInstance?.createdBy === User?.payload?.data?.id}
            onClick={async () => {
              const idWorkFlowInstance = workFlowInstance?.id;
              if (idWorkFlowInstance) await handleRequestToChange(idWorkFlowInstance);
            }}
          >
            Yêu cầu thay đổi
          </Button>
        </Space>
      </Col>
    </Row>
  );
  const handleAction = () => {
    modal.info({
      title: `Phiếu ${workFlowInstance?.workflowCode}`,
      icon: <NodeCollapseOutlined />,
      width: 500,
      content: (
        <Fragment>
          <p>
            Trạng thái:{" "}
            <Tag color={record?.currentWorkFlowStep?.statusColor}>{record.workflowInstances?.at(0)?.nameStatus}</Tag>
          </p>
          {workFlowInstance?.message && <p>Ghi chú: {workFlowInstance?.message}</p>}
          <Typography.Title></Typography.Title>
          <Action />
        </Fragment>
      ),
      footer: null,
      closable: true
    });
  };
  return (
    <div className="HandleActionWorkFlow">
      {contextHolder}
      {workFlowInstance?.isCompleted && (
        <Tag color={"green-inverse"}>
          <LikeOutlined /> Đã hoàn thành
        </Tag>
      )}
      {workFlowInstance?.isTerminated && (
        <Tag color={"red-inverse"}>
          <DislikeOutlined /> Đã từ chối
        </Tag>
      )}
      {!workFlowInstance?.isTerminated && !workFlowInstance?.isCompleted && (
        <Button type="primary" size={"middle"} icon={<SwapOutlined />} onClick={() => handleAction()}>
          Ký duyệt
        </Button>
      )}
    </div>
  );
}

export const HandleActionWorkFlow = WithErrorBoundaryCustom(_HandleActionWorkFlow);
