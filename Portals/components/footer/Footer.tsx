import moment from "moment";
import { Fragment, useEffect, useState } from "react";
import Link from "next/link";
import WithErrorBoundaryCustom from "@utils/errorBounDary/WithErrorBoundaryCustom";
import NoImage from "@asset/images/logoBigSize.jpg";
import { CompanyInformationDTO } from "@models/companyInformationDTO";
import { CompanyInformationApis } from "@apis/api/CompanyInformationApis";
import { NewsApis } from "@apis/api/NewsApis";
import { NewsDTO } from "@models/newsDTO";

function _Footer() {
  const [isToTopVisible, setIsToTopVisible] = useState(false);
  const [CompanyInformation, setCompanyInformation] = useState<CompanyInformationDTO>();
  const [CategoryNews, setCategoryNews] = useState<NewsDTO[]>([]);

  useEffect(() => {
    (async () => {
      const [CompanyInformation, CategoryNews] = await Promise.all([CompanyInformationApis.getListCompanyInformation(), NewsApis.getListNewsApproved({
        pageNumber: 1,
        pageSize: 3
      })]);
      setCompanyInformation(CompanyInformation?.listPayload?.at(0));
      setCategoryNews(CategoryNews?.listPayload);
    })();
  }, []);
  const handleScroll = () => {
    if (window.scrollY >= 100) {
      setIsToTopVisible(true);
    } else {
      setIsToTopVisible(false);
    }
  };

  useEffect(() => {
    window.addEventListener("scroll", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  // @ts-ignore
  // @ts-ignore
  // @ts-ignore
  return (
    <Fragment>
      <div className="Footer">
        <footer className="footer widget-footer clearfix">
          <div className="first-footer ttm-bgcolor-skincolor ttm-bg ttm-bgimage-yes bg-img1">
            <div className="ttm-row-wrapper-bg-layer ttm-bg-layer"></div>
            <div className="container">
              <div className="row align-items-md-center">
                <div className="col-lg-8 col-md-4 col-sm-6 order-md-1">
                  <div className="text-left">
                    <div className="featured-icon-box left-icon icon-align-top">
                      <div className="featured-icon">
                        <div className="ttm-icon ttm-icon_element-color-white ttm-icon_element-size-md">
                          <i className="fa fa-map-marker"></i>
                        </div>
                      </div>
                      <div className="featured-content">
                        <div className="featured-desc">
                          <p>
                            {CompanyInformation?.address}
                          </p>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="col-lg-4 col-md-4 col-sm-6 order-md-3">
                  <div className="text-sm-right">
                    <a
                      className="ttm-btn ttm-btn-size-md ttm-btn-style-border ttm-icon-btn-left ttm-btn-color-white"
                      href={`mailto:${CompanyInformation?.email}`}
                      title=""
                    >
                      {CompanyInformation?.email}{" "}
                      <i className="fa fa-envelope-o"></i>
                    </a>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="second-footer ttm-textcolor-white bg-img2">
            <div className="container">
              <div className="row">
                <div className="col-xs-12 col-sm-12 col-md-6 col-lg-3 widget-area">
                  <div className="widget widget_text  clearfix">
                    <h3 className="widget-title">{CompanyInformation?.companyName}</h3>

                    <div className="quicklink-box">
                      <div className="featured-icon-box left-icon">
                        <div className="featured-content">
                          <div className="featured-desc">
                            <p>{"Giờ làm việc"}</p>
                          </div>
                          <div className="featured-title">
                            <h5>
                              {CompanyInformation?.openingHours}
                            </h5>
                          </div>
                          {/* <div className="featured-desc">
                            <p>Thứ 7</p>
                          </div>
                          <div className="featured-title">
                            <h5>8:00 sáng - 12:00 chiều</h5>
                          </div> */}

                          <div className="separator">
                            <div className="sep-line mt-25 mb-25"></div>
                          </div>

                          <div className="featured-desc">
                            <p>Bộ phận hỗ trợ</p>
                          </div>
                          <div className="featured-title">
                            <h5>
                              {CompanyInformation?.phoneNumber}
                            </h5>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="col-xs-12 col-sm-12 col-md-6 col-lg-3 widget-area">
                  <div className="widget link-widget clearfix">
                    <h3 className="widget-title">{"Link nhanh"}</h3>
                    <ul>
                      <li>
                        <Link href="/">Tin tức tuyển dụng</Link>
                      </li>
                      <li>
                        <Link href="/candidate">Người tìm việc</Link>
                      </li>
                    </ul>
                  </div>
                </div>
                <div className="col-xs-12 col-sm-12 col-md-6 col-lg-3 widget-area">
                  <div className="widget widget_text clearfix">
                    <h3 className="widget-title">Tin tức</h3>
                    <ul className="widget-post ttm-recent-post-list">
                      {CategoryNews.map((item, index) => (
                        <li key={item.id}>
                          <Link
                            href={`/blog-detail/${item.id}&null`}
                          >
                            <img
                              src={
                                item.avatar
                                  ? NewsApis.getFileImageNews(
                                    item.id + "." + item.extensionFile
                                  )
                                  : NoImage as any
                              }
                              alt="post-img"
                              style={{
                                width: "360",
                                height: "220",
                                objectFit: "cover"
                              }}
                            />
                          </Link>
                          <Link
                            data-tooltip={item.title}
                            href={`/blog-detail/${item.id}&null`}
                          >
                            {item.title?.slice(0, 65)}
                            {item.title?.length > 65 ? "..." : ""}
                          </Link>
                          <span className="post-date">
                                  <i className="fa fa-calendar"></i>
                            {moment(item.createdDate).format(
                              "MMM DD, YYYY"
                            )}
                                </span>
                        </li>
                      ))}
                    </ul>
                  </div>
                </div>
                <div className="col-xs-12 col-sm-12 col-md-6 col-lg-3 widget-area">
                  <div className="widget flicker_widget clearfix">
                    <h3 className="widget-title">Bản tin</h3>
                    <div className="textwidget widget-text">
                      Đăng ký ngay hôm nay để nhận được sự tư vất tốt nhất cho công việc của bạn
                      <h5 className="mb-20">Theo dõi chúng tôi</h5>
                      <div className="social-icons circle social-hover">
                        <ul className="list-inline">
                        </ul>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="bottom-footer-text ttm-bgcolor-darkgrey ttm-textcolor-white">
            <div className="container">
              <div className="row copyright">
                <div className="col-md-6">
                  <div className="">
                    <span>
                      {CompanyInformation?.copyright}
                    </span>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="text-md-right res-767-mt-10">
                    <div className="d-lg-inline-flex">
                      <ul id="menu-footer-menu" className="footer-nav-menu">
                        <li>
                          <Link href="/">Tin tức tuyển dụng</Link>
                        </li>
                        <li>
                          <Link href="/candidate">Người tìm việc</Link>
                        </li>
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </footer>
      </div>

      <a
        id="totop"
        onClick={() => window.scrollTo({ top: 0, behavior: "smooth" })}
        className={isToTopVisible ? "top-visible" : ""}
        style={{
          cursor: "pointer"
        }}
      >
        <i className="fa fa-angle-up"></i>
      </a>
    </Fragment>
  );
}

export const Footer = WithErrorBoundaryCustom(_Footer);
