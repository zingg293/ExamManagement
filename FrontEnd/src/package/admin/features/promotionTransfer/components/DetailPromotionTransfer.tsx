import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Col, Descriptions, Row, Spin, Tag } from "antd";
import { useGetPromotionTransferByIdQuery } from "@API/services/PromotionTransferApis.service";
import { useGetUnitByIdQuery } from "@API/services/UnitApis.service";
import { useGetUserByIdQuery } from "@API/services/UserApis.service";
import { CheckCircleOutlined } from "@ant-design/icons";
import { useGetEmployeeByIdQuery } from "@API/services/Employee.service";

interface IDetailPromotionTransferProps {
  idPromotionTransfer: string;
}

function _DetailPromotionTransfer(props: IDetailPromotionTransferProps) {
  const { idPromotionTransfer } = props;
  const { data: dataPromotionTransfer, isLoading: isLoadingPromotionTransfer } = useGetPromotionTransferByIdQuery(
    {
      idPromotionTransfer: idPromotionTransfer
    },
    { skip: !idPromotionTransfer }
  );
  const promotionTransfer = dataPromotionTransfer?.payload;
  const { data: dataUnit } = useGetUnitByIdQuery(
    {
      id: promotionTransfer?.idUnit || ""
    },
    {
      skip: !promotionTransfer?.idUnit
    }
  );
  const { data: dataUser } = useGetUserByIdQuery(
    { id: promotionTransfer?.idUserRequest || "" },
    { skip: !promotionTransfer?.idUserRequest }
  );
  const { data: dataEmployee } = useGetEmployeeByIdQuery(
    {
      idEmployee: promotionTransfer?.idEmployee || ""
    },
    {
      skip: !promotionTransfer?.idEmployee
    }
  );
  // const { data: dataCategoryPosition } = useGetCategoryPositionByIdQuery(
  //   {
  //     idCategoryPosition: promotionTransfer?.idCategoryPosition || ""
  //   },
  //   {
  //     skip: !promotionTransfer?.idCategoryPosition
  //   }
  // );
  // const { data: PositionEmployeeCurrent } = useGetCategoryPositionByIdQuery(
  //   {
  //     idCategoryPosition: promotionTransfer?.idPositionEmployeeCurrent || ""
  //   },
  //   {
  //     skip: !promotionTransfer?.idPositionEmployeeCurrent
  //   }
  // );

  return (
    <div className="DetailPromotionTransfer">
      <Spin spinning={isLoadingPromotionTransfer} size={"large"}>
        <Row>
          <Col span={24}>
            <Descriptions bordered layout={"vertical"}>
              <Descriptions.Item label="Mô tả" span={3}>
                {promotionTransfer?.description}
              </Descriptions.Item>
              <Descriptions.Item label="Tên phòng ban">{promotionTransfer?.unitName}</Descriptions.Item>
              <Descriptions.Item label="Phòng ban" span={2}>
                {dataUnit?.payload?.unitName} - {dataUnit?.payload?.unitCode}
              </Descriptions.Item>
              <Descriptions.Item label="Nhân viên">
                {dataEmployee?.payload?.name} - {dataEmployee?.payload?.code}
              </Descriptions.Item>
              <Descriptions.Item label="Ngưởi tạo" span={2}>
                {dataUser?.payload?.fullname} - {dataEmployee?.payload?.code}
              </Descriptions.Item>
              <Descriptions.Item label="Bổ nhiệm vị trí chính thức">
                {promotionTransfer?.isHeadCount && (
                  <Tag color={"green-inverse"}>
                    <CheckCircleOutlined
                      style={{
                        fontSize: "24px"
                      }}
                    />
                  </Tag>
                )}
              </Descriptions.Item>
              <Descriptions.Item label="Điều chuyển">
                {promotionTransfer?.isTransfer && (
                  <Tag color={"blue-inverse"}>
                    <CheckCircleOutlined
                      style={{
                        fontSize: "24px"
                      }}
                    />
                  </Tag>
                )}
              </Descriptions.Item>
              <Descriptions.Item label="Bổ nhiệm">
                {promotionTransfer?.isPromotion && (
                  <Tag color={"blue-inverse"}>
                    <CheckCircleOutlined
                      style={{
                        fontSize: "24px"
                      }}
                    />
                  </Tag>
                )}
              </Descriptions.Item>
              {promotionTransfer?.isPromotion && (
                <>
                  <Descriptions.Item label="Vị trí, chức vụ hiện tại muốn bổ nhiệm">
                    {promotionTransfer?.nameCategoryPosition}
                  </Descriptions.Item>
                  {/*<Descriptions.Item label="Vị trí, chức vụ hiện tại muốn bổ nhiệm" span={2}>*/}
                  {/*  {PositionEmployeeCurrent?.payload?.positionName}*/}
                  {/*</Descriptions.Item>*/}
                </>
              )}
              {promotionTransfer?.isTransfer && (
                <>
                  <Descriptions.Item label="Vị trí, chức vụ hiện tại">
                    {promotionTransfer?.positionNameCurrent}
                  </Descriptions.Item>
                  <Descriptions.Item label="Vị trí, chức vụ hiện tại muốn điều chuyển" span={2}>
                    {promotionTransfer?.nameCategoryPosition}
                  </Descriptions.Item>
                  {/*<Descriptions.Item label="Vị trí, chức vụ hiện tại muốn diều chuyển" span={2}>*/}
                  {/*  {dataCategoryPosition?.payload?.positionName}*/}
                  {/*</Descriptions.Item>*/}
                </>
              )}
            </Descriptions>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const DetailPromotionTransfer = WithErrorBoundaryCustom(_DetailPromotionTransfer);
