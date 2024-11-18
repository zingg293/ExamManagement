import Link from "next/link";
import { PageTitle } from "@components/pageTille";
import { CategoryNewsDTO } from "@models/categoryNewsDTO";
import { ArticleJsonLd, DefaultSeo } from "next-seo";
import { SeoArticleJsonLd, SeoDefault } from "~/next-seo.config";
import { globalVariable } from "~/globalVariable";
import { CategoryNewsApis } from "@apis/api/CategoryNewsApis";
import { useEffect, useState } from "react";
import { CompanyInformationApis } from "@apis/api/CompanyInformationApis";
import { CompanyInformationDTO } from "@models/companyInformationDTO";

function blog() {
  const [apisCategory, setApisCategory] = useState<CategoryNewsDTO[]>([]);
  const [CompanyInformation, setCompanyInformation] = useState<CompanyInformationDTO>();

  useEffect(() => {
    (async () => {
      const [res,CompanyInformation] = await Promise.all([
        CategoryNewsApis.getListCategoryNewsAvailable({
          pageNumber: 0,
          pageSize: 0
        }),CompanyInformationApis.getListCompanyInformation()

      ])
      setApisCategory(res?.listPayload);
      setCompanyInformation(CompanyInformation?.listPayload?.at(0));
    })();
  }, []);
  const SeoConfig = {
    ...SeoDefault,
    title: "Tin tức tuyển dụng",
    description: "Tổng hợp tin tức tuyển dụng mới nhất",
    canonical: `${globalVariable.domain}/blog`,
    openGraph: {
      ...SeoDefault.openGraph,
      title: "Tin tức tuyển dụng",
      description: "Tổng hợp tin tức tuyển dụng mới nhất",
      url: `${globalVariable.domain}/blog`
    }
  };
  return (
    <div className="Blog">
      <DefaultSeo {...SeoConfig} />
      <ArticleJsonLd {...SeoArticleJsonLd} />
      <PageTitle
        title={"Tin tức tuyển dụng"}
        subTitle={"Tin tức tuyển dụng"}
        breadcrumb={"Home"}
        breadcrumbLink="/"
      />
      <div className="site-main">
        <section className="ttm-row grid-section ttm-bgcolor-grey clearfix">
          <div className="container">
            <div className="row">
              {apisCategory?.map((item) => (
                <div className="col-lg-4 col-md-6" key={item.id}>
                  <div className="featured-imagebox featured-imagebox-blog style2 mb-30">
                    <div className="featured-thumbnail">
                      <img
                        src={
                          item.avatar ? CategoryNewsApis.getFileImage(item?.id + '.' + item.avatar.split(".").pop()) : CompanyInformationApis.getFileImage(CompanyInformation?.id + ".jpg")
                        }
                        alt="image"
                        height={203}
                        width={330}
                      />
                      <div className="ttm-blog-overlay-iconbox">
                        <Link href={item?.showChild ? `/blog-category-child/${item.nameCategory}/${item.id}` : `/blog-list/${item.id}`}>
                          <i className="fa fa-plus"></i>
                        </Link>
                      </div>
                      <div className="ttm-box-view-overlay"></div>
                    </div>
                    <div className="featured-content">
                      <div className="featured-title">
                        <h5>
                          <Link href={item?.showChild ? `/blog-category-child/${item.nameCategory}/${item.id}` : `/blog-list/${item.id}`}>
                            {item.nameCategory}{" "}
                            <svg
                              viewBox="0 0 1024 1024"
                              version="1.1"
                              xmlns="http://www.w3.org/2000/svg"
                              width="24"
                              height="24"
                            >
                              <path
                                d="M922.026667 561.493333a136.533333 136.533333 0 0 1 0-98.986666l7.253333-19.626667a139.52 139.52 0 0 0-72.96-177.066667L837.12 256A141.226667 141.226667 0 0 1 768 186.88l-8.533333-19.2a139.52 139.52 0 0 0-177.066667-72.96l-19.626667 7.253333a136.533333 136.533333 0 0 1-98.986666 0l-19.626667-7.253333a139.52 139.52 0 0 0-178.346667 72.96L256 186.88A141.226667 141.226667 0 0 1 186.88 256l-19.2 8.533333a139.52 139.52 0 0 0-72.96 177.066667l7.253333 19.626667a136.533333 136.533333 0 0 1 0 98.986666l-7.253333 19.626667a139.52 139.52 0 0 0 72.96 177.066667l19.2 8.533333A141.226667 141.226667 0 0 1 256 837.12l8.533333 19.2a139.52 139.52 0 0 0 177.066667 72.96l19.626667-7.253333a136.533333 136.533333 0 0 1 98.986666 0l19.626667 7.253333a139.52 139.52 0 0 0 177.066667-72.96l8.533333-19.2A141.226667 141.226667 0 0 1 837.12 768l19.2-8.533333a139.52 139.52 0 0 0 72.96-177.066667z m-224.426667-146.346666l-239.786667 239.786666a20.48 20.48 0 0 1-29.866666 0L326.4 554.666667a21.333333 21.333333 0 0 1 0-30.293334l22.613333-22.613333a20.48 20.48 0 0 1 29.866667 0l64 63.573333 202.24-202.24a21.333333 21.333333 0 0 1 29.866667 0l22.613333 22.613334a21.333333 21.333333 0 0 1 0 29.44z"
                                fill="#1296db"
                              ></path>
                            </svg>
                          </Link>
                        </h5>
                      </div>
                      <div className="post-meta">
                        <svg
                          viewBox="0 0 1024 1024"
                          version="1.1"
                          xmlns="http://www.w3.org/2000/svg"
                          width="34"
                          height="34"
                        >
                          <path
                            d="M889.6 272l-758.4 0c-41.6 0-73.6 32-73.6 73.6l0 361.6c0 41.6 32 73.6 73.6 73.6l758.4 0c41.6 0 73.6-32 73.6-73.6l0-361.6C963.2 304 931.2 272 889.6 272zM320 646.4l-51.2 0-99.2-195.2 0 195.2-32 0 0-233.6 44.8 0 102.4 201.6 3.2 0 0-201.6 32 0L320 646.4zM400 512l124.8 0 0 32-124.8 0c0 3.2 0 3.2 0 6.4l0 0c0 41.6 12.8 60.8 38.4 60.8l89.6 0 0 32-89.6 0c-19.2 0-35.2-6.4-48-22.4-12.8-16-19.2-41.6-19.2-70.4 0-12.8 0-28.8 0-54.4 0-54.4 35.2-76.8 70.4-83.2l0 0c12.8 0 80 0 86.4 0l0 32c-28.8 0-73.6 0-83.2 0-12.8 3.2-41.6 12.8-41.6 51.2C400 502.4 400 505.6 400 512zM816 646.4l-44.8 0-44.8-185.6-51.2 185.6-48 0-67.2-233.6 35.2 0 57.6 192 3.2 0 51.2-192 38.4 0 51.2 201.6 60.8-201.6 32 0L816 646.4z"
                            fill="#1296db"
                          ></path>
                        </svg>
                        <span className="ttm-meta-line">
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </section>
      </div>
    </div>
  );
}

export default blog;



