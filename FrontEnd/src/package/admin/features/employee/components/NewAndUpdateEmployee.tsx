import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  GetFIleEmployee,
  useGetEmployeeByIdQuery,
  useInsertEmployeeMutation,
  useUpdateEmployeeMutation
} from "@API/services/Employee.service";
import {
  Button,
  Col,
  DatePicker,
  Divider,
  Form,
  Input,
  InputNumber,
  Radio,
  Row,
  Select,
  Space,
  Spin,
  Typography
} from "antd";
import { CustomUploadImage, HandleError, normFile } from "@admin/components";
import { EmployeeDTO } from "@models/employeeDTO";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect } from "react";

import {
  useGetCategoryDistrictByIdQuery,
  useLazyGetListCategoryDistrictByIdCityQuery
} from "@API/services/CategorryDistrict.service";
import {
  useGetCategoryWardByIdQuery,
  useLazyGetListCategoryWardByIdDistrictQuery
} from "@API/services/CategoryWardApis.service";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";
import { useGetListEmployeeTypeAvailableQuery } from "@API/services/EmployeeType.service";
import dayjs from "dayjs";
import { useGetListCategoryCityAvailableQuery } from "@API/services/CategoryCity.service";
import { useGetListUserQuery } from "@API/services/UserApis.service";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateEmployee(props: IProps) {
  const { setVisible, id } = props;
  const { data: ListUser, isLoading: isLoadingListUser } = useGetListUserQuery({
    pageSize: 0,
    pageNumber: 0
  });

  const { data: Employee, isLoading: LoadingEmployee } = useGetEmployeeByIdQuery(
    { idEmployee: id! },
    {
      skip: !id
    }
  );
  const { data: ListCategoryCity, isLoading: isLoadingCategoryCIty } = useGetListCategoryCityAvailableQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: categoryDistrict } = useGetCategoryDistrictByIdQuery(Employee?.payload?.idDistrict || "", {
    skip: !Employee?.payload?.idDistrict
  });
  const { data: categoryWard } = useGetCategoryWardByIdQuery(
    { idCategoryWard: Employee?.payload?.idWard || "" },
    { skip: !Employee?.payload?.idWard }
  );
  const { data: ListUserType, isLoading: isLoadingUserType } = useGetListEmployeeTypeAvailableQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: ListUnit, isLoading: isLoadingUnit } = useGetListUnitAvailableQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const [triggerGetListCategoryDistrict, { data: apisDistrict, isLoading: loadingDistrict }] =
    useLazyGetListCategoryDistrictByIdCityQuery();
  const [triggerGetListCategoryWard, { data: apisWard, isLoading: loadingWard }] =
    useLazyGetListCategoryWardByIdDistrictQuery();

  const [newEmployee, { isLoading: LoadingInsertEmployee }] = useInsertEmployeeMutation();
  const [updateEmployee, { isLoading: LoadingUpdateEmployee }] = useUpdateEmployeeMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    formRef.resetFields();
    if (Employee?.payload && id) {
      formRef.setFieldsValue({
        ...Employee?.payload,
        birthday: dayjs(Employee?.payload.birthday)
      });
      if (Employee?.payload?.avatar) {
        formRef.setFieldsValue({
          Files: [
            {
              uid: "-1",
              name: Employee?.payload.id,
              status: "done",
              url: GetFIleEmployee(Employee.payload.id + ".jpg")
            }
          ]
        });
      }
    } else {
      formRef.resetFields();
    }
  }, [Employee, formRef, id]);
  useEffect(() => {
    if (categoryDistrict?.payload) {
      triggerGetListCategoryDistrict({
        CityCode: categoryDistrict?.payload?.cityCode || "",
        pageNumber: 0,
        pageSize: 0
      });
    }
    if (categoryWard?.payload) {
      triggerGetListCategoryWard({
        DistrictCode: categoryWard?.payload.districtCode || "",
        pageNumber: 0,
        pageSize: 0
      });
    }
  }, [categoryDistrict?.payload, categoryWard?.payload, triggerGetListCategoryDistrict, triggerGetListCategoryWard]);
  const onfinish = async (values: EmployeeDTO) => {
    try {
      const newData = new FormData();
      Object.entries(values).forEach(([key, value]) => {
        if (key === "Files") return;
        const processedValue = value || "";
        newData.append(key, processedValue);
      });
      if (!(values.Files?.length > 0 && values.Files?.at(0)?.uid !== "-1")) {
        if (values.Files?.length > 0 && values.Files?.at(0)?.uid === "-1") {
          // không chỉnh sửa file
          newData.append("idFile", values.Files?.at(0)?.name as string);
        } else if (values.Files?.length === 0) {
          // xóa file
          newData.append("idFile", "");
        }
      } else {
        // đã chỉnh sửa file
        newData.append("Files", values.Files?.at(0)?.originFileObj as Blob);
      }

      const result = id
        ? await updateEmployee({
            employee: newData as any
          }).unwrap()
        : await newEmployee({ employee: newData as any }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateEmployee">
      <Spin spinning={LoadingEmployee}>
        <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
          <Row>
            <Form.Item name={"id"} hidden />
            {/*information*/}
            <Col xs={24} sm={24} md={24} lg={24} xl={24}>
              <Divider>
                <Typography.Title level={5}>Thông tin nhân viên</Typography.Title>
              </Divider>
              <Row gutter={[24, 0]}>
                <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                  <Form.Item
                    name="idUser"
                    label="User"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng chọn user"
                      }
                    ]}
                  >
                    <Select
                      showSearch
                      onChange={(value) => {
                        const user = ListUser?.listPayload?.find((item) => item.id === value);
                        const fillData = {
                          name: user?.fullname,
                          phone: user?.phone,
                          email: user?.email,
                          address: user?.address,
                          idUnit: user?.unitId
                        };
                        formRef.setFieldsValue(fillData);
                      }}
                      optionFilterProp={"label"}
                      options={ListUser?.listPayload?.map((user) => {
                        return {
                          value: user.id,
                          label: `${user.fullname} - ${user.email}`
                        };
                      })}
                      loading={isLoadingListUser}
                    />
                  </Form.Item>

                  <Form.Item
                    name="phone"
                    label="Số điện thoại"
                    rules={[
                      //regex phone
                      {
                        pattern: /^0[0-9]{9}$/,
                        message: "Số điện thoại không hợp lệ"
                      },
                      {
                        required: true,
                        message: "Vui lòng nhập số điện thoại"
                      }
                    ]}
                  >
                    <Input />
                  </Form.Item>
                  <Form.Item
                    name="email"
                    label="Email"
                    rules={[
                      //regex email
                      {
                        pattern: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/,
                        message: "Email không hợp lệ"
                      }
                    ]}
                  >
                    <Input />
                  </Form.Item>
                </Col>
                <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                  <Form.Item
                    name="name"
                    label="Họ và tên"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng nhập họ và tên"
                      }
                    ]}
                  >
                    <Input allowClear />
                  </Form.Item>
                  <Form.Item
                    name="sex"
                    label="Giới tính"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng chọn giới tính"
                      }
                    ]}
                  >
                    <Radio.Group optionType="button">
                      <Radio value={true}>Nam</Radio>
                      <Radio value={false}>Nữ</Radio>
                    </Radio.Group>
                  </Form.Item>
                  <Form.Item
                    name="birthday"
                    label="Ngày sinh"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng chọn ngày sinh"
                      }
                    ]}
                  >
                    <DatePicker format="DD/MM/YYYY" placeholder="00/00/0000" />
                  </Form.Item>
                </Col>
              </Row>
            </Col>
            {/*address*/}
            <Col xs={24} sm={24} md={24} lg={24} xl={24}>
              <Divider>
                <Typography.Title level={5}>Địa chỉ</Typography.Title>
              </Divider>
              <Row gutter={[24, 0]}>
                <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                  <Form.Item name="idCity" label="Tỉnh">
                    <Select
                      showSearch
                      loading={isLoadingCategoryCIty}
                      placeholder={"Chọn tỉnh"}
                      optionFilterProp="label"
                      onChange={async (e, option: any) => {
                        try {
                          await triggerGetListCategoryDistrict({ CityCode: option.code, pageSize: 0, pageNumber: 0 });
                          formRef.setFieldsValue({ idDistrict: undefined, idWard: undefined });
                        } catch (e) {
                          console.log(e);
                        }
                      }}
                      options={ListCategoryCity?.listPayload?.map((item) => ({
                        value: item.id,
                        label: item.cityName,
                        code: item.cityCode
                      }))}
                    />
                  </Form.Item>
                  <Form.Item name="idDistrict" label="Huyện, thành phố">
                    <Select
                      showSearch
                      loading={loadingDistrict}
                      placeholder={"Chọn huyện, thành phố"}
                      optionFilterProp="label"
                      onChange={async (e, option: any) => {
                        try {
                          await triggerGetListCategoryWard({ DistrictCode: option.code, pageSize: 0, pageNumber: 0 });
                          formRef.setFieldsValue({ idWard: undefined });
                        } catch (e) {
                          console.log(e);
                        }
                      }}
                      options={apisDistrict?.listPayload?.map((item) => ({
                        value: item.id,
                        label: item.districtName,
                        code: item.districtCode
                      }))}
                    />
                  </Form.Item>
                </Col>
                <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                  <Form.Item name="idWard" label="Xã, phường">
                    <Select
                      showSearch
                      loading={loadingWard}
                      optionFilterProp="label"
                      options={apisWard?.listPayload?.map((item) => ({
                        value: item.id,
                        label: item.wardName,
                        code: item.wardCode
                      }))}
                    />
                  </Form.Item>
                  <Form.Item name="address" label="Địa chỉ">
                    <Input.TextArea allowClear placeholder="401/6, KP8" />
                  </Form.Item>
                </Col>
              </Row>
            </Col>
            {/*profile*/}
            <Col xs={24} sm={24} md={24} lg={24} xl={24}>
              <Divider>
                <Typography.Title level={5}>Hồ sơ nhân viên</Typography.Title>
              </Divider>
              <Row gutter={[24, 0]}>
                <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                  {/*<Form.Item*/}
                  {/*  name="taxNumber"*/}
                  {/*  label="Mã số thuế"*/}
                  {/*  rules={[*/}
                  {/*    {*/}
                  {/*      pattern: /^[0-9]{10}$/,*/}
                  {/*      message: "Mã số thuế không hợp lệ"*/}
                  {/*    }*/}
                  {/*  ]}*/}
                  {/*>*/}
                  {/*  <Input allowClear />*/}
                  {/*</Form.Item>*/}
                  <Form.Item name="accountNumber" label="Số tài khoản">
                    <Input />
                  </Form.Item>
                  <Form.Item name="note" label="Ghi chú">
                    <Input.TextArea allowClear />
                  </Form.Item>
                </Col>
                <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                  <Form.Item
                    name="typeOfEmployee"
                    label="Loại nhân viên"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng chọn loại nhân viên"
                      }
                    ]}
                  >
                    <Select
                      loading={isLoadingUserType}
                      showSearch
                      optionFilterProp={"label"}
                      options={ListUserType?.listPayload?.map((item) => ({
                        value: item.id,
                        label: `${item.typeName}`
                      }))}
                    />
                  </Form.Item>
                  <Form.Item
                    name="idUnit"
                    label="Phòng ban"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng chọn phòng ban"
                      }
                    ]}
                  >
                    <Select
                      loading={isLoadingUnit}
                      showSearch
                      optionFilterProp={"label"}
                      options={ListUnit?.listPayload?.map((item) => ({
                        value: item.id,
                        label: `${item.unitName}-${item.unitCode} `
                      }))}
                    />
                  </Form.Item>
                  <Form.Item label="Hình ảnh" getValueFromEvent={normFile} valuePropName="fileList" name={"Files"}>
                    <CustomUploadImage multiple={false} maxCount={1} aspect={16 / 9} beforeUpload={() => false} />
                  </Form.Item>
                </Col>
              </Row>
            </Col>
            {/*benifit*/}
            <Col xs={24} sm={24} md={24} lg={24} xl={24}>
              <Divider>
                <Typography.Title level={5}>Phúc lợi nhân viên</Typography.Title>
              </Divider>
              <Row gutter={[24, 0]}>
                <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                  <Form.Item
                    name={"salaryBase"}
                    label="Lương cơ bản"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng nhập lương cơ bản"
                      },
                      {
                        validator(rule, value, callback) {
                          if (typeof value !== "number") {
                            callback("Vui lòng nhập số");
                          } else {
                            callback();
                          }
                        }
                      }
                    ]}
                  >
                    <InputNumber
                      formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")}
                      parser={(value) => value!.replace(/\$\s?|(,*)/g, "")}
                      style={{
                        width: "100%"
                      }}
                    />
                  </Form.Item>
                  <Form.Item
                    name="socialInsurancePercent"
                    label="(%) bảo hiểm xã hội"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng nhập % bảo hiểm xã hội"
                      }
                    ]}
                  >
                    <Input type={"number"} />
                  </Form.Item>
                </Col>{" "}
                <Col xs={24} sm={24} md={24} lg={12} xl={12} className="mb-24">
                  <Form.Item
                    name="taxPercent"
                    label="(%) thuế"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng nhập % thuế. Nếu không có thuế thì nhập 0"
                      }
                    ]}
                  >
                    <Input type={"number"} />
                  </Form.Item>
                  <Form.Item
                    name="jobGrade"
                    label="Thâm niên"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng nhập thâm niên"
                      }
                    ]}
                  >
                    <Input type={"number"} />
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
                        loading={LoadingInsertEmployee || LoadingUpdateEmployee}
                        icon={<RetweetOutlined />}
                      >
                        Xóa
                      </Button>
                      <Button
                        type="primary"
                        htmlType="submit"
                        loading={LoadingInsertEmployee || LoadingUpdateEmployee}
                        icon={<CheckCircleOutlined />}
                        style={{
                          float: "right"
                        }}
                      >
                        Lưu
                      </Button>
                    </Space>
                  </Form.Item>
                </Col>
              </Row>
            </Col>
          </Row>
        </Form>
      </Spin>
    </div>
  );
}

export const NewAndUpdateEmployee = WithErrorBoundaryCustom(_NewAndUpdateEmployee);
