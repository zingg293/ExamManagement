import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useGetWorkflowStepByIdQuery,
  useInsertWorkflowStepMutation,
  useUpdateWorkflowStepMutation
} from "@API/services/WorkflowStepApis.service";
import { Button, Col, ColorPicker, Form, Input, InputNumber, Radio, Row, Select, Space, Spin, Switch, Tag } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { useGetListRoleQuery } from "@API/services/Role.service";
import { RetweetOutlined } from "@ant-design/icons";

interface NewAndUpdateWorkFlowStepProps {
  setVisible: (value: boolean) => void;
  idWorkFlowStep?: string;
  idWorkFlowTemplate?: string;
}

function _NewAndUpdateWorkFlowStep(props: NewAndUpdateWorkFlowStepProps) {
  const { setVisible, idWorkFlowStep, idWorkFlowTemplate } = props;
  const { data: WorkflowStep, isLoading: LoadingWorkflowStep } = useGetWorkflowStepByIdQuery(
    { idWorkflowStep: idWorkFlowStep! },
    { skip: !idWorkFlowStep }
  );
  const [UpdateWorkflowStep, { isLoading: LoadingUpdateWorkflowStep }] = useUpdateWorkflowStepMutation();
  const [NewWorkflowStep, { isLoading: LoadingNewWorkflowStep }] = useInsertWorkflowStepMutation();
  const { data: ListUnit, isLoading: isLoadingListUnit } = useGetListUnitQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListRole, isLoading: isLoadingListRole } = useGetListRoleQuery({ pageSize: 0, pageNumber: 0 });
  const [form] = Form.useForm();
  useEffect(() => {
    if (WorkflowStep?.payload && idWorkFlowStep) {
      form.setFieldsValue(WorkflowStep?.payload);
    } else {
      form.resetFields();
    }
  }, [WorkflowStep?.payload, form, idWorkFlowStep]);
  const handleUpdateWorkflowStep = async (values: any) => {
    try {
      values.templateId ??= idWorkFlowTemplate || "";
      const res = idWorkFlowStep
        ? await UpdateWorkflowStep({ workflowStep: values }).unwrap()
        : await NewWorkflowStep({ workflowStep: values }).unwrap();
      if (res) {
        setVisible(false);
        form.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div
      className="NewAndUpdateWorkFlowStep"
      style={{
        width: 700
      }}
    >
      <Spin spinning={LoadingWorkflowStep} size={"large"}>
        <Row>
          <Col span={24}>
            <Form form={form} layout="vertical" onFinish={handleUpdateWorkflowStep}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"templateId"} hidden />
              <Form.Item
                name="stepName"
                label="Tên bước"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập tên bước!"
                  }
                ]}
              >
                <Input />
              </Form.Item>
              <Form.Item
                name="order"
                label="Thứ tự bước"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập thứ tự bước!"
                  }
                ]}
              >
                <InputNumber />
              </Form.Item>
              <Form.Item name="allowTerminated" label="Có cho phép được hủy ở bước này không ?" valuePropName="checked">
                <Switch checkedChildren={"Được phép"} unCheckedChildren={"Không được phép"} />
              </Form.Item>
              <Form.Item name="rejectName" label="Trạng thái từ chối">
                <Input />
              </Form.Item>
              <Form.Item name="outCome" label="Trạng thái kết thúc">
                <Input />
              </Form.Item>
              <Form.Item name="processWorkflowButton" label="Nút phê duyệt">
                <Input />
              </Form.Item>
              <Form.Item name="statusColor" label="Màu sắc">
                <ColorPicker
                  format={"hex"}
                  showText
                  size={"large"}
                  onChange={(color, hex) => form.setFieldValue("statusColor", hex)}
                  presets={[
                    {
                      label: "Recommended",
                      colors: [
                        "#F5222D",
                        "#FA8C16",
                        "#FADB14",
                        "#8BBB11",
                        "#52C41A",
                        "#13A8A8",
                        "#1677FF",
                        "#2F54EB",
                        "#722ED1",
                        "#EB2F96",
                        "#F5222D4D",
                        "#FA8C164D",
                        "#FADB144D",
                        "#8BBB114D",
                        "#52C41A4D",
                        "#13A8A84D",
                        "#1677FF4D",
                        "#2F54EB4D",
                        "#722ED14D",
                        "#EB2F964D"
                      ]
                    }
                  ]}
                />
              </Form.Item>
              <Form.Item
                name="idRoleAssign"
                label="Quyền xử lý"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập quyền xử lý!"
                  }
                ]}
              >
                <Select
                  showSearch
                  loading={isLoadingListRole}
                  options={ListRole?.listPayload?.map((x) => ({ label: x.roleName, value: x.id }))}
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item
                name="idUnitAssign"
                label="Phòng ban xử lý"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập phòng ban xử lý!"
                  }
                ]}
              >
                <Select
                  showSearch
                  allowClear
                  loading={isLoadingListUnit}
                  options={ListUnit?.listPayload?.map((x) => ({ label: x.unitName, value: x.id }))}
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item
                name="isDirectUnit"
                label="Phòng ban quản lý trực tiếp duyệt"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập phòng ban quản lý trực tiếp duyệt!"
                  }
                ]}
                initialValue={false}
              >
                <Radio.Group>
                  <Radio value={true}>
                    <Tag color={"green-inverse"}>Đồng ý</Tag>
                  </Radio>
                  <Radio value={false}>
                    <Tag color={"red-inverse"}>Không đồng ý</Tag>
                  </Radio>
                </Radio.Group>
              </Form.Item>
              <Form.Item>
                <Space
                  style={{
                    width: "100%",
                    justifyContent: "flex-end"
                  }}
                >
                  <Button
                    type="default"
                    htmlType="reset"
                    loading={LoadingUpdateWorkflowStep || LoadingNewWorkflowStep}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingUpdateWorkflowStep || LoadingNewWorkflowStep}
                    block
                  >
                    Lưu
                  </Button>
                </Space>
              </Form.Item>
            </Form>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const NewAndUpdateWorkFlowStep = WithErrorBoundaryCustom(_NewAndUpdateWorkFlowStep);
