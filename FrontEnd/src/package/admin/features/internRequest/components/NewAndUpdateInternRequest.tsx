import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  getFileInternRequest,
  useGetInternRequestByIdQuery,
  useInsertInternRequestMutation,
  useUpdateInternRequestMutation
} from "@API/services/InternRequestApis.service";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { useGetListCategoryPositionAvailableQuery } from "@API/services/CategoryPositionApis.service";
import { Button, Col, Form, Input, Row, Select, Space, Spin } from "antd";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CustomUploadFileDrag, HandleError, normFile } from "@admin/components";
import { useEffect } from "react";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";

interface Props {
  id?: string;
  AfterSave?: () => void;
}

function _NewAndUpdateInternRequest(props: Props) {
  const { id, AfterSave } = props;
  const { data: InternRequest, isLoading: LoadingInternRequest } = useGetInternRequestByIdQuery(
    { idInternRequest: id! },
    { skip: !id }
  );
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitQuery({ pageNumber: 0, pageSize: 0 });
  const { data: ListCategoryPosition, isLoading: LoadingListCategoryPosition } =
    useGetListCategoryPositionAvailableQuery({
      pageNumber: 0,
      pageSize: 0
    });
  const { data: ListEmployee, isLoading: LoadingListEmployee } = useGetListEmployeeQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const [newInternRequest, { isLoading: LoadingInsertInternRequest }] = useInsertInternRequestMutation();
  const [updateInternRequest, { isLoading: LoadingUpdateInternRequest }] = useUpdateInternRequestMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    formRef.resetFields();
    if (InternRequest?.payload && id) {
      formRef.setFieldsValue(InternRequest?.payload);
      if (InternRequest?.payload?.attachments) {
        formRef.setFieldsValue({
          Files: [
            {
              idFIle: InternRequest?.payload.id,
              uid: "-1",
              name: InternRequest?.payload.attachments,
              status: "done",
              url: getFileInternRequest(
                InternRequest.payload.id + "." + InternRequest?.payload?.attachments?.split(".")?.at(1)
              )
            }
          ]
        });
      }
    } else {
      formRef.resetFields();
    }
  }, [InternRequest?.payload, formRef, id]);

  const onfinish = async (values: any) => {
    try {
      const newDataInternRequest = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        if (key === "Files") return;
        const processedValue = value || ("" as any);
        newDataInternRequest.append(key, processedValue);
      });
      if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
        if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
          // không chỉnh sửa file
          newDataInternRequest.append("idFile", values.Files?.at(0)?.idFIle as string);
        } else if (values.Files?.length === 0) {
          // xóa file
          newDataInternRequest.append("idFile", "");
        }
      } else {
        // đã chỉnh sửa file
        newDataInternRequest.append("Files", values.Files?.at(0)?.originFileObj as Blob);
      }

      const result = id
        ? await updateInternRequest({
            internRequest: newDataInternRequest as any
          }).unwrap()
        : await newInternRequest({
            internRequest: newDataInternRequest as any
          }).unwrap();
      if (result.success) {
        AfterSave && AfterSave();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="NewAndUpdateInternRequest">
      <Spin spinning={LoadingInternRequest}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Mô tả" name={"description"}>
                <Input.TextArea />
              </Form.Item>
              <Form.Item label="Tên phòng ban" name={"unitName"} hidden>
                <Input />
              </Form.Item>
              <Form.Item
                label="Phòng ban"
                name={"idUnit"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn phòng ban"
                  }
                ]}
              >
                <Select
                  onChange={(_, option: any) => {
                    formRef.setFieldsValue({
                      unitName: option?.label
                    });
                  }}
                  loading={LoadingListUnit}
                  showSearch
                  options={ListUnit?.listPayload?.map((item) => ({
                    label: item.unitName,
                    value: item.id
                  }))}
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item label="Tên vị trí" name={"positionName"} hidden>
                <Input />
              </Form.Item>
              <Form.Item
                label="Vị trí"
                name={"idPosition"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn vị trí"
                  }
                ]}
              >
                <Select
                  onChange={(_, option: any) => {
                    formRef.setFieldsValue({
                      positionName: option?.label
                    });
                  }}
                  loading={LoadingListCategoryPosition}
                  showSearch
                  options={ListCategoryPosition?.listPayload?.map((item) => ({
                    label: item.positionName,
                    value: item.id
                  }))}
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item
                label="Nhân viên"
                name={"idEmployee"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn nhân viên"
                  }
                ]}
              >
                <Select
                  loading={LoadingListEmployee}
                  showSearch
                  options={ListEmployee?.listPayload?.map((item) => ({
                    label: item.name + " - " + item.code,
                    value: item.id
                  }))}
                  optionFilterProp={"label"}
                />
              </Form.Item>
              <Form.Item label="File đính kèm" name={"Files"} getValueFromEvent={normFile} valuePropName="fileList">
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
                    loading={LoadingInsertInternRequest || LoadingUpdateInternRequest}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertInternRequest || LoadingUpdateInternRequest}
                    icon={<CheckCircleOutlined />}
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

export const NewAndUpdateInternRequest = WithErrorBoundaryCustom(_NewAndUpdateInternRequest);
