import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { GetFIleEmployee, useGetEmployeeByIdQuery } from "@API/services/Employee.service";
import { Avatar, Col, Descriptions, Row, Spin, Tag } from "antd";
import dayjs from "dayjs";
import { useGetCategoryWardByIdQuery } from "@API/services/CategoryWardApis.service";
import { useGetCategoryDistrictByIdQuery } from "@API/services/CategorryDistrict.service";
import { useGetCategoryCityByIdQuery } from "@API/services/CategoryCity.service";
import { useGetEmployeeTypeByIdQuery } from "@API/services/EmployeeType.service";
import { useGetListUnitQuery, useGetUnitByIdQuery } from "@API/services/UnitApis.service";
import { useGetListCategoryPositionAvailableQuery } from "@API/services/CategoryPositionApis.service";

interface IProps {
  idEmployee: string;
}

function _DetailEmployee(props: IProps) {
  const { idEmployee } = props;
  const { data: dataEmployee, isLoading: isLoadingEmployee } = useGetEmployeeByIdQuery(
    {
      idEmployee
    },
    {
      skip: !idEmployee
    }
  );
  const employee = dataEmployee?.payload;
  const { data: dataCategoryWard } = useGetCategoryWardByIdQuery(
    {
      idCategoryWard: employee?.idWard || ""
    },
    {
      skip: !employee?.idWard
    }
  );
  const { data: dataCategoryDistrict } = useGetCategoryDistrictByIdQuery(employee?.idDistrict || "", {
    skip: !employee?.idDistrict
  });
  const { data: dataCategoryCity } = useGetCategoryCityByIdQuery(
    {
      idCategoryCity: employee?.idCity || ""
    },
    {
      skip: !employee?.idCity
    }
  );
  const { data: dataEmployeeType } = useGetEmployeeTypeByIdQuery(
    {
      id: employee?.typeOfEmployee || ""
    },
    {
      skip: !employee?.typeOfEmployee
    }
  );
  const { data: dataUnit } = useGetUnitByIdQuery(
    {
      id: employee?.idUnit || ""
    },
    {
      skip: !employee?.idUnit
    }
  );
  const { data: dataUnitList } = useGetListUnitQuery({
    pageSize: 0,
    pageNumber: 0
  });
  const { data: dataCategoryPositionList } = useGetListCategoryPositionAvailableQuery({
    pageSize: 0,
    pageNumber: 0
  });
  return (
    <div className="DetailEmployee">
      <Spin spinning={isLoadingEmployee} size={"large"}>
        <Row>
          <Col span={24}>
            {employee?.avatar && (
              <Avatar
                size={150}
                src={GetFIleEmployee(employee?.id + ".jpg")}
                style={{
                  marginBottom: -150,
                  float: "right"
                }}
              />
            )}
            <Descriptions bordered layout={"horizontal"} column={1}>
              <Descriptions.Item label="Họ và tên">{employee?.name}</Descriptions.Item>
              <Descriptions.Item label="Email">{employee?.email}</Descriptions.Item>
              <Descriptions.Item label="Số điện thoại">{employee?.phone}</Descriptions.Item>
              <Descriptions.Item label="Ngày sinh">{dayjs(employee?.birthday).format("DD/MM/YYYY")}</Descriptions.Item>
              <Descriptions.Item label="Giới tính">{employee?.sex ? "Nam" : "Nữ"}</Descriptions.Item>

              <Descriptions.Item label="Tỉnh/Thành phố">{dataCategoryCity?.payload?.cityName}</Descriptions.Item>
              <Descriptions.Item label="Huyện">{dataCategoryDistrict?.payload?.districtName}</Descriptions.Item>
              <Descriptions.Item label="Xã/Phường">{dataCategoryWard?.payload?.wardName}</Descriptions.Item>
              <Descriptions.Item label="Địa chỉ">{employee?.address}</Descriptions.Item>

              <Descriptions.Item label="Loại nhân viên">
                <Tag color={"magenta-inverse"}>{dataEmployeeType?.payload?.typeName}</Tag>
              </Descriptions.Item>
              <Descriptions.Item label="Phòng ban">
                <Tag color="blue-inverse">
                  {dataUnit?.payload?.unitName} - {dataUnit?.payload?.unitCode}
                </Tag>
              </Descriptions.Item>
              <Descriptions.Item label="Số tài khoản">{employee?.accountNumber}</Descriptions.Item>
              {/*<Descriptions.Item label="Mã số thuế">{employee?.taxNumber}</Descriptions.Item>*/}
              <Descriptions.Item label="Vị trí nhân viên">
                <Descriptions bordered>
                  {employee?.positionEmployees?.map((x) => {
                    const unit = dataUnitList?.listPayload?.find((item) => item.id === x.idUnit);
                    const positionName = dataCategoryPositionList?.listPayload?.find(
                      (item) => item.id === x.idPosition
                    )?.positionName;
                    return (
                      <Descriptions.Item key={x.id}>
                        {positionName} | {unit?.unitName} - {unit?.unitCode}{" "}
                        {x.isHeadcount ? <Tag color={"green-inverse"}>Chính thức</Tag> : ""}
                      </Descriptions.Item>
                    );
                  })}
                </Descriptions>
              </Descriptions.Item>
              <Descriptions.Item label="Ghi chú" span={3}>
                {employee?.note}
              </Descriptions.Item>

              <Descriptions.Item label="Thâm niên( năm )">{employee?.jobGrade}</Descriptions.Item>
              <Descriptions.Item label="% thuế">{employee?.taxPercent}</Descriptions.Item>
              <Descriptions.Item label="% BHXH">{employee?.socialInsurancePercent}</Descriptions.Item>
              <Descriptions.Item label="Lương cơ bản">
                {employee?.salaryBase?.toLocaleString("it-IT", {
                  style: "currency",
                  currency: "VND"
                })}
              </Descriptions.Item>
            </Descriptions>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const DetailEmployee = WithErrorBoundaryCustom(_DetailEmployee);
