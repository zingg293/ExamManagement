import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useGetEmployeeAndBenefitsQuery, useUpdateEmployeeAndBenefitsMutation } from "@API/services/Employee.service";
import { Button, Col, Form, InputNumber, Row, Select, Space, Spin, Typography } from "antd";
import { HandleError } from "@admin/components";
import { useGetListCategoryCompensationBenefitsQuery } from "@API/services/CategoryCompensationBenefits.service";
import { formatMoney } from "~/units";
import { CheckCircleOutlined, MinusCircleOutlined, PlusOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect } from "react";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _EmployeeBenefit(props: IProps) {
  const { setVisible, id } = props;
  const { data: dataEmployee, isLoading: isLoadingEmployee } = useGetEmployeeAndBenefitsQuery(
    {
      idEmployee: id!
    },
    {
      skip: !id
    }
  );
  const { data: dataEmployeeBenefits, isLoading: isLoadingEmployeeBenefits } =
    useGetListCategoryCompensationBenefitsQuery(
      {
        pageSize: 0,
        pageNumber: 0
      },
      {
        skip: !id
      }
    );
  const [UpdateEmployeeAndBenefit, { isLoading: isLoadingUpdateEmployeeAndBenefit }] =
    useUpdateEmployeeAndBenefitsMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    if (dataEmployee?.payload) {
      formRef.setFieldsValue({
        employeeBenefitsList: dataEmployee?.payload?.employeeBenefits
      });
    }
  }, [dataEmployee?.payload, formRef]);
  const onfinish = async (values: any) => {
    try {
      const result = await UpdateEmployeeAndBenefit({
        employeeBenefits:
          values.employeeBenefitsList?.length === 0
            ? [
                {
                  id: null,
                  idEmployee: id,
                  idCategoryCompensationBenefits: null,
                  quantity: null
                }
              ]
            : values
      }).unwrap();
      if (result.success) {
        setVisible(false);
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="EmployeeBenefit">
      <Spin spinning={isLoadingEmployee} size="large">
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
            <Form layout={"vertical"} labelCol={{ span: 6 }} form={formRef} onFinish={onfinish}>
              <Form.List name={"employeeBenefitsList"}>
                {(fields, { add, remove }) => (
                  <>
                    {fields.map((field, index) => (
                      <Row gutter={[16, 0]} key={field.key} align={"stretch"}>
                        <Col span={1}>{<Typography.Text strong>{index + 1}</Typography.Text>}</Col>
                        <Col span={0}>
                          <Form.Item name={[field.name, "idEmployee"]} hidden initialValue={id} />
                        </Col>
                        <Col span={0}>
                          <Form.Item
                            name={[field.name, "id"]}
                            hidden
                            initialValue={"00000000-0000-0000-0000-000000000000"}
                          />
                        </Col>
                        <Col span={15}>
                          <Form.Item
                            {...field}
                            name={[field.name, "idCategoryCompensationBenefits"]}
                            rules={[{ required: true, message: "Vui lòng chọn" }]}
                          >
                            <Select
                              showSearch={true}
                              placeholder="Chọn phúc lợi cho nhân viên"
                              loading={isLoadingEmployeeBenefits}
                              options={dataEmployeeBenefits?.listPayload?.map((item) => ({
                                label: `${item.name} - ${formatMoney(item.amountMoney as number)}`,
                                value: item.id
                              }))}
                            />
                          </Form.Item>
                        </Col>
                        <Col span={8}>
                          <Space align={"start"}>
                            <Form.Item {...field} name={[field.name, "quantity"]}>
                              <InputNumber defaultValue={0} min={0} />
                            </Form.Item>
                            <MinusCircleOutlined onClick={() => remove(field.name)} />
                          </Space>
                        </Col>
                      </Row>
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
                    loading={isLoadingUpdateEmployeeAndBenefit}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={isLoadingUpdateEmployeeAndBenefit}
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
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const EmployeeBenefit = WithErrorBoundaryCustom(_EmployeeBenefit);
