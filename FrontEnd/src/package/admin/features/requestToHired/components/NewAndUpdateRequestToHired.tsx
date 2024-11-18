import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  getFileRequestToHired,
  useGetRequestToHiredByIdQuery,
  useInsertRequestToHiredMutation,
  useUpdateRequestToHiredMutation
} from "@API/services/RequestToHiredApis.service";
import { Button, Col, Form, Input, Row, Select, Space, Spin } from "antd";
import { CustomUploadFileDrag, HandleError, normFile } from "@admin/components";
import { RequestToHiredDTO } from "@models/requestToHiredDTO";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect } from "react";
import { useGetListCategoryVacanciesApprovedQuery } from "@API/services/CategoryVacanciesApis.service";
import FormData from "form-data";

interface IProps {
  setVisible: (value: boolean) => any;
  id?: string;
}

function _NewAndUpdateRequestToHired(props: IProps) {
  const { setVisible, id } = props;
  const { data: RequestToHired, isLoading: LoadingRequestToHired } = useGetRequestToHiredByIdQuery(
    { idRequestToHired: id! },
    {
      skip: !id
    }
  );
  const { data: ListCategoryVacanCies, isLoading: isLoadingCategoryVacancies } =
    useGetListCategoryVacanciesApprovedQuery({
      pageNumber: 0,
      pageSize: 0
    });
  const [newRequestToHired, { isLoading: LoadingInsertRequestToHired }] = useInsertRequestToHiredMutation();
  const [updateRequestToHired, { isLoading: LoadingUpdateRequestToHired }] = useUpdateRequestToHiredMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    formRef.resetFields();
    if (RequestToHired?.payload && id) {
      formRef.setFieldsValue({
        ...RequestToHired?.payload
      });
      if (RequestToHired?.payload?.filePath) {
        formRef.setFieldsValue({
          Files: [
            {
              uid: "-1",
              name: RequestToHired?.payload.id,
              status: "done",
              url: getFileRequestToHired(
                RequestToHired.payload.id + "." + RequestToHired?.payload?.filePath?.split(".")?.at(1)
              )
            }
          ]
        });
      }
    } else {
      formRef.resetFields();
    }
  }, [RequestToHired, formRef, id]);
  const onfinish = async (values: RequestToHiredDTO) => {
    try {
      const data = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        if (key === "Files") return;
        const processedValue = value || "";
        data.append(key, processedValue);
      });
      if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
        if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
          // không chỉnh sửa file
          data.append("idFile", values.Files?.at(0)?.name as string);
        } else if (values.Files?.length === 0) {
          // xóa file
          data.append("idFile", "");
        }
      } else {
        // đã chỉnh sửa file
        data.append("Files", values.Files?.at(0)?.originFileObj as Blob);
      }
      const result = id
        ? await updateRequestToHired({
            requestToHired: data as any
          }).unwrap()
        : await newRequestToHired({ requestToHired: data as any }).unwrap();
      if (result.success) {
        setVisible(false);
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateRequestToHired">
      <Spin spinning={LoadingRequestToHired}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item name={"idUnit"} hidden />
              <Form.Item label="Lý do" name={"reason"}>
                <Input.TextArea />
              </Form.Item>
              <Form.Item label="Số lượng" name={"quantity"}>
                <Input type={"number"} min={0} />
              </Form.Item>
              <Form.Item label="Vị trí tuyển dụng" name={"idCategoryVacancies"}>
                <Select
                  showSearch
                  loading={isLoadingCategoryVacancies}
                  optionFilterProp={"label"}
                  options={ListCategoryVacanCies?.listPayload?.map((x) => {
                    return {
                      label: x.positionName,
                      value: x.id
                    };
                  })}
                />
              </Form.Item>
              <Form.Item label="File" name={"Files"} getValueFromEvent={normFile} valuePropName="fileList">
                <CustomUploadFileDrag multiple={false} maxCount={1} />
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
                    loading={LoadingInsertRequestToHired || LoadingUpdateRequestToHired}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertRequestToHired || LoadingUpdateRequestToHired}
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

export const NewAndUpdateRequestToHired = WithErrorBoundaryCustom(_NewAndUpdateRequestToHired);
