import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useGetListBusinessTripEmployeeByIdBusinessTripQuery,
  useInsertBusinessTripEmployeeByListMutation
} from "@API/services/BusinessTripEmployeeApis.service";
import { App, Button, Card, Checkbox, Col, Form, Row, Select, Space, Spin, Typography } from "antd";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, MinusCircleOutlined, PlusOutlined, RetweetOutlined } from "@ant-design/icons";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { useEffect } from "react";

interface IProps {
  idBusinessTrip: string;
  afterSave?: () => void;
}

function _NewAndUpdateBusinessTripEmployee(props: IProps) {
  const { notification } = App.useApp();
  const { idBusinessTrip, afterSave } = props;
  const { data: dataEmployee, isLoading: isLoadingEmployee } = useGetListEmployeeQuery(
    { pageNumber: 0, pageSize: 0 },
    {
      skip: !idBusinessTrip
    }
  );
  const { data: dataUnit } = useGetListUnitQuery(
    { pageNumber: 0, pageSize: 0 },
    {
      skip: !idBusinessTrip
    }
  );
  const { data: dataBusinessTripEmployee, isLoading: isLoadingBusinessTripEmployee } =
    useGetListBusinessTripEmployeeByIdBusinessTripQuery(
      {
        idBusinessTrip
      },
      {
        skip: !idBusinessTrip
      }
    );
  const [insertBusinessTripEmployeeByList, { isLoading: isLoadingInsertBusinessTripEmployeeByList }] =
    useInsertBusinessTripEmployeeByListMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    formRef.resetFields();
    if (dataBusinessTripEmployee?.listPayload) {
      formRef.setFieldsValue({
        BusinessTripEmployees: dataBusinessTripEmployee?.listPayload?.map((item) => ({
          id: item.id,
          idBusinessTrip: item.idBusinessTrip,
          idEmployee: item.idEmployee,
          captain: item.captain
        }))
      });
    } else {
      formRef.resetFields();
    }
  }, [dataBusinessTripEmployee?.listPayload, formRef]);

  const onFinish = async (values: any) => {
    try {
      const checkOnlyCaptain = values.BusinessTripEmployees.filter((item: any) => item.captain === true);
      if (checkOnlyCaptain.length > 1) {
        return notification.info({
          message: "Thông báo",
          description: "Chỉ được chọn 1 trưởng nhóm"
        });
      }
      const result = await insertBusinessTripEmployeeByList({
        businessTripEmployees: values.BusinessTripEmployees
      }).unwrap();
      if (result.success) {
        afterSave && afterSave();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateBusinessTripEmployee">
      <Typography.Title />
      <Spin spinning={isLoadingBusinessTripEmployee}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Card>
              <Form layout={"vertical"} form={formRef} onFinish={onFinish}>
                <Form.List name="BusinessTripEmployees">
                  {(fields, { add, remove }) => (
                    <>
                      {fields.map(({ key, name, ...restField }) => (
                        <Space
                          key={key}
                          style={{ display: "flex", marginBottom: 8, width: "100%" }}
                          align="baseline"
                          wrap
                        >
                          <Form.Item name={[name, "idBusinessTrip"]} initialValue={idBusinessTrip} hidden={true} />
                          <Form.Item
                            name={[name, "id"]}
                            initialValue={"00000000-0000-0000-0000-000000000000"}
                            hidden={true}
                          />
                          <Form.Item>
                            <Typography.Text strong>{name + 1}. </Typography.Text>
                          </Form.Item>
                          <Form.Item
                            {...restField}
                            name={[name, "idEmployee"]}
                            rules={[{ required: true, message: "Vui lòng chọn nhân viên" }]}
                          >
                            <Select
                              placeholder={"Chọn nhân viên"}
                              style={{ width: 600 }}
                              showSearch
                              optionFilterProp={"label"}
                              loading={isLoadingEmployee}
                              options={dataEmployee?.listPayload?.map((item) => ({
                                label:
                                  item.name +
                                  " - " +
                                  item.email +
                                  " - " +
                                  item.code +
                                  " - " +
                                  dataUnit?.listPayload?.find((unit) => unit.id === item.idUnit)?.unitName,
                                value: item.id
                              }))}
                            />
                          </Form.Item>
                          <Form.Item
                            {...restField}
                            name={[name, "captain"]}
                            valuePropName="checked"
                            initialValue={false}
                          >
                            <Checkbox>Trưởng nhóm</Checkbox>
                          </Form.Item>
                          <MinusCircleOutlined onClick={() => remove(name)} />
                        </Space>
                      ))}
                      <Form.Item>
                        <Button type="dashed" onClick={() => add()} block icon={<PlusOutlined />}>
                          Thêm
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
                    <Button
                      type="default"
                      htmlType="reset"
                      loading={isLoadingInsertBusinessTripEmployeeByList}
                      icon={<RetweetOutlined />}
                    >
                      Xóa
                    </Button>
                    <Button
                      type="primary"
                      htmlType="submit"
                      loading={isLoadingInsertBusinessTripEmployeeByList}
                      icon={<CheckCircleOutlined />}
                      style={{
                        float: "right"
                      }}
                    >
                      Lưu
                    </Button>
                  </Space>
                </Form.Item>
              </Form>
            </Card>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const NewAndUpdateBusinessTripEmployee = WithErrorBoundaryCustom(_NewAndUpdateBusinessTripEmployee);
