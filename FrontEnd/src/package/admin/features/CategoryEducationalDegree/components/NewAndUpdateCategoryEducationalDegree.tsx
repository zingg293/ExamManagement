import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect } from "react";
import { HandleError } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CategoryEducationalDegreeDTO } from "@models/CategoryEducationalDegreeDTO";
import {
  useGetCategoryEducationalDegreeByIdQuery,
  useInsertCategoryEducationalDegreeMutation,
  useUpdateCategoryEducationalDegreeMutation
} from "@API/services/CategoryEducationalDegree.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryEducationalDegree(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryEducationalDegree, isLoading: LoadingCategoryEducationalDegree } = useGetCategoryEducationalDegreeByIdQuery(
    {idCategoryEducationalDegree: id!}, {skip: !id});
  const [newCategoryEducationalDegree, { isLoading: LoadingInsertCategoryNationaly }] = useInsertCategoryEducationalDegreeMutation();
  const [updateCategoryEducationalDegree, { isLoading: LoadingUpdateCategoryNationaly }] = useUpdateCategoryEducationalDegreeMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (CategoryEducationalDegree?.payload && id) {
      formRef.setFieldsValue(CategoryEducationalDegree?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryEducationalDegree, formRef, id]);
  const onfinish = async (values: CategoryEducationalDegreeDTO) => {
    try {
      const result = id
        ? await updateCategoryEducationalDegree({
          CategoryEducationalDegree: values
        }).unwrap()
        : await newCategoryEducationalDegree({ CategoryEducationalDegree: values }).unwrap();
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
      <Spin spinning={LoadingCategoryEducationalDegree}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"horizontal"} labelCol={{ span: 4 }} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"status"} hidden />
              <Form.Item name={"createdDate"} hidden />
              <Form.Item label="Tên" name={"nameEducationalDegree"}>
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

export const NewAndUpdateCategoryEducationalDegree = WithErrorBoundaryCustom(_NewAndUpdateCategoryEducationalDegree);
