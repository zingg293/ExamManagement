import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  useGetListUnitAvailableQuery,
  useGetUnitByIdQuery,
  useInsertUnitMutation,
  useUpdateUnitMutation
} from "@API/services/UnitApis.service";
import { Button, Col, Form, Input, Row, Select, Space, Spin } from "antd";
import { HandleError } from "@admin/components";
import { UnitDTO } from "@models/unitDto";
import { CheckCircleOutlined, RetweetOutlined } from "@ant-design/icons";
import { useEffect } from "react";

interface IProps {
  setVisible: (value: boolean) => void;
  id?: string;
}

function _NewAndUpdateUnit(props: IProps) {
  const { setVisible, id } = props;
  const { data: Unit, isLoading: LoadingUnit } = useGetUnitByIdQuery(
    { id: id! },
    {
      skip: !id
    }
  );
  const { data: listUnit, isLoading: LoadingListUnit } = useGetListUnitAvailableQuery({ pageSize: 0, pageNumber: 0 });
  const [newUnit, { isLoading: LoadingInsertUnit }] = useInsertUnitMutation();
  const [updateUnit, { isLoading: LoadingUpdateUnit }] = useUpdateUnitMutation();
  const [formRef] = Form.useForm();

  useEffect(() => {
    if (Unit?.payload && id) {
      formRef.setFieldsValue(Unit?.payload);
    } else {
      formRef.resetFields();
    }
  }, [Unit, formRef, id]);
  const onfinish = async (values: UnitDTO) => {
    try {
      const result = id
        ? await updateUnit({
          unit: values
        }).unwrap()
        : await newUnit({ unit: values }).unwrap();
      if (result.success) {
        setVisible(false);
        formRef.resetFields();
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="NewAndUpdateUnit">
      <Spin spinning={LoadingUnit}>
        <Row>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form layout={"vertical"} form={formRef} onFinish={onfinish}>
              <Form.Item name={"id"} hidden />
              <Form.Item
                label="Tên phòng ban"
                name={"unitName"}
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập tên phòng ban"
                  }
                ]}
              >
                <Input />
              </Form.Item>
              <Form.Item label="Phòng ban cha" name={"parentId"}>
                <Select
                  loading={LoadingListUnit}
                  options={listUnit?.listPayload?.map((unit) => {
                    return {
                      label: unit.unitName,
                      value: unit.id
                    };
                  })}
                  showSearch
                  optionFilterProp={"label"}
                  allowClear={true}
                />
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
                    loading={LoadingInsertUnit || LoadingUpdateUnit}
                    icon={<RetweetOutlined />}
                  >
                    Xóa
                  </Button>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={LoadingInsertUnit || LoadingUpdateUnit}
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

export const NewAndUpdateUnit = WithErrorBoundaryCustom(_NewAndUpdateUnit);
