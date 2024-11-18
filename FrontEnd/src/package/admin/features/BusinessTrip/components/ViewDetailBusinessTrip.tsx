import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Button, Col, Descriptions, Modal, Row, Spin, Typography } from "antd";
import { getFileBusinessTrip, useGetBusinessTripByIdQuery } from "@API/services/BusinessTripApis.service";
import { useGetUnitByIdQuery } from "@API/services/UnitApis.service";
import { useGetUserByIdQuery } from "@API/services/UserApis.service";
import dayjs from "dayjs";
import { TeamOutlined } from "@ant-design/icons";
import { ViewDetailBusinessTripEmployee } from "@admin/features/BusinessTrip";
import { DownloadButton } from "@admin/components";

interface ViewDetailBusinessTripProps {
  idBusinessTrip: string;
}

function _ViewDetailBusinessTrip(props: ViewDetailBusinessTripProps) {
  const { idBusinessTrip } = props;
  const [modal, contextHolder] = Modal.useModal();
  const { data: dataBusinessTrip, isLoading: isLoadingBusinessTrip } = useGetBusinessTripByIdQuery(
    { idBusinessTrip },
    { skip: !idBusinessTrip }
  );
  const BusinessTrip = dataBusinessTrip?.payload;
  const { data: dataUnit } = useGetUnitByIdQuery(
    {
      id: BusinessTrip?.idUnit || ""
    },
    {
      skip: !BusinessTrip?.idUnit
    }
  );
  const { data: dataUser } = useGetUserByIdQuery(
    {
      id: BusinessTrip?.idUserRequest || ""
    },
    {
      skip: !BusinessTrip?.idUserRequest
    }
  );
  const handleViewBusinessTripEmployee = (idBusinessTrip: string) => {
    modal.confirm({
      title: "Xem chi tiết danh sách nhân viên tham gia",
      width: 1400,
      icon: <TeamOutlined />,
      content: <ViewDetailBusinessTripEmployee idBusinessTrip={idBusinessTrip} />,
      footer: null,
      closable: true
    });
  };

  return (
    <div className="ViewDetailBusinessTrip">
      {contextHolder}
      <Spin spinning={isLoadingBusinessTrip} size={"large"}>
        <Row>
          <Col span={24}>
            <Button type="primary" onClick={() => handleViewBusinessTripEmployee(idBusinessTrip)} block>
              {" "}
              Danh sách nhân viên tham gia{" "}
            </Button>
            <Typography.Title />
            <Descriptions bordered layout={"vertical"}>
              <Descriptions.Item label="Mô tả" span={3}>
                {BusinessTrip?.description}
              </Descriptions.Item>
              <Descriptions.Item label="Tên phòng ban">{BusinessTrip?.unitName}</Descriptions.Item>
              <Descriptions.Item label="Phòng ban" span={2}>
                {dataUnit?.payload?.unitName} - {dataUnit?.payload?.unitCode}
              </Descriptions.Item>
              <Descriptions.Item label="Ngưởi tạo">
                {dataUser?.payload?.fullname} - {dataUser?.payload?.email}
              </Descriptions.Item>
              <Descriptions.Item label="Thời gian">
                {dayjs(BusinessTrip?.startDate).format("DD/MM/YYYY hh:ss")} -{" "}
                {dayjs(BusinessTrip?.endDate).format("DD/MM/YYYY hh:ss")}
              </Descriptions.Item>
              <Descriptions.Item label="Đối tác làm việc">{BusinessTrip?.client}</Descriptions.Item>
              <Descriptions.Item label="Địa điểm công tác">{BusinessTrip?.businessTripLocation}</Descriptions.Item>
              <Descriptions.Item label="Phương tiện di chuyển">{BusinessTrip?.vehicle}</Descriptions.Item>
              <Descriptions.Item label="Kinh phí công tác">
                {BusinessTrip?.expense?.toLocaleString("vi-VN", {
                  style: "currency",
                  currency: "VND"
                })}
              </Descriptions.Item>
              <Descriptions.Item label="File đính kèm">
                {BusinessTrip?.attachments && (
                  <DownloadButton
                    downloadUrl={getFileBusinessTrip(
                      BusinessTrip?.id + "." + BusinessTrip?.attachments?.split(".").pop()
                    )}
                  />
                )}
              </Descriptions.Item>
            </Descriptions>
          </Col>
        </Row>
      </Spin>
    </div>
  );
}

export const ViewDetailBusinessTrip = WithErrorBoundaryCustom(_ViewDetailBusinessTrip);
