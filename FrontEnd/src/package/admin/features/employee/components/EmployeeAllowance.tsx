import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { useGetListAllowanceQuery } from "@API/services/Allowance.service";
import { useGetEmployeeAndAllowanceQuery, useInsertEmployeeAllowanceMutation } from "@API/services/Employee.service";
import { Button, Col, Form, Row, Select, Space, Spin, Typography } from "antd";
import { HandleError } from "@admin/components";
import { useEffect } from "react";
import { EmployeeDTO } from "@models/employeeDTO";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { formatMoney } from "~/units";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _EmployeeAllowance(props: IProps) {
  const { setVisible, id } = props;
  const { data, isLoading } = useGetListAllowanceQuery(
    { pageSize: 0, pageNumber: 0 },
    {
      skip: !id
    }
  );
  const { data: DataEmployee, isLoading: isLoadingEmployee } = useGetEmployeeAndAllowanceQuery(
    {
      idEmployee: id!
    },
    {
      skip: !id
    }
  );
  const [InsertEmployeeAllowance, { isLoading: isLoadingInsertEmployeeAllowance }] =
    useInsertEmployeeAllowanceMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    if (DataEmployee?.payload && id) {
      formRef.setFieldsValue({
        idEmployeeAllowance: DataEmployee?.payload?.allowances?.map((item) => item.id),
        employee: DataEmployee?.payload?.employee?.id
      });
    } else {
      formRef.resetFields();
    }
  }, [DataEmployee?.payload, formRef, id]);

  const onfinish = async (values: Partial<EmployeeDTO>) => {
    try {
      const result = await InsertEmployeeAllowance({
        employeeAllowance: values
      }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="EmployeeAllowance">
      <Spin spinning={isLoadingEmployee} size="large">
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24} className="mb-24">
            <Typography.Text strong>
              {DataEmployee?.payload?.employee?.name} - {DataEmployee?.payload?.employee?.code}
            </Typography.Text>
            <Form layout={"vertical"} labelCol={{ span: 6 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"employee"} hidden />
              <Form.Item name={"idEmployeeAllowance"} label={"Chọn phụ cấp"}>
                <Select
                  loading={isLoading}
                  mode={"multiple"}
                  maxTagCount={"responsive"}
                  showSearch
                  placeholder="Chọn phụ cấp"
                  optionFilterProp="label"
                  options={data?.listPayload?.map((item) => ({
                    label: `${item.name} - ${formatMoney(item.amount || 0)}`,
                    value: item.id
                  }))}
                />
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
                    loading={isLoadingInsertEmployeeAllowance}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={isLoadingInsertEmployeeAllowance}
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

export const EmployeeAllowance = WithErrorBoundaryCustom(_EmployeeAllowance);
