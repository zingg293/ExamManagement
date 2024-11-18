import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { Select, Spin, Typography } from "antd";
import { SearchOutlined, ZoomInOutlined } from "@ant-design/icons";
import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { DataNavigate } from "@admin/components";
import { BaseSelectRef } from "rc-select";

function _HeaderAutoComplete() {
  const navigate = useNavigate();
  const listTreeNavigation = DataNavigate();
  const focusSearchKeyRef = useRef<BaseSelectRef>(null);
  const [loading, setLoading] = useState(true);
  const [value, setValue] = useState(null);
  const renderTitle = (title: string) => (
    <Typography.Text type={"secondary"} strong>
      {title}
    </Typography.Text>
  );
  const renderItem = (index: number, title: string, link: string) => ({
    key: link,
    value: link,
    title: title,
    label: (
      <div
        role={link}
        tabIndex={index}
        key={link}
        style={{
          display: "flex",
          justifyContent: "space-between"
        }}
      >
        {title}
        <span>
          <ZoomInOutlined />
        </span>
      </div>
    ),
    link: link
  });
  const options = listTreeNavigation?.map((items) => {
    return {
      label: renderTitle(items?.navigation.menuName),
      options: items?.navigationsChild?.map((x, index) => renderItem(index, x?.menuName, x?.path))
    };
  });
  useEffect(() => {
    setLoading(false);

    function handleKeyDown(event: any) {
      if (event.ctrlKey && event.key === "k") {
        event.preventDefault();
        if (focusSearchKeyRef.current) focusSearchKeyRef.current.focus();
      }
    }

    window.addEventListener("keydown", handleKeyDown);
    return () => {
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, []);

  return (
    <div className="HeaderAutoComplete" style={{ marginBottom: 10 }}>
      <Spin spinning={loading} size={"default"}>
        <Select
          ref={focusSearchKeyRef}
          showSearch
          size={"large"}
          style={{ width: 250, height: 35 }}
          popupMatchSelectWidth={250}
          placeholder={
            <p style={{ display: 'flex', alignItems: 'left', justifyContent: 'left', margin: 0 }}>
              <SearchOutlined />  Tìm kiếm danh mục
            </p>
          }
          value={value}
          // suffixIcon={
          //   <Typography.Text>
          //     <kbd>Ctrl</kbd>
          //     <kbd>k</kbd>
          //   </Typography.Text>
          // }
          options={options}
          optionFilterProp={"title"}
          onSelect={(value, option: any) => {
            setValue(null);
            navigate(option.link);
          }}
        />
      </Spin>
    </div>
  );
}

export const HeaderAutoComplete = WithErrorBoundaryCustom(_HeaderAutoComplete);
