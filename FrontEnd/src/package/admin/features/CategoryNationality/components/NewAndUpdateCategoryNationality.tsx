import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CategoryNationalityDTO } from "@models/CategoryNationalityDTO";
import {
  useGetCategoryNationalityByIdQuery,
  useInsertCategoryNationalityMutation,
  useUpdateCategoryNationalityMutation
} from "@API/services/CategoryNationality.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryNationality(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryNationality, isLoading: LoadingCategoryNationality } = useGetCategoryNationalityByIdQuery(
    { idCategoryNationality: id! },
    { skip: !id }
  );
  const [newCategoryNationality, { isLoading: LoadingInsertCategoryNationaly }] =
    useInsertCategoryNationalityMutation();
  const [updateCategoryNationality, { isLoading: LoadingUpdateCategoryNationaly }] =
    useUpdateCategoryNationalityMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (CategoryNationality?.payload && id) {
      formRef.setFieldsValue(CategoryNationality?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryNationality, formRef, id]);
  const onfinish = async (values: CategoryNationalityDTO) => {
    try {
      const result = id
        ? await updateCategoryNationality({
            CategoryNationality: values
          }).unwrap()
        : await newCategoryNationality({ CategoryNationality: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateAllowance">
      <Spin spinning={LoadingCategoryNationality}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item label="Tên" name={"nameNationality"}>
                <Input />
              </Form.Item>
              {/*<Form.Item label="Số tiền" name={"amount"}>*/}
              {/*  <InputNumber*/}
              {/*    formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")}*/}
              {/*    parser={(value) => value!.replace(/\$\s?|(,*)/g, "")}*/}
              {/*  />*/}
              {/*</Form.Item>*/}
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
                    loading={LoadingInsertCategoryNationaly || LoadingUpdateCategoryNationaly}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryNationaly || LoadingUpdateCategoryNationaly}
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

export const NewAndUpdateCategoryNationality = WithErrorBoundaryCustom(_NewAndUpdateCategoryNationality);
