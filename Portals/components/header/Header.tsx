import { useEffect, useRef, useState } from "react";
import WithErrorBoundaryCustom from "@utils/errorBounDary/WithErrorBoundaryCustom";
import Link from "next/link";
import { useRouter } from "next/router";
import { CompanyInformationApis } from "@apis/api/CompanyInformationApis";
import { CompanyInformationDTO } from "@models/companyInformationDTO";

function _Header() {
  const router = useRouter();
  const [isSearchOpen, setIsSearchOpen] = useState(false);
  const [menuDropdownVisible, setMenuDropdownVisible] = useState(false);
  const headerRef = useRef<HTMLDivElement>(null);
  const [CompanyInformation, setCompanyInformation] = useState<CompanyInformationDTO>();

  useEffect(() => {
    (async () => {
      const res = await CompanyInformationApis.getListCompanyInformation();
      setCompanyInformation(res?.listPayload?.at(0));
    })();
  }, []);

  useEffect(() => {
    const handleScroll = () => {
      if (window.scrollY >= 50) {
        headerRef.current?.classList.add("fixed-header");
        headerRef.current?.classList.add("visible-title");
      } else {
        headerRef.current?.classList.remove("fixed-header");
        headerRef.current?.classList.remove("visible-title");
      }
    };
    window.addEventListener("scroll", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);
  return (
    <div className="Header">
      <header id="masthead" className="header ttm-header-style-01">
        <div className="ttm-topbar-wrapper clearfix">
          <div className="container">
            <div className="ttm-topbar-content">
              <ul className="top-contact text-left">
                <li>
                  <i className="fa fa-map-marker"></i>
                  {CompanyInformation?.address}
                </li>
                <li>
                  <i className="fa fa-envelope-o"></i>
                  <a href={`mailto:${CompanyInformation?.email}`}>
                    {CompanyInformation?.email}
                  </a>
                </li>
              </ul>
              <div className="topbar-right text-right">
                <ul className="top-contact">
                  <li>
                    <i className="fa fa-clock-o"></i>
                    Giờ làm việc:{" "}
                    {CompanyInformation?.openingHours}
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </div>
        <div className="ttm-header-wrap">
          <div
            id="ttm-stickable-header-w"
            className="ttm-stickable-header-w clearfix"
          >
            <div id="site-header-menu" className="site-header-menu">
              <div
                className="site-header-menu-inner ttm-stickable-header"
                ref={headerRef}
              >
                <div className="container">
                  <div className="site-branding">
                    <Link
                      className="home-link"
                      href="/"
                      title="Trang chủ"
                      rel="home"
                    >
                      <img
                        id="logo-img"
                        className="img-center lazyload"
                        src={CompanyInformationApis.getFileImage(
                          CompanyInformation?.id +
                          ".jpg"
                        )}
                        alt="logo"
                        style={{
                          objectFit: "cover"
                        }}
                      />

                    </Link>
                  </div>

                  <div id="site-navigation" className="site-navigation">
                    <div className="ttm-rt-contact">
                      <div className="ttm-header-icons">
                        <div className="ttm-header-icon ttm-header-search-link">
                          <a
                            href="#"
                            onClick={(e) => {
                              e.preventDefault();
                              setIsSearchOpen(!isSearchOpen);
                            }}
                            className={`${isSearchOpen ? "open" : ""}`}
                          >
                            <i
                              className={`fa ${
                                isSearchOpen ? "fa-close" : "fa-search"
                              }`}
                            ></i>
                          </a>

                          <div
                            className={`ttm-search-overlay ${
                              isSearchOpen ? "st-show" : ""
                            }`}
                          >
                            <form
                              className="ttm-site-searchform"
                              onSubmit={(e) => {
                                e.preventDefault();
                                const search = e.currentTarget.search.value;
                                if (search) {
                                  router.push(
                                    `/search-news/${search}`
                                  );
                                }
                              }}
                            >
                              <div className="w-search-form-h">
                                <div className="w-search-form-row">
                                  <div className="w-search-input">
                                    <input
                                      type="search"
                                      className="field searchform-s"
                                      name="search"
                                      placeholder={"Tìm kiếm"}
                                      required={true}
                                    />
                                    <button type="submit">
                                      <i className="fa fa-search"></i>
                                    </button>
                                  </div>
                                </div>
                              </div>
                            </form>
                          </div>
                        </div>
                      </div>
                    </div>

                    <div
                      className="ttm-menu-toggle"
                      onClick={(e) => {
                        e.preventDefault();
                        setMenuDropdownVisible(!menuDropdownVisible);
                      }}
                    >
                      <input type="checkbox" id="menu-toggle-form" />
                      <label
                        htmlFor="menu-toggle-form"
                        className="ttm-menu-toggle-block"
                      >
                        <span className="toggle-block toggle-blocks-1"></span>
                        <span className="toggle-block toggle-blocks-2"></span>
                        <span className="toggle-block toggle-blocks-3"></span>
                      </label>
                    </div>
                    <nav
                      id="menu"
                      className={`menu ${menuDropdownVisible ? "active" : ""}`}
                    >
                      <ul className="dropdown">
                        <li
                          key={"1"}
                          className={
                            router.pathname.length === 1
                              ? "active"
                              : ""
                          }
                        >
                          <Link
                            href={"/"}
                            onClick={() => setMenuDropdownVisible(false)}
                          >
                            Tin tức tuyển dụng
                          </Link>
                        </li>
                        <li
                          key={"2"}
                          className={
                            router.pathname.includes("/candidate")
                              ? "active"
                              : ""
                          }
                        >
                          <Link
                            href={"/candidate"}
                            onClick={() => setMenuDropdownVisible(false)}
                          >
                            Người tìm việc
                          </Link>
                        </li>

                      </ul>
                    </nav>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </header>
    </div>
  );
}

export const Header = WithErrorBoundaryCustom(_Header);
