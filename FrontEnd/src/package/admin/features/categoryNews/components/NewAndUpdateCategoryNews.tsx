import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, InputNumber, Row, Select, Space, Spin } from "antd";
import { CustomUploadImage, HandleError, normFile } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import {
  getFileImageCategoryNews,
  useGetCategoryNewsByIdQuery,
  useGetListCategoryNewsQuery,
  useInsertCategoryNewsMutation,
  useUpdateCategoryNewsMutation
} from "@API/services/CategoryNewsApis.service";
import { useEffect } from "react";
import { CategoryNewsDTO } from "@models/categoryNewsDTO";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCategoryNews(props: IProps) {
  const { setVisible, id } = props;
  const { data: CategoryNews, isLoading: LoadingCategoryNews } = useGetCategoryNewsByIdQuery(
    { idCategoryNews: id! },
    { skip: !id }
  );
  const { data: ListCategoryNews, isLoading: LoadingListCategoryNews } = useGetListCategoryNewsQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const [newCategoryNews, { isLoading: LoadingInsertCategoryNews }] = useInsertCategoryNewsMutation();
  const [updateCategoryNews, { isLoading: LoadingUpdateCategoryNews }] = useUpdateCategoryNewsMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    formRef.resetFields();
    if (CategoryNews?.payload && id) {
      formRef.setFieldsValue(CategoryNews?.payload);
      if (CategoryNews?.payload?.avatar) {
        formRef.setFieldsValue({
          Files: [
            {
              uid: "-1",
              name: CategoryNews?.payload.id,
              status: "done",
              url: getFileImageCategoryNews(
                CategoryNews.payload.id + "." + CategoryNews.payload.avatar?.split(".").pop()
              )
            }
          ]
        });
      }
    } else {
      formRef.resetFields();
    }
  }, [CategoryNews?.payload, formRef, id]);
  const onfinish = async (values: CategoryNewsDTO) => {
    try {
      const newDataCategoryNews = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        if (key === "File") return;
        const processedValue = value || "";
        newDataCategoryNews.append(key, processedValue);
      });
      if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
        if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
          // không chỉnh sửa file
          newDataCategoryNews.append("idFile", values.Files?.at(0)?.name as string);
        } else if (values.Files?.length === 0) {
          // xóa file
          newDataCategoryNews.append("idFile", "");
        }
      } else {
        // đã chỉnh sửa file
        newDataCategoryNews.append("File", values.Files?.at(0)?.originFileObj as Blob);
      }
      const result = id
        ? await updateCategoryNews(newDataCategoryNews as any).unwrap()
        : await newCategoryNews(newDataCategoryNews as any).unwrap();
      if (result.success) {
        setVisible(false);
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateCategoryNews">
      <Spin spinning={LoadingCategoryNews}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Tên" name={"nameCategory"}>
                <Input />
              </Form.Item>
              <Form.Item label="Danh mục cha" name={"parentId"}>
                <Select
                  allowClear
                  showSearch
                  loading={LoadingListCategoryNews}
                  optionFilterProp={"label"}
                  options={ListCategoryNews?.listPayload?.map((item) => ({
                    label: item.nameCategory,
                    value: item.id
                  }))}
                />
              </Form.Item>
              <Form.Item label="Nhóm" name={"categoryGroup"}>
                <InputNumber />
              </Form.Item>
              <Form.Item label="Sắp xếp" name={"sort"}>
                <InputNumber />
              </Form.Item>
              <Form.Item label="Thumb" name={"Files"} getValueFromEvent={normFile} valuePropName="fileList">
                <CustomUploadImage beforeUpload={() => false} maxCount={1} aspect={16 / 9} />
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
                    loading={LoadingInsertCategoryNews || LoadingUpdateCategoryNews}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCategoryNews || LoadingUpdateCategoryNews}
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

export const NewAndUpdateCategoryNews = WithErrorBoundaryCustom(_NewAndUpdateCategoryNews);
