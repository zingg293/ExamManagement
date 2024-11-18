import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Card, Col, DatePicker, Form, Input, Row, Select, Space, Spin } from "antd";
import { CustomUploadImage, HandleError, MyEditor, normFile } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import {
  getFileImageNews,
  useGetNewsByIdQuery,
  useInsertNewsMutation,
  useUpdateNewsMutation
} from "@API/services/NewsApis.service";
import { useEffect, useRef, useState } from "react";
import { useGetListCategoryNewsAvailableQuery } from "@API/services/CategoryNewsApis.service";
import dayjs from "dayjs";
import { NewsDTO } from "@models/newsDTO";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateNews(props: IProps) {
  const { setVisible, id } = props;
  const { data: News, isLoading: LoadingNews } = useGetNewsByIdQuery({ idNews: id! }, { skip: !id });
  const { data: ListCategoryNews, isLoading: LoadingListCategoryNews } = useGetListCategoryNewsAvailableQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const [newNews, { isLoading: LoadingInsertNews }] = useInsertNewsMutation();
  const [updateNews, { isLoading: LoadingUpdateNews }] = useUpdateNewsMutation();
  const [formRef] = Form.useForm();
  const [initHtmlContent, setInitHtmlContent] = useState("");
  const convertedContentRef = useRef("");
  const GetHtmlContent = (html: string) => {
    convertedContentRef.current = html;
  };
  useEffect(() => {
    formRef.resetFields();
    if (News?.payload && id) {
      formRef.setFieldsValue({
        ...News?.payload,
        createdDateDisplay: dayjs(News?.payload.createdDateDisplay)
      });
      if (News?.payload?.avatar) {
        formRef.setFieldsValue({
          Files: [
            {
              uid: "-1",
              name: News?.payload.id,
              status: "done",
              url: getFileImageNews(News.payload.id + "." + News.payload.extensionFile)
            }
          ]
        });
      }
      setInitHtmlContent(News?.payload.newsContent || "");
    } else {
      formRef.resetFields();
    }
  }, [News?.payload, formRef, id]);
  const onfinish = async (values: NewsDTO) => {
    const newDataNews = new FormData();
    Object.entries(values).forEach(([key, value]) => {
      if (key === "Files") return;
      if (key === "createdDateDisplay") newDataNews.append(key, dayjs(value)?.format("YYYY-MM-DD HH:mm:ss"));
      if (key === "newsContent") newDataNews.append(key, convertedContentRef.current);
      const processedValue = value || "";
      newDataNews.append(key, processedValue);
    });
    if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
      if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
        // không chỉnh sửa file
        newDataNews.append("idFile", values.Files?.at(0)?.name as string);
      } else if (values.Files?.length === 0) {
        // xóa file
        newDataNews.append("idFile", "");
      }
    } else {
      // đã chỉnh sửa file
      newDataNews.append("Files", values.Files?.at(0)?.originFileObj as Blob);
    }
    try {
      const result = id ? await updateNews(newDataNews as any).unwrap() : await newNews(newDataNews as any).unwrap();
      if (result.success) {
        setVisible(false);
        // formRef.resetFields();
        // setInitHtmlContent("");
        // convertedContentRef.current = "";
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateNews">
      <Spin spinning={LoadingNews}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Tiêu đề" name={"title"}>
                <Input.TextArea />
              </Form.Item>
              <Form.Item label="Mô tả" name={"description"}>
                <Input.TextArea />
              </Form.Item>
              <Form.Item
                label="Danh mục"
                name={"idCategoryNews"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn danh mục"
                  }
                ]}
              >
                <Select
                  showSearch
                  loading={LoadingListCategoryNews}
                  allowClear
                  optionFilterProp={"label"}
                  options={ListCategoryNews?.listPayload?.map((item) => ({
                    label: item.nameCategory,
                    value: item.id
                  }))}
                />
              </Form.Item>
              <Form.Item label="Ngày hiển thị" name={"createdDateDisplay"}>
                <DatePicker showTime format={"DD/MM/YYYY HH:mm"} />
              </Form.Item>
              <Form.Item label="Thumb" name={"Files"} getValueFromEvent={normFile} valuePropName="fileList">
                <CustomUploadImage beforeUpload={() => false} maxCount={1} aspect={16 / 9} />
              </Form.Item>

              <Form.Item label="Nôi dung" name={"newsContent"}>
                <Card>
                  <MyEditor
                    language="en"
                    setHtmlContent={GetHtmlContent}
                    TypeUseMyUploadFn={1}
                    initHtmlContent={initHtmlContent}
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
                    loading={LoadingInsertNews || LoadingUpdateNews}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertNews || LoadingUpdateNews}
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

export const NewAndUpdateNews = WithErrorBoundaryCustom(_NewAndUpdateNews);
