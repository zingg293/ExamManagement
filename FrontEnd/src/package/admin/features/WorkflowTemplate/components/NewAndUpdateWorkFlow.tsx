import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Spin } from "antd";
import {
  useGetWorkflowTemplateByIdQuery,
  useInsertWorkflowTemplateMutation,
  useUpdateWorkflowTemplateMutation
} from "@API/services/WorkflowTemplateApis.service";
import { SaveOutlined } from "@ant-design/icons";
import { WorkflowTemplateDTO } from "@models/workflowTemplateDTO";
import { HandleError } from "@admin/components";
import { TableWorkFlowStep } from "@admin/features/WorkflowTemplate";
import { useEffect } from "react";

interface IProps {
  idWorkflowTemplate?: string;
  setIsOpenModal: (isOpenModal: boolean) => void;
}

function _NewAndUpdateWorkFlow(props: IProps) {
  const { idWorkflowTemplate } = props;
  const { data: WorkflowTemplate, isLoading: isLoadingWorkflowTemplate } = useGetWorkflowTemplateByIdQuery(
    {
      idWorkflowTemplate: idWorkflowTemplate || ""
    },
    {
      skip: !idWorkflowTemplate
    }
  );
  const [InsertWorkflowTemplate, { isLoading: isLoadingInsertWorkflowTemplate }] = useInsertWorkflowTemplateMutation();
  const [UpdateWorkflowTemplate, { isLoading: isLoadingUpdateWorkflowTemplate }] = useUpdateWorkflowTemplateMutation();
  const [formWorkflowTemplate] = Form.useForm();
  useEffect(() => {
    if (WorkflowTemplate?.payload && idWorkflowTemplate) {
      formWorkflowTemplate.setFieldsValue(WorkflowTemplate.payload);
    } else {
      formWorkflowTemplate.resetFields();
    }
  }, [idWorkflowTemplate, formWorkflowTemplate, WorkflowTemplate?.payload]);
  const onFinishSaveWorkFlowTemplate = async (values: WorkflowTemplateDTO) => {
    try {
      idWorkflowTemplate
        ? await UpdateWorkflowTemplate({ workflowTemplate: values })
        : await InsertWorkflowTemplate({ workflowTemplate: values });
    } catch (error: any) {
      await HandleError(error);
    }
  };

  return (
    <div className={"NewAndUpdateWorkFlow"}>
      <Row>
        <Col span={24}>
          <Spin spinning={isLoadingWorkflowTemplate} size={"large"}>
            <Form
              form={formWorkflowTemplate}
              layout="horizontal"
              name="formWorkflowTemplate"
              labelCol={{ span: 5 }}
              onFinish={onFinishSaveWorkFlowTemplate}
            >
              <Form.Item name={"id"} hidden />

              <Form.Item
                name="workflowName"
                label="Tên quy trình"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập tên quy trình!"
                  }
                ]}
              >
                <Input placeholder="Nhập tên quy trình" />
              </Form.Item>
              <Form.Item
                name="workflowCode"
                label="Mã"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập mã!"
                  }
                ]}
              >
                <Input disabled={!!idWorkflowTemplate} />
              </Form.Item>
              {/*<Form.Item name="order" label="Thứ tự">*/}
              {/*  <InputNumber />*/}
              {/*</Form.Item>*/}
              <Form.Item name="startWorkflowButton" label="Nút bắt đầu quy trình">
                <Input />
              </Form.Item>
              <Form.Item name="defaultCompletedStatus" label="Trạng Thái Hoàn Thành mặc định">
                <Input />
              </Form.Item>
              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  style={{
                    float: "right"
                  }}
                  block
                  size={"middle"}
                  icon={<SaveOutlined />}
                  loading={isLoadingUpdateWorkflowTemplate || isLoadingInsertWorkflowTemplate}
                >
                  lưu
                </Button>
              </Form.Item>
            </Form>
          </Spin>
        </Col>
        <Col span={24} hidden={!idWorkflowTemplate}>
          <TableWorkFlowStep idWorkflowTemplate={idWorkflowTemplate} />
        </Col>
      </Row>
    </div>
  );
}

export const NewAndUpdateWorkFlow = WithErrorBoundaryCustom(_NewAndUpdateWorkFlow);
