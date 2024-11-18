import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Modal, Spin, Steps, Typography } from "antd";
import {
  ApartmentOutlined,
  CheckCircleFilled,
  ClockCircleTwoTone,
  HistoryOutlined,
  IssuesCloseOutlined,
  StopOutlined,
  UserOutlined
} from "@ant-design/icons";
import { WorkflowInstancesDTO } from "@models/workflowInstancesDTO";
import { useLazyGetListWorkflowHistoriesByIdInstanceQuery } from "@API/services/WorkFlowApis.service";
import dayjs from "dayjs";
import { useGetListUserQuery } from "@API/services/UserApis.service";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { HandleError } from "@admin/components";
import { useEffect, useState } from "react";

interface WorkFlowHistoryProps {
  workFlowInstance: WorkflowInstancesDTO | undefined;
}

function _WorkFlowHistory(props: WorkFlowHistoryProps) {
  const { workFlowInstance } = props;
  const [postLength, setPostLength] = useState(0);
  const [modal, contextHolder] = Modal.useModal();
  const [
    getListWorkflowHistoriesByIdInstance,
    { isLoading: LoadingListWorkflowHistories, data: ListWorkflowHistories }
  ] = useLazyGetListWorkflowHistoriesByIdInstanceQuery();
  const { data: dataUser } = useGetListUserQuery(
    {
      pageNumber: 0,
      pageSize: 0
    },
    { skip: !workFlowInstance }
  );
  const { data: dataUnit } = useGetListUnitQuery(
    {
      pageNumber: 0,
      pageSize: 0
    },
    { skip: !workFlowInstance }
  );
  const handleGetListWorkflowHistoriesByIdInstance = async () => {
    try {
      const res = await getListWorkflowHistoriesByIdInstance({
        idWorkFlowInstance: workFlowInstance?.id as string
      }).unwrap();
      if (res.success) setPostLength((prevState) => prevState + 1);
    } catch (e: any) {
      await HandleError(e);
    }
  };
  useEffect(() => {
    if (postLength === 0) return;
    (() => {
      modal.info({
        title: `Mốc thời gian của phiếu ${workFlowInstance?.workflowCode}`,
        icon: <HistoryOutlined />,
        width: 1000,
        content: <ContentHistory />,
        footer: null,
        closable: true,
        style: { top: 20 }
      });
    })();
  }, [postLength]);
  const ContentHistory = () => {
    return (
      <div
        style={{
          width: "100%",
          height: "auto",
          maxHeight: "80vh",
          overflow: "auto"
        }}
      >
        <Spin spinning={LoadingListWorkflowHistories}>
          <Typography.Title />
          <Steps
            current={ListWorkflowHistories?.listPayload?.length}
            direction="vertical"
            items={ListWorkflowHistories?.listPayload?.map((item) => {
              const User = dataUser?.listPayload?.find((user) => user.id === item?.idUser);
              const Unit = dataUnit?.listPayload?.find((unit) => unit.id === item?.idUnit);
              const finish = item?.isStepCompleted ? "finish" : "wait";
              const cancelled = item?.isCancelled ? "error" : "wait";
              const requestToChanged = item?.isRequestToChanged ? "process" : "wait";
              const status = finish === "finish" ? finish : cancelled === "error" ? cancelled : requestToChanged;

              let icon = <HistoryOutlined />;
              switch (status) {
                case "finish":
                  icon = <CheckCircleFilled style={{ color: "green" }} />;
                  break;
                case "error":
                  icon = <StopOutlined style={{ color: "red" }} />;
                  break;
                case "process":
                  icon = <IssuesCloseOutlined style={{ color: "#15d4ed" }} />;
                  break;
                default:
                  icon = <HistoryOutlined />;
                  break;
              }
              return {
                icon,
                status,
                subTitle: (
                  <Typography.Text type="secondary">
                    <ClockCircleTwoTone /> {item?.createdDate && dayjs(item?.createdDate).format("DD/MM/YYYY HH:mm:ss")}
                  </Typography.Text>
                ),
                title: item?.action,
                description: (
                  <div>
                    <Typography.Text type={"success"}>
                      <UserOutlined /> {User?.fullname} - <ApartmentOutlined /> {Unit?.unitName}
                    </Typography.Text>
                    <br />
                    {item?.message && <Typography.Text strong>Ghi chú : {item?.message}</Typography.Text>}
                  </div>
                )
              };
            })}
          />
        </Spin>
      </div>
    );
  };
  return (
    <div className="WorkFlowHistory">
      {contextHolder}
      <Button
        type="text"
        size={"middle"}
        icon={<HistoryOutlined />}
        onClick={async () => handleGetListWorkflowHistoriesByIdInstance()}
      >
        Lịch sử
      </Button>
    </div>
  );
}

export const WorkFlowHistory = WithErrorBoundaryCustom(_WorkFlowHistory);
