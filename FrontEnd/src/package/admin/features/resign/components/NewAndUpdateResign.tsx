import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  getFileResign,
  useGetResignByIdQuery,
  useInsertResignMutation,
  useUpdateResignMutation
} from "@API/services/ResignApis.service";
import { useGetListUnitQuery } from "@API/services/UnitApis.service";
import { Button, Col, Form, Input, Row, Select, Space, Spin } from "antd";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { CustomUploadFileDrag, HandleError, normFile } from "@admin/components";
import { useEffect } from "react";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";
import FormData from "form-data";

interface Props {
  id?: string;
  AfterSave?: () => void;
}

function _NewAndUpdateResign(props: Props) {
  const { id, AfterSave } = props;
  const { data: Resign, isLoading: LoadingResign } = useGetResignByIdQuery({ idResign: id! }, { skip: !id });
  const { data: ListUnit, isLoading: LoadingListUnit } = useGetListUnitQuery({ pageNumber: 0, pageSize: 0 });
  const { data: ListEmployee, isLoading: LoadingListEmployee } = useGetListEmployeeQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const [newResign, { isLoading: LoadingInsertResign }] = useInsertResignMutation();
  const [updateResign, { isLoading: LoadingUpdateResign }] = useUpdateResignMutation();
  const [formRef] = Form.useForm();
  useEffect(() => {
    formRef.resetFields();
    if (Resign?.payload && id) {
      formRef.setFieldsValue(Resign?.payload);
      if (Resign?.payload?.resignForm) {
        formRef.setFieldsValue({
          Files: [
            {
              uid: "-1",
              name: Resign?.payload.id,
              status: "done",
              url: getFileResign(Resign.payload.id + "." + Resign?.payload?.resignForm?.split(".")?.at(1))
            }
          ]
        });
      }
    } else {
      formRef.resetFields();
    }
  }, [Resign?.payload, formRef, id]);

  const onfinish = async (values: any) => {
    try {
      const dataResign = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        if (key === "Files") return;
        if (key === "type") dataResign.append(key, value);
        const processedValue = value || "";
        dataResign.append(key, processedValue);
      });
      if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
        if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
          // không chỉnh sửa file
          dataResign.append("idFile", values.Files?.at(0)?.name as string);
        } else if (values.Files?.length === 0) {
          // xóa file
          dataResign.append("idFile", "");
        }
      } else {
        // đã chỉnh sửa file
        dataResign.append("Files", values.Files?.at(0)?.originFileObj as Blob);
      }
      const result = id
        ? await updateResign({
            resign: dataResign as any
          }).unwrap()
        : await newResign({
            resign: dataResign as any
          }).unwrap();
      if (result.success) {
        AfterSave && AfterSave();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="NewAndUpdateResign">
      <Spin spinning={LoadingResign}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Mô tả" name={"description"}>
                <Input.TextArea />
              </Form.Item>
              <Form.Item label="Tên phòng ban" name={"unitName"}>
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
                  loading={LoadingListUnit}
                  showSearch
                  options={ListUnit?.listPayload?.map((item) => ({
                    label: item.unitName,
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
                    loading={LoadingInsertResign || LoadingUpdateResign}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertResign || LoadingUpdateResign}
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

export const NewAndUpdateResign = WithErrorBoundaryCustom(_NewAndUpdateResign);
