import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Form, Input, Row, Space, Spin } from "antd";
import { CustomUploadImage, HandleError, normFile } from "@admin/components";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import {
  getFileImageCompanyInformation,
  useGetCompanyInformationByIdQuery,
  useInsertCompanyInformationMutation,
  useUpdateCompanyInformationMutation
} from "@API/services/CompanyInformationApis.service";
import { useEffect } from "react";
import { CompanyInformationDTO } from "@models/companyInformationDTO";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateCompanyInformation(props: IProps) {
  const { setVisible, id } = props;
  const { data: CompanyInformation, isLoading: LoadingCompanyInformation } = useGetCompanyInformationByIdQuery(
    { idCompanyInformation: id! },
    { skip: !id }
  );
  const [newCompanyInformation, { isLoading: LoadingInsertCompanyInformation }] = useInsertCompanyInformationMutation();
  const [updateCompanyInformation, { isLoading: LoadingUpdateCompanyInformation }] =
    useUpdateCompanyInformationMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    formRef.resetFields();
    if (CompanyInformation?.payload && id) {
      formRef.setFieldsValue(CompanyInformation?.payload);
      if (CompanyInformation?.payload?.logo) {
        formRef.setFieldsValue({
          Files: [
            {
              uid: "-1",
              name: CompanyInformation?.payload.id,
              status: "done",
              url: getFileImageCompanyInformation(CompanyInformation.payload.id + ".jpg")
            }
          ]
        });
      }
    } else {
      formRef.resetFields();
    }
  }, [CompanyInformation?.payload, formRef, id]);
  const onfinish = async (values: CompanyInformationDTO) => {
    try {
      const newDataCompanyInformation = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        if (key === "Files") return;
        const processedValue = value || "";
        newDataCompanyInformation.append(key, processedValue);
      });
      if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
        if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
          // không chỉnh sửa file
          newDataCompanyInformation.append("idFile", values.Files?.at(0)?.name as string);
        } else if (values.Files?.length === 0) {
          // xóa file
          newDataCompanyInformation.append("idFile", "");
        }
      } else {
        // đã chỉnh sửa file
        newDataCompanyInformation.append("Files", values.Files?.at(0)?.originFileObj as Blob);
      }
      const result = id
        ? await updateCompanyInformation(newDataCompanyInformation as any).unwrap()
        : await newCompanyInformation(newDataCompanyInformation as any).unwrap();
      if (result.success) {
        setVisible(false);
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateCompanyInformation">
      <Spin spinning={LoadingCompanyInformation}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form
              layout={"horizontal"}
              form={formRef}
              onFinish={onfinish}
              labelCol={{
                xs: { span: 2, offset: 1 },
                sm: { span: 2, offset: 1 },
                md: { span: 2, offset: 1 },
                lg: { span: 2, offset: 1 },
                xl: { span: 2, offset: 1 }
              }}
            >
              <Form.Item name={"id"} hidden />
              <Form.Item label="Tên" name={"companyName"}>
                <Input />
              </Form.Item>
              {/*<Form.Item name={"taxNumber"} label={"Mã số thuế"}>*/}
              {/*  <Input />*/}
              {/*</Form.Item>*/}
              <Form.Item label="Số tài khoản" name={"accountNumber"}>
                <Input />
              </Form.Item>
              <Form.Item label="Địa chỉ" name={"address"}>
                <Input />
              </Form.Item>
              <Form.Item label="Số điện thoại" name={"phoneNumber"}>
                <Input />
              </Form.Item>
              <Form.Item label="Email" name={"email"}>
                <Input />
              </Form.Item>
              <Form.Item label="Giờ làm việc" name={"openingHours"}>
                <Input />
              </Form.Item>
              <Form.Item label="copyright" name={"copyright"}>
                <Input />
              </Form.Item>
              <Form.Item label="Logo" name={"Files"} getValueFromEvent={normFile} valuePropName="fileList">
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
                    loading={LoadingInsertCompanyInformation || LoadingUpdateCompanyInformation}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertCompanyInformation || LoadingUpdateCompanyInformation}
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

export const NewAndUpdateCompanyInformation = WithErrorBoundaryCustom(_NewAndUpdateCompanyInformation);
