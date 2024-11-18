import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import {
  getFileTicketLaborEquipment,
  useGetTicketLaborEquipmentByIdQuery
} from "@API/services/TicketLaborEquipmentApis.service";
import { Button, Col, Descriptions, Modal, Row, Spin, Tag, Typography } from "antd";
import { useGetUnitByIdQuery } from "@API/services/UnitApis.service";
import { useGetUserByIdQuery } from "@API/services/UserApis.service";
import { DownloadButton } from "@admin/components";
import { CheckCircleOutlined } from "@ant-design/icons";
import { ViewTicketLaborEquipmentDetail } from "@admin/features/TicketLaborEquipment";

export const listColorForTypeTicketLaborEquipment = (type: number | undefined) => {
  switch (type) {
    case 0:
      return <Tag color="blue-inverse">Mua mới</Tag>;
    case 1:
      return <Tag color="green-inverse">Sửa chữa</Tag>;
    case 2:
      return <Tag color="purple-inverse">Thu hồi, nhập kho</Tag>;
    default:
      return <></>;
  }
};
interface ViewTicketLaborEquipmentProps {
  idTicketLaborEquipment: string;
}

function _ViewTicketLaborEquipment(props: ViewTicketLaborEquipmentProps) {
  const { idTicketLaborEquipment } = props;
  const [modal, contextHolder] = Modal.useModal();
  const { data: dataTicketLaborEquipment, isLoading: isLoadingTicketLaborEquipment } =
    useGetTicketLaborEquipmentByIdQuery(
      {
        idTicketLaborEquipment: idTicketLaborEquipment
      },
      { skip: !idTicketLaborEquipment }
    );
  const ticketLaborEquipment = dataTicketLaborEquipment?.payload;
  const { data: dataUnit } = useGetUnitByIdQuery(
    {
      id: ticketLaborEquipment?.idUnit || ""
    },
    {
      skip: !ticketLaborEquipment?.idUnit
    }
  );
  const { data: dataUser } = useGetUserByIdQuery(
    { id: ticketLaborEquipment?.idUserRequest || "" },
    { skip: !ticketLaborEquipment?.idUserRequest }
  );
  const handleViewTicketLaborEquipmentDetail = (idTicketLaborEquipment: string) => {
    modal.confirm({
      title: "Xem chi tiết phiếu yêu cầu thiết bi lao động",
      width: 1400,
      icon: <CheckCircleOutlined />,
      content: (
        <ViewTicketLaborEquipmentDetail
          setVisible={() => Modal.destroyAll()}
          idTicketLaborEquipment={idTicketLaborEquipment}
        />
      ),
      footer: null,
      closable: true
    });
  };
  return (
    <div className="ViewTicketLaborEquipment">
      {contextHolder}
      <Spin spinning={isLoadingTicketLaborEquipment} size={"large"}>
        <Row>
          <Col span={24}>
            <Button
              type="primary"
              size={"middle"}
              block
              onClick={() => handleViewTicketLaborEquipmentDetail(ticketLaborEquipment?.id || "")}
            >
              Xem chi tiết phiếu yêu cầu thiết bi lao động
            </Button>
            <Typography.Title />
            <Descriptions bordered layout={"vertical"}>
              <Descriptions.Item label="Lý do" span={3}>
                {ticketLaborEquipment?.reason}
              </Descriptions.Item>
              <Descriptions.Item label="Mô tả" span={3}>
                {ticketLaborEquipment?.description}
              </Descriptions.Item>
              <Descriptions.Item label="Loại">
                {listColorForTypeTicketLaborEquipment(ticketLaborEquipment?.type)}
              </Descriptions.Item>
              <Descriptions.Item label="Phòng ban">
                {dataUnit?.payload?.unitName} - {dataUnit?.payload?.unitCode}
              </Descriptions.Item>
              <Descriptions.Item label="Ngưởi tạo">{dataUser?.payload?.fullname}</Descriptions.Item>
              <Descriptions.Item label="File đính kèm" span={3}>
                {ticketLaborEquipment?.fileAttachment && (
                  <DownloadButton
                    downloadUrl={getFileTicketLaborEquipment(
                      ticketLaborEquipment.id + "." + ticketLaborEquipment?.fileAttachment?.split(".")?.at(1)
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

export const ViewTicketLaborEquipment = WithErrorBoundaryCustom(_ViewTicketLaborEquipment);
