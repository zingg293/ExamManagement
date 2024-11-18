// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-nocheck
import React, { useEffect, useState } from "react";
import { App, Button, Card, Col, Divider, Input, Modal, Popconfirm, Row, Space, Tag, Typography } from "antd";
import {
  ClockCircleFilled,
  CloseCircleFilled,
  CloseCircleOutlined,
  PlusOutlined,
  UnorderedListOutlined
} from "@ant-design/icons";
import dayjs from "dayjs";
import { useDragAndDropWithStrictMode } from "@hooks/useDragAndDropWithStrictMode";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { DragDropContext, Droppable, DropResult, Draggable } from "react-beautiful-dnd";
type Task = {
  id: string;
  content: string;
  time: Date;
};

type Column = "todo" | "process" | "done" | "pending";

type TasksState = {
  [key in Column]: Task[];
};

const columns: { [key in Column]: string } = {
  todo: "To Do",
  process: "In Process",
  done: "Done",
  pending: "Pending"
};
const initialValue: TasksState = {
  todo: [
    {
      id: "1",
      content: "Hi there ! your task here, you can add more task by click button below, and drag to other column",
      time: dayjs().toDate()
    }
  ],
  process: [],
  done: [],
  pending: []
};

const _ToDoListDragAndDrop: React.FC = () => {
  const MyTask: TasksState = JSON.parse(localStorage.getItem("tasks")!) || initialValue;
  const { isDragAndDropEnabled } = useDragAndDropWithStrictMode();
  const [tasks, setTasks] = useState<TasksState>(MyTask);
  const { modal } = App.useApp();
  useEffect(() => {
    localStorage.setItem("tasks", JSON.stringify(tasks));
  }, [tasks]);

  const handleDragEnd = (result: DropResult) => {
    const { source, destination } = result;
    if (!destination) return;

    const sourceColumn = source.droppableId as Column;
    const destinationColumn = destination.droppableId as Column;
    const draggedTask = tasks[sourceColumn][source.index];

    if (sourceColumn === destinationColumn) {
      const newTasks = [...tasks[sourceColumn]];
      newTasks.splice(source.index, 1);
      newTasks.splice(destination.index, 0, draggedTask);
      setTasks({ ...tasks, [sourceColumn]: newTasks });
    } else {
      const sourceTasks = [...tasks[sourceColumn]];
      const destinationTasks = [...tasks[destinationColumn]];

      sourceTasks.splice(source.index, 1);
      destinationTasks.splice(destination.index, 0, draggedTask);

      setTasks({
        ...tasks,
        [sourceColumn]: sourceTasks,
        [destinationColumn]: destinationTasks
      });
    }
  };
  const renderTitle = (column: string) => {
    switch (column) {
      case "To Do":
        return <Tag color="red">{column}</Tag>;
      case "In Process":
        return <Tag color="blue">{column}</Tag>;
      case "Done":
        return <Tag color="green">{column}</Tag>;
      default:
        return <Tag color="orange">{column}</Tag>;
    }
  };
  const AddTask = (columnId: string) => {
    const onClose = () => {
      Modal.destroyAll();
    };
    modal.success({
      title: "Add Task to " + columns[columnId as Column],
      width: "50%",
      closeIcon: <CloseCircleFilled style={{ fontSize: 24 }} />,
      content: (
        <Row gutter={16}>
          <Divider />
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Input.TextArea
              placeholder={`Add your task`}
              onPressEnter={(e) => {
                const newTaskId = `${Math.floor(Math.random() * 10000000)}`;
                const newTask = {
                  id: newTaskId,
                  content: e.currentTarget.value,
                  time: dayjs().toDate()
                };
                setTasks({ ...tasks, [columnId as Column]: [...tasks[columnId as Column], newTask] });
                e.currentTarget.value = "";
                onClose();
              }}
            />
          </Col>
        </Row>
      ),
      closable: true,
      maskClosable: true,
      footer: null
    });
  };

  return (
    <DragDropContext onDragEnd={handleDragEnd}>
      <Row gutter={[16, 16]}>
        {Object.entries(columns).map(([columnId, column]) => (
          <Col key={columnId} xs={24} md={24 / 2} sm={24 / 2} lg={24 / 2} xl={24 / 4}>
            <Card
              type="inner"
              headStyle={{
                backgroundColor: "rgba(69,69,70,0.44)"
              }}
              title={renderTitle(column)}
              bodyStyle={{
                backgroundColor: "rgba(69,69,70,0.44)",
                maxHeight: "50vh",
                borderRadius: "unset"
              }}
              extra={<UnorderedListOutlined />}
              actions={[
                <Button key={"addTask"} type="link" icon={<PlusOutlined />} onClick={() => AddTask(columnId)}>
                  Add a task
                </Button>
              ]}
            >
              {isDragAndDropEnabled && (
                <Droppable droppableId={columnId} key={columnId}>
                  {(provided) => (
                    <Space
                      style={{
                        width: "100%",
                        minHeight: 10
                      }}
                      direction="vertical"
                      ref={provided.innerRef}
                      {...provided.droppableProps}
                    >
                      {tasks[columnId as Column]?.map((task, index) => (
                        <Draggable key={task.id} draggableId={task.id} index={index}>
                          {(provided) => (
                            <Card
                              type={"inner"}
                              ref={provided.innerRef}
                              {...provided.draggableProps}
                              {...provided.dragHandleProps}
                              style={{
                                cursor: "pointer",
                                ...provided.draggableProps.style
                              }}
                              size="small"
                            >
                              <Space
                                direction={"vertical"}
                                style={{
                                  width: "100%"
                                }}
                              >
                                <Typography.Paragraph>{task.content}</Typography.Paragraph>
                                <Space
                                  direction={"horizontal"}
                                  style={{
                                    width: "100%",
                                    justifyContent: "space-between"
                                  }}
                                >
                                  <small
                                    style={{
                                      color: "rgba(146,145,145,0.65)"
                                    }}
                                  >
                                    <ClockCircleFilled /> {dayjs(task.time).format("DD/MM/YYYY HH:mm")}
                                  </small>
                                  <Popconfirm
                                    title={"Are you sure to delete this task?"}
                                    onConfirm={() => {
                                      const newTasks = [...tasks[columnId as Column]];
                                      newTasks.splice(index, 1);
                                      setTasks({ ...tasks, [columnId as Column]: newTasks });
                                    }}
                                  >
                                    <CloseCircleOutlined />
                                  </Popconfirm>
                                </Space>
                              </Space>
                            </Card>
                          )}
                        </Draggable>
                      ))}
                      {provided.placeholder}
                    </Space>
                  )}
                </Droppable>
              )}
            </Card>
          </Col>
        ))}
      </Row>
    </DragDropContext>
  );
};

export const ToDoListDragAndDrop = WithErrorBoundaryCustom(_ToDoListDragAndDrop);
