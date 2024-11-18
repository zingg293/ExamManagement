import React, { useEffect, useMemo } from "react";
import {
  Button,
  ColorPicker,
  Divider,
  Drawer,
  Form,
  InputNumber,
  Popconfirm,
  Radio,
  Select,
  Slider,
  Space,
  Switch,
  Typography
} from "antd";
import { fontFamilyOptions } from "@admin/components/header/ListFontFamily";
import Title from "antd/es/typography/Title";
import { ReloadOutlined, SaveOutlined } from "@ant-design/icons";
import dayjs from "dayjs";

interface IProps {
  visibleDrawSetting: boolean;
  setVisibleDrawSetting: (value: boolean) => void;
}

export const TransferOptions: React.FC<IProps> = ({ visibleDrawSetting, setVisibleDrawSetting }) => {
  const listDarkModeColor = useMemo(
    () => [
      "#1f90e6", // Dark Blue
      "#08979c", // Dark Cyan
      "#237804", // Dark Green
      "#d48806", // Dark Gold
      "#cf1322", // Dark Red
      "#531dab", // Dark Purple
      "#ad2102", // Dark Magenta
      "#d4b106", // Dark Yellow
      "#8c8c8c", // Dark Grey
      "#d4380d", // Dark Orange
      "#d46b08", // Dark Orange Gold
      "#722ed1", // Dark Deep Purple
      "#41a746", // Dark Lime Green
      "#f1da36", // Dark Sunflower
      "#e6a23c", // Dark Orange Yellow
      "#df6d38" // Dark Peach
    ],
    []
  );
  const listLightModeColor = useMemo(
    () => [
      "#1890ff", // Blue
      "#13c2c2", // Cyan
      "#52c41a", // Green
      "#faad14", // Gold
      "#f5222d", // Red
      "#722ed1", // Purple
      "#eb2f96", // Magenta
      "#fadb14", // Yellow
      "#bfbfbf", // Grey
      "#fa541c", // Orange
      "#ff4d4f", // Coral Red
      "#9254de", // Deep Purple
      "#597ef7", // Indigo
      "#096dd9", // Dark Blue
      "#40a9ff", // Light Blue
      "#73d13d", // Lime Green
      "#ffec3d", // Sunflower
      "#ffc53d", // Orange Yellow
      "#ff7a45", // Peach
      "#ff85c0", // Pink
      "#a61d4c" // Ruby
    ],
    []
  );
  const settings = JSON.parse(localStorage.getItem("setting")!);
  const [form] = Form.useForm();
  useEffect(() => {
    if (!settings?.RandomPrimaryColorEachDay) return;
    const currentDate = dayjs().format("YYYY-MM-DD");
    let color = "";
    if (settings?.darkMode) {
      const randomIndex = parseInt(dayjs(currentDate).format("DD"), 10) % listDarkModeColor.length;
      color = listDarkModeColor[randomIndex];
    } else {
      const randomIndex = parseInt(dayjs(currentDate).format("DD"), 10) % listLightModeColor.length;
      color = listLightModeColor[randomIndex];
    }
    localStorage.setItem("setting", JSON.stringify({ ...settings, PrimaryColor: color }));
  }, [listDarkModeColor, listLightModeColor, settings]);
  const handleSaveSettingTheme = (values: any) => {
    localStorage.setItem("setting", JSON.stringify(values));
    window.location.reload();
  };
  const handleReset = () => {
    localStorage.removeItem("setting");
    window.location.reload();
  };
  return (
    <Drawer
      className="settings-drawer"
      mask={true}
      width={"auto"}
      onClose={() => setVisibleDrawSetting(false)}
      open={visibleDrawSetting}
    >
      <div>
        <div className="header-top">
          <Typography.Title level={4}>
            Cá nhân hóa
            <Typography.Text className="subtitle">Xem các tùy chọn bảng điều khiển của chúng tôi.</Typography.Text>
          </Typography.Title>
        </div>

        <div className="sidebar-color">
          <Form layout={"vertical"} onFinish={handleSaveSettingTheme} initialValues={settings} form={form}>
            <Form.Item name={"darkMode"} hidden />

            <Form.Item name={"PrimaryColor"} label={<Title level={5}>Màu chủ đạo</Title>}>
              <ColorPicker
                format={"hex"}
                showText
                size={"large"}
                onChange={(color, hex) => form.setFieldValue("PrimaryColor", hex)}
                presets={[
                  {
                    label: "Gợi ý cho light mode",
                    colors: listLightModeColor
                  },
                  {
                    label: "Gợi ý cho dark mode",
                    colors: listDarkModeColor
                  }
                ]}
              />
            </Form.Item>
            <Form.Item
              name={"RandomPrimaryColorEachDay"}
              valuePropName={"checked"}
              extra={
                <Typography.Text type={"secondary"}>
                  Khi bật, màu chủ đạo sẽ được chọn ngẫu nhiên mỗi ngày. Nếu bạn muốn sử dụng màu chủ đạo vui lòng tắt
                  chức năng này.
                </Typography.Text>
              }
            >
              <Switch checkedChildren={"Tắt"} unCheckedChildren={"Bật"} />
            </Form.Item>

            <Space
              direction={"horizontal"}
              align={"end"}
              size={"large"}
              style={{
                width: "100%"
              }}
              wrap
            >
              <Form.Item
                name={"BorderRadius"}
                label={<Title level={5}>Border Radius (độ bo góc cạnh) </Title>}
                initialValue={8}
              >
                <Slider />
              </Form.Item>
              <Form.Item name={"BorderRadius"} label={<></>}>
                <InputNumber />
              </Form.Item>
            </Space>
            <Form.Item name={"FontFamily"} label={<Title level={5}>Phông chữ</Title>}>
              <Select
                showSearch
                optionFilterProp={"label"}
                options={fontFamilyOptions.map((x, index) => ({
                  label: `${index}. ${index !== 0 ? x : "Default"}`,
                  value: x
                }))}
              />
            </Form.Item>
            <Form.Item name={"compactAlgorithm"} label={<Title level={5}>Chủ đề nhỏ gọn</Title>} initialValue={false}>
              <Radio.Group>
                <Radio.Button value={false}>Dạng mặc định</Radio.Button>
                <Radio.Button value={true}>Dạng nhỏ gọn</Radio.Button>
              </Radio.Group>
            </Form.Item>
            <Typography.Title level={5}>Màu nền thanh điều hướng</Typography.Title>
            <div className="trans">
              <Form.Item name={"styleSideNav"} initialValue={"transparent"}>
                <Radio.Group>
                  <Radio value={"transparent"}>Trong suốt</Radio>
                  <Radio value={"#fff"}>Trắng</Radio>
                  <Radio value={"#000"}>Đen</Radio>
                </Radio.Group>
              </Form.Item>
            </div>
            <Form.Item name={"fixedNavbar"} label={<Title level={5}>Ghim thanh tiêu đề</Title>} hidden>
              <Radio.Group>
                <Radio value={true}>Bật</Radio>
                <Radio value={false}>Tắt</Radio>
              </Radio.Group>
            </Form.Item>
            <Form.Item>
              <Space
                direction={"horizontal"}
                style={{
                  width: "100%",
                  justifyContent: "flex-end"
                }}
                wrap
              >
                <Popconfirm title={"Bạn chắc chắn muốn khôi phục cấu hình mặc định?"} onConfirm={() => handleReset()}>
                  <Button
                    htmlType={"button"}
                    style={{
                      float: "right"
                    }}
                    icon={<ReloadOutlined />}
                  >
                    Khôi phục cấu hình mặc định
                  </Button>
                </Popconfirm>
                <Button
                  type="primary"
                  htmlType={"submit"}
                  style={{
                    float: "right"
                  }}
                  icon={<SaveOutlined />}
                >
                  Lưu thay đổi
                </Button>
              </Space>
            </Form.Item>
          </Form>
          <Divider />
          <div className="ant-docment">
            {/* <ButtonContainer>
           <Button type="black" size="large">
             FREE DOWNLOAD
           </Button>
           <Button size="large">VIEW DOCUMENTATION</Button>
          </ButtonContainer> */}
          </div>
          <div className="viewstar">
            {/* <a href="#pablo">{<StarOutlined />} Star</a>
          <a href="#pablo"> 190</a> */}
          </div>

          <div className="ant-thank">
            <Title level={5} className="mb-2">
              Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!
            </Title>
            {/* <div className="social">
              <Button type="black">{<MailOutlined />}Email</Button>
              <Button type="black">{<FacebookFilled />}FaceBook</Button>
            </div> */}
          </div>
        </div>
      </div>
    </Drawer>
  );
};
