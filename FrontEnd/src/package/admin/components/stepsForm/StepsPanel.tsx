import { LeftCircleOutlined, FileDoneOutlined, RightCircleOutlined } from "@ant-design/icons";
import { Button, Card, Steps } from "antd";
import { useState } from "react";
import WithErrorBoundaryCustom from "~/units/errorBounDary/WithErrorBoundaryCustom";
import "./style.css";

function _StepsPanel(props: {
  steps: { step: number; title: string; content: JSX.Element; icon?: JSX.Element }[];
  progressDot: boolean;
}) {
  const [activeStep, setActiveStep] = useState(0);

  function next() {
    const nextStep = activeStep + 1;
    setActiveStep(nextStep);
  }

  function prev() {
    const prevStep = activeStep - 1;
    setActiveStep(prevStep);
  }

  const onChange = (current: number) => {
    setActiveStep(current);
  };

  return (
    <div className="StepsPanel">
      <Steps
        current={activeStep}
        progressDot={props.progressDot}
        style={{ marginBottom: 50 }}
        items={props.steps}
        onChange={onChange}
      >
        {props.steps.map((item) => (
          <div key={item.step} className={`steps-content ${item.step !== activeStep + 1 && "hidden"}`}>
            {item.content}
          </div>
        ))}
      </Steps>

      <Card bordered={false} className="criclebox h-full">
        {props.steps[activeStep].content}

        <div className="steps-action">
          {activeStep < props.steps.length - 1 && (
            <Button type="primary" onClick={() => next()} style={{ float: "right" }} icon={<RightCircleOutlined />}>
              Tiếp theo
            </Button>
          )}
          {activeStep === props.steps.length - 1 && (
            <Button type="primary" htmlType="submit" style={{ float: "right" }} icon={<FileDoneOutlined />}>
              Hoàn Thành
            </Button>
          )}
          {activeStep > 0 && (
            <Button style={{ margin: "0 8px", float: "left" }} onClick={() => prev()} icon={<LeftCircleOutlined />}>
              Quay lại
            </Button>
          )}
        </div>
      </Card>
    </div>
  );
}
/**
 * @description: StepsPanel
 * @example: <StepsPanel steps={steps} />
 * @example: const steps = [
 * {
 * step: 1,
 * title: "Bước 1",
 * content: <div>Content 1</div>,
 * },
 */
export const StepsPanel = WithErrorBoundaryCustom(_StepsPanel);
