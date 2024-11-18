import React, { useEffect, useState } from "react";
import { Button, Modal, Progress, Typography } from "antd";
import { MyIcon } from "@admin/components";

interface ModalProgressProps {
  start: boolean;
  title: string;
}

const maskStyle = {
  backgroundColor: "rgba(0, 0, 0, 0.5)",
  backdropFilter: "blur(5px)"
};
/**
 * @param {boolean} start progress if true else stop progress
 * @param {string} title of modal
 * @returns {ReactNode}
 * @example <ModalProgress start={true or false} title="Đang tải dữ liệu" />
 * @description modal progress to 0% to 100%
 * @author khanhdoan693@gmail.com
 */
export const ModalProgress: React.FC<ModalProgressProps> = ({ start, title }) => {
  const [completed, setCompleted] = useState<boolean>(false);
  const [percent, setPercent] = useState<number>(0);
  const [visible, setVisible] = useState<boolean>(false);
  useEffect(() => {
    if (visible) {
      const interval = setInterval(() => {
        setPercent((prev) => {
          if (prev === 99) return prev;
          if (prev === 100) {
            setCompleted(true);
          }
          return prev + 1;
        });
      }, 1000);
      const interval2 = setInterval(() => {
        setPercent((prev) => {
          if (prev + 15 >= 99) return prev;
          return prev + 15;
        });
      }, 10000);
      return () => {
        clearInterval(interval);
        clearInterval(interval2);
      };
    }
  }, [visible]);
  useEffect(() => {
    if (start) {
      setPercent(0);
      setVisible(true);
    } else {
      setPercent(100);
    }
  }, [start]);

  const handleModalClose = () => {
    if (completed) {
      setVisible(false);
      setPercent(0);
    }
  };

  return (
    <Modal
      title={
        <Typography.Title level={5} style={{ textAlign: "center" }}>
          {title}
        </Typography.Title>
      }
      open={visible}
      footer={null}
      closable={false}
      maskClosable={false}
      maskStyle={maskStyle}
      onCancel={handleModalClose}
      afterClose={() => setCompleted(false)}
    >
      <div style={{ textAlign: "center", marginTop: 30 }}>
        <Progress
          percent={percent}
          status={completed ? "success" : "active"}
          size={200}
          strokeColor={{ "0%": "#108ee9", "100%": "#87d068" }}
          type={"dashboard"}
        />
        <Typography.Title level={5}>{completed ? "Quá trình hoàn thành" : "Đang xử lý quá trình..."}</Typography.Title>
        <Button
          type="primary"
          onClick={handleModalClose}
          hidden={!completed}
          icon={<MyIcon type={"icon-check-circle-fill"} />}
        >
          Hoàn thành
        </Button>
      </div>
    </Modal>
  );
};
