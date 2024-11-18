import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin, InputNumber, Select } from "antd";
const { Option } = Select;
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CategorySalaryLevelDTO } from "@models/CategorySalaryLevelDTO";
import {
  useGetCategorySalaryLevelByIdQuery,
  useUpdateCategorySalaryLevelMutation,
  useInsertCategorySalaryLevelMutation,
} from "@API/services/CategorySalaryLevel.service";
import {useGetListCategorySalaryScaleQuery} from "@API/services/CategorySalaryScale.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategorySalaryLevel (props: IProps) {
  const { setVisible, id } = props;
  const { data: CategorySalaryLevel, isLoading: LoadingCategorySalaryLevel } = useGetCategorySalaryLevelByIdQuery(
    {idCategorySalaryLevel: id!}, {skip: !id});
  const [newCategorySalaryLevel, { isLoading: LoadingInsertCategorySalaryLevel }] = useInsertCategorySalaryLevelMutation();
  const [updateCategorySalaryLevel, { isLoading: LoadingUpdateCategorySalaryLevel }] = useUpdateCategorySalaryLevelMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (CategorySalaryLevel?.payload && id) {
      formRef.setFieldsValue(CategorySalaryLevel?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategorySalaryLevel, formRef, id]);
  const onfinish = async (values: CategorySalaryLevelDTO) => {
    try {
      const result = id
        ? await updateCategorySalaryLevel({
          CategorySalaryLevel: values
        }).unwrap()
        : await newCategorySalaryLevel({ CategorySalaryLevel: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  const { data: ListCategorySalaryScale, isLoading: LoadingListCategorySalaryScale } = useGetListCategorySalaryScaleQuery({
    pageSize: 0,
    pageNumber: 0
  });
  return (
    <div className="NewAndUpdateAllowance">
      <Spin spinning={LoadingCategorySalaryLevel}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item name={"createdDate"} hidden />
              <Form.Item label="Tên" name={"nameSalaryLevel"}>
                <Input />
              </Form.Item>
              <Form.Item label="Số tiền" name={"amount"}>
                <InputNumber
                  formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")}
                  parser={(value) => value!.replace(/\$\s?|(,*)/g, "")}
                />
              </Form.Item>
              <Form.Item label="Ngạch lương" name={"idSalaryScale"}>
                <Select
                  allowClear
                  showSearch
                  loading={LoadingListCategorySalaryScale}
                  optionFilterProp={"label"}
                  options={ListCategorySalaryScale?.listPayload?.map((item) => ({
                    label: item.nameSalaryScale,
                    value: item.id
                  }))}
                />
              </Form.Item>
              <Form.Item label="Loại bậc lương:" name="isCoefficient">
                <Select style={{ width: 200 }} placeholder="Chọn một giá trị">
                  <Option value={true}>Theo hệ số</Option>
                  <Option value={false}>Không theo hệ số</Option>
                </Select>
              </Form.Item>
              <Form.Item label="Hệ số đóng bảo hiểm:" name={"socialSecurityContributionRate"}>
                <Input />
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
                    loading={LoadingInsertCategorySalaryLevel || LoadingUpdateCategorySalaryLevel}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategorySalaryLevel || LoadingUpdateCategorySalaryLevel}
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

export const NewAndUpdateCategorySalaryLevel = WithErrorBoundaryCustom(_NewAndUpdateCategorySalaryLevel);
