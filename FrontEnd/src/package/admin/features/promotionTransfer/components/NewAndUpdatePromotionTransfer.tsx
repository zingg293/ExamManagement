import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useGetPromotionTransferByIdQuery,
  useInsertPromotionTransferMutation,
  useUpdatePromotionTransferMutation
} from "@API/services/PromotionTransferApis.service";
import { Button, Checkbox, Col, Form, Input, Radio, Row, Select, Space, Spin, Typography } from "antd";
import { HandleError } from "@admin/components";
import { PromotionTransferDTO } from "@models/promotionTransferDTO";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect, useState } from "react";
import { useGetListCategoryPositionQuery } from "@API/services/CategoryPositionApis.service";
import { useGetListEmployeeQuery } from "@API/services/Employee.service";
import { useLazyGetListPositionEmployeeByIdEmployeeQuery } from "@API/services/PositionEmployeeApis.service";
import { useGetListUnitAvailableQuery } from "@API/services/UnitApis.service";

interface IProps {
  setVisible: (value: boolean) => any;
  id?: string;
}

function _NewAndUpdatePromotionTransfer(props: IProps) {
  const { setVisible, id } = props;
  const { data: PromotionTransfer, isLoading: LoadingPromotionTransfer } = useGetPromotionTransferByIdQuery(
    { idPromotionTransfer: id! },
    {
      skip: !id
    }
  );
  const { data: ListCategoryPosition, isLoading: isLoadingCategoryPosition } = useGetListCategoryPositionQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const { data: ListEmployee, isLoading: isLoadingListEmployee } = useGetListEmployeeQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const { data: ListUnit, isLoading: isLoadingListUnit } = useGetListUnitAvailableQuery({
    pageNumber: 0,
    pageSize: 0
  });
  const [
    GetListPositionEmployeeByIdEmployee,
    { isLoading: isLoadingGetListPositionEmployeeByIdEmployee, data: ListPositionEmployeeByIdEmployee }
  ] = useLazyGetListPositionEmployeeByIdEmployeeQuery();
  const [newPromotionTransfer, { isLoading: LoadingInsertPromotionTransfer }] = useInsertPromotionTransferMutation();
  const [updatePromotionTransfer, { isLoading: LoadingUpdatePromotionTransfer }] = useUpdatePromotionTransferMutation();
  const [formRef] = Form.useForm();
  const [isPromotion, setIsPromotion] = useState<boolean>(false);
  const [isTransfer, setIsTransfer] = useState<boolean>(false);

  useEffect(() => {
    formRef.resetFields();
    if (PromotionTransfer?.payload && id) {
      formRef.setFieldsValue(PromotionTransfer?.payload);
      setIsPromotion(PromotionTransfer?.payload?.isPromotion || false);
      setIsTransfer(PromotionTransfer?.payload?.isTransfer || false);
    } else {
      formRef.resetFields();
    }
  }, [PromotionTransfer, formRef, id]);
  const onfinish = async (values: PromotionTransferDTO) => {
    try {
      //if isTransfer = true → isPromotion = false
      values.isPromotion = !values.isTransfer;
      const result = id
        ? await updatePromotionTransfer({
            PromotionTransfer: values
          }).unwrap()
        : await newPromotionTransfer({ PromotionTransfer: values }).unwrap();
      if (result.success) {
        setVisible(false);
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };
  const handleGetPositionEmployeeByIdEmployee = async (idEmployee: string) => {
    try {
      await GetListPositionEmployeeByIdEmployee({
        idEmployee: idEmployee
      });
    } catch (e: any) {
      await HandleError(e);
    }
  };
  return (
    <div className="NewAndUpdatePromotionTransfer">
      <Spin spinning={LoadingPromotionTransfer}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item label="Mô tả" name={"description"}>
                <Input.TextArea />
              </Form.Item>
              <Form.Item label="Chọn phòng ban bổ nhiệm/điều chuyển" name={"unitName"} hidden>
                <Input />
              </Form.Item>
              <Form.Item
                label="Chọn phòng ban bổ nhiệm/điều chuyển"
                name={"idUnit"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn phòng ban"
                  }
                ]}
              >
                <Select
                  showSearch
                  onChange={(value, option: any) => formRef.setFieldsValue({ unitName: option?.name })}
                  loading={isLoadingListUnit}
                  optionFilterProp={"label"}
                  options={ListUnit?.listPayload?.map((x) => {
                    return {
                      name: x.unitName,
                      label: x.unitName + " - " + x.unitCode,
                      value: x.id
                    };
                  })}
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
                  showSearch
                  loading={isLoadingListEmployee}
                  onChange={(value) => handleGetPositionEmployeeByIdEmployee(value as string)}
                  optionFilterProp={"label"}
                  options={ListEmployee?.listPayload?.map((x) => {
                    return {
                      label: x.name + " - " + x.code + " - " + x.email,
                      value: x.id
                    };
                  })}
                />
              </Form.Item>
              <Form.Item name={"isHeadCount"} valuePropName={"checked"} initialValue={false}>
                <Checkbox>
                  <Typography.Text strong>Bổ nhiệm vị trí chính thức</Typography.Text>
                </Checkbox>
              </Form.Item>
              <Form.Item
                name={"isTransfer"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng chọn điều chuyển hoặc bổ nhiệm"
                  }
                ]}
              >
                <Radio.Group
                  optionType="button"
                  buttonStyle="solid"
                  onChange={(value) => {
                    let isTransfer = false;
                    let isPromotion = false;
                    let clearFields = {};

                    if (value.target.value === true) {
                      isTransfer = true;
                      clearFields = {
                        idPositionEmployeeCurrent: undefined,
                        positionNameCurrent: undefined
                      };
                    } else {
                      isPromotion = true;
                      clearFields = {
                        idCategoryPosition: undefined,
                        nameCategoryPosition: undefined
                      };
                    }

                    setIsTransfer(isTransfer);
                    setIsPromotion(isPromotion);
                    formRef.setFieldsValue(clearFields);

                    if (!value.target.checked) {
                      formRef.setFieldsValue(clearFields);
                    }
                  }}
                >
                  <Radio.Button value={true}>Nếu điều chuyển thì chọn</Radio.Button>
                  <Radio.Button value={false}>Nếu bổ nhiệm thì chọn</Radio.Button>
                </Radio.Group>
              </Form.Item>
              <div hidden={!isPromotion}>
                <Form.Item label="Tên vị trí bổ nhiệm" name={"nameCategoryPosition"} hidden>
                  <Input />
                </Form.Item>
                <Form.Item label="Vị trí bổ nhiệm" name={"idCategoryPosition"}>
                  <Select
                    showSearch
                    allowClear
                    loading={isLoadingCategoryPosition}
                    optionFilterProp={"label"}
                    onChange={(value, option: any) => formRef.setFieldsValue({ nameCategoryPosition: option?.name })}
                    options={ListCategoryPosition?.listPayload?.map((x) => {
                      return {
                        name: x.positionName,
                        label: x.positionName,
                        value: x.id
                      };
                    })}
                  />
                </Form.Item>
              </div>
              <div hidden={!isTransfer}>
                <Form.Item name={"positionNameCurrent"} hidden>
                  <Input />
                </Form.Item>
                <Form.Item label="Vị trí, chức vụ hiện tại" name={"idPositionEmployeeCurrent"}>
                  <Select
                    showSearch
                    allowClear
                    loading={isLoadingGetListPositionEmployeeByIdEmployee}
                    optionFilterProp={"label"}
                    onChange={(value, option: any) => formRef.setFieldsValue({ positionNameCurrent: option.label })}
                    options={ListPositionEmployeeByIdEmployee?.listPayload?.map((x) => {
                      const positionName = ListCategoryPosition?.listPayload?.find(
                        (y) => y.id === x.idPosition
                      )?.positionName;
                      return {
                        label: positionName,
                        value: x.idPosition
                      };
                    })}
                  />
                </Form.Item>
                <Form.Item name={"nameCategoryPosition"} hidden>
                  <Input />
                </Form.Item>
                <Form.Item label="Vị trí, chức vụ muốn điều chuyển" name={"idCategoryPosition"}>
                  <Select
                    showSearch
                    allowClear
                    loading={isLoadingCategoryPosition}
                    optionFilterProp={"label"}
                    onChange={(value, option: any) => formRef.setFieldsValue({ nameCategoryPosition: option?.name })}
                    options={ListCategoryPosition?.listPayload?.map((x) => {
                      return {
                        name: x.positionName,
                        label: x.positionName,
                        value: x.id
                      };
                    })}
                  />
                </Form.Item>
              </div>
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
                    loading={LoadingInsertPromotionTransfer || LoadingUpdatePromotionTransfer}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertPromotionTransfer || LoadingUpdatePromotionTransfer}
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

export const NewAndUpdatePromotionTransfer = WithErrorBoundaryCustom(_NewAndUpdatePromotionTransfer);
