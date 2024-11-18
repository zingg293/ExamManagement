import { PageTitle } from "@components/pageTille";
import moment from "moment";
import Link from "next/link";
import { ListResponse } from "@models/commom";
import { useRouter } from "next/router";
import { SeoDefault } from "~/next-seo.config";
import { DefaultSeo } from "next-seo";
import { globalVariable } from "~/globalVariable";
import { NewsDTO } from "@models/newsDTO";
import { useEffect, useState } from "react";
import { NewsApis } from "@apis/api/NewsApis";
import { CompanyInformationApis } from "@apis/api/CompanyInformationApis";
import { CompanyInformationDTO } from "@models/companyInformationDTO";

function SearchNews() {
  const router = useRouter();
  const { filter } = router?.query;
  const [apis, setApis] = useState<ListResponse<NewsDTO>>();
  const [pageSize, setPageSize] = useState<number>(6);
  const [CompanyInformation, setCompanyInformation] = useState<CompanyInformationDTO>();

  useEffect(() => {
    (async () => {
      const res = await CompanyInformationApis.getListCompanyInformation();
      setCompanyInformation(res?.listPayload?.at(0));
    })();
  }, []);

  useEffect(() => {
    (async () => {
      if (typeof filter === "string") {
        const res = await NewsApis.searchNews({
          pageSize: pageSize as number,
          pageNumber: 1
        }, filter);
        setApis(res);
      }
    })();
  }, [pageSize, filter]);

  const SeoConfig = {
    ...SeoDefault,
    title: "Tin tức tuyển dụng",
    description: "Tổng hợp tin tức tuyển dụng mới nhất",
    canonical: `${globalVariable.domain}/search-news/${router.query.id}`,
    openGraph: {
      ...SeoDefault.openGraph,
      title: "Tin tức tuyển dụng",
      description: "Tổng hợp tin tức tuyển dụng mới nhất",
      url: `${globalVariable.domain}/search-news/${router.query.id}`
    }
  };
  return (
    <div className="BlogCategory">
      <DefaultSeo {...SeoConfig} />
      <PageTitle
        title={filter as string || "Tin tức tuyển dụng"}
        subTitle={filter as string || "Tin tức tuyển dụng"}
        breadcrumb={"Home"}
        breadcrumbLink="/"
      />
      <div className="site-main">
        <section className="ttm-row grid-section ttm-bgcolor-grey clearfix">
          <div className="container">
            <div className="row">
              {apis?.listPayload.map((item) => (
                <div className="col-lg-4 col-md-6" key={item.id}>
                  <div className="featured-imagebox featured-imagebox-blog style2 mb-30">
                    <div className="featured-thumbnail">
                      <img
                        className="img-fluid"
                        src={
                          item?.avatar ? NewsApis.getFileImageNews(item?.id + "." + item.extensionFile) :
                            CompanyInformationApis.getFileImage(CompanyInformation?.id + ".jpg")
                        }
                        alt="image"
                        style={{
                          height: 202,
                          width: 370,
                          objectFit: "cover"
                        }}
                      />
                      <div className="ttm-blog-overlay-iconbox">
                        <Link href={`/blog-detail/${item.id}`}>
                          <i className="fa fa-plus"></i>
                        </Link>
                      </div>
                      <div className="ttm-box-view-overlay"></div>
                    </div>
                    <div className="featured-content" style={{ height: 230 }}>
                      <div className="ttm-box-post-date">
                        <span className="ttm-entry-date">
                          <time
                            className="entry-date"
                            dateTime="2019-01-16T07:07:55+00:00"
                          >
                            {moment(item.createdDateDisplay).format("DD")}
                            <span className="entry-month entry-year">
                              {moment(item.createdDateDisplay).format("MMM")}
                            </span>
                          </time>
                        </span>
                      </div>
                      <div className="featured-title">
                        <div
                          style={{
                            height: 50,
                            overflow: "hidden",
                            display: "-webkit-box",
                            WebkitLineClamp: 2,
                            WebkitBoxOrient: "vertical",
                            marginBottom: 10
                          }}
                          data-tooltip={item?.title}
                        >
                          <h5 data-tooltip={item?.title}>
                            <Link href={`/blog-detail/${item.id}`}>
                              {item?.title}
                            </Link>
                          </h5>
                        </div>
                        <div
                          style={{
                            height: 75,
                            overflow: "hidden",
                            display: "-webkit-box",
                            WebkitLineClamp: 2,
                            WebkitBoxOrient: "vertical"
                          }}
                          data-tooltip={item?.description}
                        >
                          <p>{item?.description}</p>
                        </div>
                      </div>
                      <div className="post-meta">
                        <span className="ttm-meta-line">
                          <i className="fa fa-comments"></i>
                          Bình luận
                        </span>
                        <span className="ttm-meta-line">
                          <i className="fa fa-user"></i>
                          Admin
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
            <div className="row">
              <div className="col-md-12 text-center">
                {apis?.totalElement && Number(pageSize) < apis.totalElement && (
                  <a
                    className="ttm-btn ttm-btn-size-md ttm-btn-style-border  ttm-btn-color-blue"
                    onClick={() => setPageSize(pageSize + 6)}
                  >
                    Xem thêm
                  </a>
                )}
              </div>
            </div>
          </div>
        </section>
      </div>
    </div>
  );
}

export default SearchNews;

