import {
  useGetCategoryVacanciesByIdQuery,
  useInsertCategoryVacanciesMutation,
  useUpdateCategoryVacanciesMutation
} from "@API/services/CategoryVacanciesApis.service";
import { Button, Card, Col, Form, Input, Row, Space, Spin } from "antd";
import { useEffect, useRef } from "react";
import { CategoryVacanciesDTO } from "@models/categoryVacanciesDTO";
import { HandleError, MyEditor } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryVacancies(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryVacancies, isLoading: LoadingCategoryVacancies } = useGetCategoryVacanciesByIdQuery(
    { idCategoryVacancies: id! },
    {
      skip: !id
    }
  );
  const [newCategoryVacancies, { isLoading: LoadingInsertCategoryVacancies }] = useInsertCategoryVacanciesMutation();
  const [updateCategoryVacancies, { isLoading: LoadingUpdateCategoryVacancies }] = useUpdateCategoryVacanciesMutation();
  const [formRef] = Form.useForm();

  const htmlContentJobRequirements = useRef("");
  const htmlContentJobDescription = useRef("");
  const getHtmlContentJobRequirements = (value: any) => {
    htmlContentJobRequirements.current = value;
  };
  const getHtmlContentJobDescription = (value: any) => {
    htmlContentJobDescription.current = value;
  };

  useEffect(() => {
    formRef.resetFields();
    if (CategoryVacancies?.payload && id) {
      formRef.setFieldsValue(CategoryVacancies?.payload);
    } else {
      formRef.resetFields();
    }
  }, [CategoryVacancies, formRef, id]);
  const onfinish = async (values: CategoryVacanciesDTO) => {
    try {
      values.jobRequirements = htmlContentJobRequirements.current;
      values.jobDescription = htmlContentJobDescription.current;
      const result = id
        ? await updateCategoryVacancies({
            categoryVacancies: values
          }).unwrap()
        : await newCategoryVacancies({ categoryVacancies: values }).unwrap();
      if (result.success) {
        setVisible(false);
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateCategoryVacancies">
      <Spin spinning={LoadingCategoryVacancies}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Tên" name={"positionName"}>
                <Input />
              </Form.Item>
              <Form.Item label="Năm kinh nghiệm" name={"numExp"}>
                <Input type={"number"} />
              </Form.Item>
              <Form.Item label="Bằng cấp" name={"degree"}>
                <Input.TextArea />
              </Form.Item>

              <Form.Item label="Yêu cầu công việc" name={"jobRequirements"}>
                <Card>
                  <MyEditor
                    setHtmlContent={getHtmlContentJobRequirements}
                    language={"en"}
                    initHtmlContent={CategoryVacancies?.payload.jobRequirements || ""}
                  />
                </Card>
              </Form.Item>
              <Form.Item label="Mô tả" name={"jobDescription"}>
                <Card>
                  <MyEditor
                    setHtmlContent={getHtmlContentJobDescription}
                    language={"en"}
                    initHtmlContent={CategoryVacancies?.payload.jobDescription || ""}
                  />
                </Card>
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
                    loading={LoadingInsertCategoryVacancies || LoadingUpdateCategoryVacancies}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryVacancies || LoadingUpdateCategoryVacancies}
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

export const NewAndUpdateCategoryVacancies = WithErrorBoundaryCustom(_NewAndUpdateCategoryVacancies);
