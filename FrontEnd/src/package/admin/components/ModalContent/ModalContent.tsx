import React, { useRef, useState } from "react";
import { Divider, Modal, ModalProps, Typography } from "antd";
import WithErrorBoundaryCustom from "~/units/errorBounDary/WithErrorBoundaryCustom";
import { CloseCircleFilled } from "@ant-design/icons";
import type { DraggableData, DraggableEvent } from "react-draggable";
import Draggable from "react-draggable";

interface IProps {
  children: React.ReactNode;
  visible: boolean;
  setVisible: (visible: boolean) => void;
  title: string;
  width?: string;
  top?: number;
  propsRest?: ModalProps;
  afterClose?: () => void;
}

const defaultType: IProps = {
  children: null,
  visible: false,
  setVisible: () => false,
  title: "",
  width: "85%",
  top: 20
};

const _Modal: React.FC<IProps> = ({ afterClose, visible, setVisible, children, title, width, top, ...propsRest }) => {
  const { PrimaryColor } = JSON.parse(localStorage.getItem("setting")!);
  const [disabled, setDisabled] = useState(true);
  const [bounds, setBounds] = useState({ left: 0, top: 0, bottom: 0, right: 0 });
  const draggableRef = useRef<HTMLDivElement>(null);

  const onStart = (_event: DraggableEvent, uiData: DraggableData) => {
    const { clientWidth, clientHeight } = window.document.documentElement;
    const targetRect = draggableRef.current?.getBoundingClientRect();
    if (!targetRect) {
      return;
    }
    setBounds({
      left: -targetRect.left + uiData.x,
      right: clientWidth - (targetRect.right - uiData.x),
      top: -targetRect.top + uiData.y,
      bottom: clientHeight - (targetRect.bottom - uiData.y)
    });
  };
  return (
    <div className="Modal">
      <Modal
        {...propsRest}
        title={
          <div
            style={{
              width: "100%",
              cursor: "move"
            }}
            onMouseOver={() => {
              if (disabled) {
                setDisabled(false);
              }
            }}
            onMouseOut={() => {
              setDisabled(true);
            }}
            // fix eslintjsx-a11y/mouse-events-have-key-events
            // https://github.com/jsx-eslint/eslint-plugin-jsx-a11y/blob/master/docs/rules/mouse-events-have-key-events.md
            onFocus={() => false}
            onBlur={() => false}
            // end
          >
            <Typography.Title level={5}>{title}</Typography.Title>
          </div>
        }
        open={visible}
        onCancel={() => setVisible(false)}
        footer={false}
        width={width}
        style={{ top: top }}
        afterClose={afterClose}
        closeIcon={<CloseCircleFilled style={{ fontSize: 24 }} />}
        modalRender={(modal) => (
          <Draggable disabled={disabled} bounds={bounds} onStart={(event, uiData) => onStart(event, uiData)}>
            <div ref={draggableRef}>{modal}</div>
          </Draggable>
        )}
      >
        <Divider
          style={{
            margin: "20px -24px",
            background: PrimaryColor,
            width: "calc(100% + 48px)",
            height: 16
          }}
        />
        <Divider />
        {children}
      </Modal>
    </div>
  );
};
_Modal.defaultProps = defaultType;
/**
 * @param  {React.ReactNode} children content of modal
 * @param  {boolean} visible visible of modal
 * @param  {(visible:boolean)=>void} setVisible set visible of modal
 * @param  {string} title title of modal
 * @param  {string} width width of modal
 * @param  {number} top top of modal.
 * @returns JSX.Element
 * @example
 *<ModalContent visible={true} setVisible={setVisible} title="title" width="85%" top={20}>
 *   <div>content</div>
 *</ModalContent>
 *@description Modal content
 *@author khanhdoan693@gmail.com
 */
export const ModalContent = WithErrorBoundaryCustom(_Modal);
