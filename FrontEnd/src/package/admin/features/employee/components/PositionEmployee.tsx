import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useGetListPositionEmployeeByIdEmployeeQuery,
  useInsertPositionEmployeeByListMutation,
  useUpdatePositionEmployeeByListMutation
} from "@API/services/PositionEmployeeApis.service";
import { Button, Checkbox, Col, Form, Row, Select, Space, Spin, Tabs, Typography } from "antd";
import { MinusCircleOutlined, PlusOutlined } from "@ant-design/icons";
import { useGetListCategoryPositionAvailableQuery } from "@API/services/CategoryPositionApis.service";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";
import { HandleError } from "@admin/components";
import { useEffect } from "react";

interface IProps {
  idEmployee: string;
}

function _PositionEmployee(props: IProps) {
  const { idEmployee } = props;
  const { data: ListCategoryPosition, isLoading: LoadingListCategoryPosition } =
    useGetListCategoryPositionAvailableQuery({
      pageSize: 0,
      pageNumber: 0
    });
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitAvailableQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListPositionEmployee, isLoading: LoadingListPositionEmployee } =
    useGetListPositionEmployeeByIdEmployeeQuery(
      {
        idEmployee
      },
      { skip: !idEmployee }
    );
  const [InsertPositionEmployee, { isLoading: LoadingInsertPositionEmployee }] =
    useInsertPositionEmployeeByListMutation();
  const [UpdatePositionEmployee, { isLoading: LoadingUpdatePositionEmployee }] =
    useUpdatePositionEmployeeByListMutation();
  const [formRefInsert] = Form.useForm();
  const [formRefUpdate] = Form.useForm();
  useEffect(() => {
    if (ListPositionEmployee?.listPayload?.length) {
      formRefUpdate.setFieldsValue({
        listPositionEmployee: ListPositionEmployee?.listPayload
      });
    }
  }, [formRefUpdate, ListPositionEmployee]);
  const onFinishInsert = async (values: any) => {
    try {
      const res = await InsertPositionEmployee({
        listPositionEmployee: values.listPositionEmployee
      }).unwrap();
      if (res.success) {
        formRefInsert.resetFields();
        formRefInsert.setFieldsValue({
          listPositionEmployee: []
        });
      }
    } catch (error: any) {
      await HandleError(error);
    }
  };
  const onFinishUpdate = async (values: any) => {
    try {
      await UpdatePositionEmployee({
        listPositionEmployee: values.listPositionEmployee
      }).unwrap();
    } catch (error: any) {
      await HandleError(error);
    }
  };

  return (
    <div className="PositionEmployee">
      <Spin spinning={LoadingListPositionEmployee} size={"large"}>
        <Typography.Title />
        <Tabs defaultActiveKey="1" type={"card"}>
          <Tabs.TabPane tab="Thêm chức vụ" key="1">
            <Form form={formRefInsert} layout="vertical" onFinish={onFinishInsert}>
              <Form.List name={"listPositionEmployee"}>
                {(fields, { add, remove }) => (
                  <>
                    {fields.map((field) => (
                      <Row gutter={[16, 0]} key={field.key} align={"stretch"}>
                        <Col span={0}>
                          <Form.Item name={[field.name, "idEmployee"]} hidden initialValue={idEmployee} />
                        </Col>
                        <Col span={8}>
                          <Form.Item
                            {...field}
                            name={[field.name, "idPosition"]}
                            rules={[
                              {
                                required: true,
                                message: "Chọn chức vụ"
                              }
                            ]}
                          >
                            <Select
                              showSearch
                              placeholder="Chọn chức vụ"
                              loading={LoadingListCategoryPosition}
                              options={ListCategoryPosition?.listPayload?.map((item) => ({
                                label: item.positionName,
                                value: item.id
                              }))}
                              optionFilterProp={"label"}
                            />
                          </Form.Item>
                        </Col>
                        <Col span={8}>
                          <Form.Item
                            {...field}
                            name={[field.name, "idUnit"]}
                            rules={[
                              {
                                required: true,
                                message: "Chọn phòng ban"
                              }
                            ]}
                          >
                            <Select
                              showSearch
                              placeholder="Chọn phòng ban"
                              loading={LoadingListUnit}
                              options={ListUnit?.listPayload?.map((item) => ({
                                label: item.unitName + " - " + item.unitCode,
                                value: item.id
                              }))}
                              optionFilterProp={"label"}
                            />
                          </Form.Item>
                        </Col>
                        <Col span={8}>
                          <Space align={"start"}>
                            <Form.Item {...field} name={[field.name, "isHeadcount"]} valuePropName={"checked"}>
                              <Checkbox> Chức vụ chính</Checkbox>
                            </Form.Item>
                            <MinusCircleOutlined onClick={() => remove(field.name)} />
                          </Space>
                        </Col>
                      </Row>
                    ))}
                    <Form.Item>
                      <Button type="dashed" onClick={() => add()} block icon={<PlusOutlined />}>
                        Thêm chức vụ
                      </Button>
                    </Form.Item>
                  </>
                )}
              </Form.List>
              <Form.Item>
                <Space
                  style={{
                    width: "100%",
                    justifyContent: "flex-end"
                  }}
                >
                  <Button type="default" htmlType="reset" loading={LoadingInsertPositionEmployee}>
                    Xóa
                  </Button>
                  <Button type="primary" htmlType="submit" loading={LoadingInsertPositionEmployee}>
                    Lưu
                  </Button>
                </Space>
              </Form.Item>
            </Form>
          </Tabs.TabPane>
          <Tabs.TabPane tab="Chỉnh sửa chức vụ" key="2">
            <Form form={formRefUpdate} layout="vertical" onFinish={onFinishUpdate}>
              <Form.List name={"listPositionEmployee"}>
                {(fields) => (
                  <>
                    {fields.map((field) => (
                      <Row gutter={[16, 0]} key={field.key} align={"stretch"}>
                        <Col span={0}>
                          <Form.Item name={[field.name, "id"]} hidden />
                        </Col>
                        <Col span={0}>
                          <Form.Item name={[field.name, "idEmployee"]} hidden initialValue={idEmployee} />
                        </Col>
                        <Col span={8}>
                          <Form.Item
                            {...field}
                            name={[field.name, "idPosition"]}
                            rules={[
                              {
                                required: true,
                                message: "Chọn chức vụ"
                              }
                            ]}
                          >
                            <Select
                              showSearch
                              placeholder="Chọn chức vụ"
                              loading={LoadingListCategoryPosition}
                              options={ListCategoryPosition?.listPayload?.map((item) => ({
                                label: item.positionName,
                                value: item.id
                              }))}
                              optionFilterProp={"label"}
                            />
                          </Form.Item>
                        </Col>
                        <Col span={8}>
                          <Form.Item
                            {...field}
                            name={[field.name, "idUnit"]}
                            rules={[
                              {
                                required: true,
                                message: "Chọn phòng ban"
                              }
                            ]}
                          >
                            <Select
                              showSearch
                              placeholder="Chọn phòng ban"
                              loading={LoadingListUnit}
                              options={ListUnit?.listPayload?.map((item) => ({
                                label: item.unitName + " - " + item.unitCode,
                                value: item.id
                              }))}
                              optionFilterProp={"label"}
                            />
                          </Form.Item>
                        </Col>
                        <Col span={8}>
                          <Space align={"start"}>
                            <Form.Item {...field} name={[field.name, "isHeadcount"]} valuePropName={"checked"}>
                              <Checkbox> Chức vụ chính</Checkbox>
                            </Form.Item>
                          </Space>
                        </Col>
                      </Row>
                    ))}
                  </>
                )}
              </Form.List>
              <Form.Item>
                <Space
                  style={{
                    width: "100%",
                    justifyContent: "flex-end"
                  }}
                >
                  <Button type="default" htmlType="reset" loading={LoadingUpdatePositionEmployee}>
                    Xóa
                  </Button>
                  <Button type="primary" htmlType="submit" loading={LoadingUpdatePositionEmployee}>
                    Lưu
                  </Button>
                </Space>
              </Form.Item>
            </Form>
          </Tabs.TabPane>
        </Tabs>
      </Spin>
    </div>
  );
}

export const PositionEmployee = WithErrorBoundaryCustom(_PositionEmployee);
