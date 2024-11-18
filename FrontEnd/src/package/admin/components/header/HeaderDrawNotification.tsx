import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Avatar, Badge, Button, Drawer, List } from "antd";
import { BellOutlined, ClockCircleOutlined, LinkOutlined } from "@ant-design/icons";
import { Fragment, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function _HeaderDrawNotification() {
  const navigate = useNavigate();
  // const { data: dataNotification } = useInventoryAlertQuery({}, { pollingInterval: 60000 * 3 });
  const [openNotification, setOpenNotification] = useState(false);
  const dataNotification = {
    message: []
  };
  const contentNotification = dataNotification?.message?.map((item: any) => {
    return {
      title: `Phụ tùng ${item.item2} với mã ${item.item3} đã đến mức cảnh báo hết, vui lòng kiểm tra`,
      description: (
        <>
          <ClockCircleOutlined /> 3 phút trước
        </>
      ),
      avatar: <Avatar shape="square">{<LinkOutlined />}</Avatar>,
      id: item.item3
    };
  });
  const menuNotification = (
    <List
      min-width="100%"
      className="header-notifications-dropdown "
      itemLayout="horizontal"
      dataSource={contentNotification}
      renderItem={(item) => (
        <List.Item
          onClick={() => navigate(`/admin/quan-ly-phu-tung/${item?.id}`)}
          key={item?.id}
          style={{ background: "#fafafa", paddingLeft: 5, cursor: "pointer" }}
        >
          <List.Item.Meta
            avatar={<Avatar shape="square" src={item?.avatar} />}
            title={item?.title}
            description={item?.description}
          />
        </List.Item>
      )}
    />
  );
  useEffect(() => {
    if (dataNotification?.message?.length && dataNotification?.message?.length > 0) {
      document.title = `(${dataNotification?.message?.length}) Thông báo`;
    }
  }, [dataNotification?.message]);
  return (
    <Fragment>
      <Button
        type={"link"}
        className="ant-dropdown-link"
        onClick={(e) => {
          e.preventDefault();
          setOpenNotification(true);
        }}
      >
        <Badge size="small" count={dataNotification?.message?.length}>
          <BellOutlined
            style={{
              color: JSON.parse(localStorage.getItem("setting") || "")?.PrimaryColor
            }}
          />
        </Badge>
      </Button>

      <Drawer
        title={
          <>
            <BellOutlined
              style={{
                color: JSON.parse(localStorage.getItem("setting") || "")?.PrimaryColor
              }}
            />{" "}
            Thông báo
          </>
        }
        placement={"right"}
        onClose={() => {
          setOpenNotification(false);
        }}
        open={openNotification}
        mask={true}
      >
        <div className="notification">{menuNotification}</div>
      </Drawer>
    </Fragment>
  );
}

export const HeaderDrawNotification = WithErrorBoundaryCustom(_HeaderDrawNotification);
