import { Select, Space, Tooltip } from "antd";
import { vietnamFlag } from "@admin/asset/countryFlags";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";

function _HeaderLanguageSelect() {
  return (
    <div className="header-langue-select">
      <Select
        defaultValue={localStorage.getItem("lang") || "lang_vi"}
        style={{ width: 80 }}
        onChange={(value) => {
          localStorage.setItem("lang", value);
          window.location.reload();
        }}
      >
        <Select.Option value="lang_vi">
          <Tooltip title="Vietnamese">
            <Space direction="horizontal">
              <img src={vietnamFlag} alt="flag" />
              VN
            </Space>
          </Tooltip>
        </Select.Option>
        {/* <Option value="lang_en">
                <Tooltip title="English">
                  <Space direction="horizontal">
                    <img src={englist} alt="flag" />
                    EN
                  </Space>
                </Tooltip>
              </Option>
              <Option value="lang_cn">
                <Tooltip title="Chinese">
                  <Space direction="horizontal">
                    <img src={china} alt="flag" />
                    CN
                  </Space>
                </Tooltip>
              </Option>
              <Option value="lang_jp">
                <Tooltip title="Japanese">
                  <Space direction="horizontal">
                    <img src={japan} alt="flag" />
                    JP
                  </Space>
                </Tooltip>
              </Option>
              <Option value="lang_kr">
                <Tooltip title="Korean">
                  <Space direction="horizontal">
                    <img src={korean} alt="flag" />
                    KR
                  </Space>
                </Tooltip>
              </Option> */}
      </Select>
    </div>
  );
}

export const HeaderLanguageSelect = WithErrorBoundaryCustom(_HeaderLanguageSelect);
