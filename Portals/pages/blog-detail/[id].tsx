import moment from "moment";
import { PageTitle } from "@components/pageTille";
import { FacebookIcon, FacebookMessengerIcon, FacebookShareCount, RedditShareCount, TwitterIcon } from "react-share";
import EmailIcon from "react-share/lib/EmailIcon";
import EmailShareButton from "react-share/lib/EmailShareButton";
import FacebookMessengerShareButton from "react-share/lib/FacebookMessengerShareButton";
import FacebookShareButton from "react-share/lib/FacebookShareButton";
import LinkedinIcon from "react-share/lib/LinkedinIcon";
import LinkedinShareButton from "react-share/lib/LinkedinShareButton";
import RedditIcon from "react-share/lib/RedditIcon";
import RedditShareButton from "react-share/lib/RedditShareButton";
import TwitterShareButton from "react-share/lib/TwitterShareButton";
import WhatsappIcon from "react-share/lib/WhatsappIcon";
import WhatsappShareButton from "react-share/lib/WhatsappShareButton";
import Link from "next/link";
import { useRouter } from "next/router";
import NoImage from "@asset/images/logoBigSize.jpg";
import { SeoDefault } from "~/next-seo.config";
import { DefaultSeo } from "next-seo";
import { globalVariable } from "~/globalVariable";
import { ListResponse } from "@models/commom";
import { CategoryNewsDTO } from "@models/categoryNewsDTO";
import { NewsDTO } from "@models/newsDTO";
import { NewsApis } from "@apis/api/NewsApis";
import { CategoryNewsApis } from "@apis/api/CategoryNewsApis";
import { useEffect, useState } from "react";

function BlogDetail() {
  const router = useRouter();
  const { id } = router.query;
  const [apisID, setApisID] = useState<ListResponse<NewsDTO>>();
  const [apisCategoryNew, setApisCategoryNew] = useState<ListResponse<CategoryNewsDTO>>();
  const [apis, setApis] = useState<ListResponse<NewsDTO>>();
  useEffect(() => {
    (async () => {
      if (typeof id === "string") {
        const [apisID, apisCategoryNew, apis] = await Promise.all([
          NewsApis.getNewsById(id),
          CategoryNewsApis.getListCategoryNewsAvailable({
            pageNumber: 0,
            pageSize: 0
          }),
          NewsApis.getListNewsApproved({
            pageNumber: 1,
            pageSize: 5
          })
        ]);
        setApisID(apisID);
        setApisCategoryNew(apisCategoryNew);
        setApis(apis);
      }
    })();
  }, [id]);
  const SeoConfig = {
    ...SeoDefault,
    title: apisID?.payload?.title,
    description: apisID?.payload?.description,
    canonical: `${globalVariable.domain}/blog-detail/${router.query.id}`,
    openGraph: {
      ...SeoDefault.openGraph,
      title: apisID?.payload?.title,
      description: apisID?.payload?.description,
      url: `${globalVariable.domain}/blog-detail/${router.query.id}`,
      images: [
        {
          url: apisID?.payload?.avatar
            ? NewsApis.getFileImageNews(apisID?.payload?.id + "." + apisID?.payload?.extensionFile)
            : (NoImage as any),
          alt: apisID?.payload?.title
        }
      ]
    }
  };
  return (
    <div className="BlogDetail">
      <DefaultSeo {...SeoConfig} />
      <PageTitle
        title={apisID?.payload?.title || "Tin tức"}
        subTitle={
          apisCategoryNew?.listPayload?.find(
            (x) => x.id === apisID?.payload?.idCategoryNews
          )?.nameCategory || "Tin tức"
        }
        breadcrumb={"Home"}
        breadcrumbLink="/"
      />
      <div className="site-main">
        <div className="sidebar ttm-bgcolor-white clearfix">
          <div className="container">
            <h6 style={{ marginTop: 5 }}>
              {moment(apisID?.payload?.createdDateDisplay).format(
                "DD-MM-YYYY hh:mm:ss a"
              )}
            </h6>
            <div className="row">
              <div className="col-lg-9 content-area" style={{ paddingTop: 24 }}>
                {
                  <div
                    dangerouslySetInnerHTML={{
                      __html: apisID?.payload?.newsContent || ""
                    }}
                  ></div>
                }
                <div>
                  <h3>Chia sẻ</h3>
                  <div className="Demo__container">
                    <div className="Demo__some-network">
                      <FacebookShareButton
                        url={globalVariable.domain + "/" + router.asPath}
                        quote={apisID?.payload?.title}
                        className="Demo__some-network__share-button"
                      >
                        <FacebookIcon size={32} round />
                      </FacebookShareButton>
                      <div>
                        <FacebookShareCount
                          url={globalVariable.domain + "/" + router.asPath}
                          className="Demo__some-network__share-count"
                        />
                      </div>
                    </div>
                    <div className="Demo__some-network">
                      <FacebookMessengerShareButton
                        url={globalVariable.domain + "/" + router.asPath}
                        appId="100041352821968"
                        className="Demo__some-network__share-button"
                      >
                        <FacebookMessengerIcon size={32} round />
                      </FacebookMessengerShareButton>
                    </div>
                    <div className="Demo__some-network">
                      <TwitterShareButton
                        url={globalVariable.domain + "/" + router.asPath}
                        title={apisID?.payload?.title}
                        className="Demo__some-network__share-button"
                      >
                        <TwitterIcon size={32} round />
                      </TwitterShareButton>

                      <div className="Demo__some-network__share-count">
                        &nbsp;
                      </div>
                    </div>
                    <div className="Demo__some-network">
                      <WhatsappShareButton
                        url={globalVariable.domain + "/" + router.asPath}
                        title={apisID?.payload?.title}
                        separator=":: "
                        className="Demo__some-network__share-button"
                      >
                        <WhatsappIcon size={32} round />
                      </WhatsappShareButton>

                      <div className="Demo__some-network__share-count">
                        &nbsp;
                      </div>
                    </div>
                    <div className="Demo__some-network">
                      <LinkedinShareButton
                        url={globalVariable.domain + "/" + router.asPath}
                        className="Demo__some-network__share-button"
                      >
                        <LinkedinIcon size={32} round />
                      </LinkedinShareButton>
                    </div>
                    <div className="Demo__some-network">
                      <RedditShareButton
                        url={globalVariable.domain + "/" + router.asPath}
                        title={apisID?.payload?.title}
                        windowWidth={660}
                        windowHeight={460}
                        className="Demo__some-network__share-button"
                      >
                        <RedditIcon size={32} round />
                      </RedditShareButton>

                      <div>
                        <RedditShareCount
                          url={globalVariable.domain + "/" + router.asPath}
                          className="Demo__some-network__share-count"
                        />
                      </div>
                    </div>
                    <div className="Demo__some-network">
                      <EmailShareButton
                        url={globalVariable.domain + "/" + router.asPath}
                        subject={apisID?.payload?.title}
                        body="body"
                        className="Demo__some-network__share-button"
                      >
                        <EmailIcon size={32} round />
                      </EmailShareButton>
                    </div>
                  </div>
                </div>
              </div>

              <div className="col-lg-3 widget-area" style={{ marginTop: 24 }}>
                <aside className="widget widget-search">
                  <form
                    role="search"
                    method="get"
                    className="search-form  box-shadow"
                    action="#"
                  >
                    <div className="form-group">
                      <input
                        name="search"
                        type="text"
                        className="form-control bg-white"
                        placeholder="Search...."
                      />
                      <i className="fa fa-search"></i>
                    </div>
                  </form>
                </aside>
                <aside className="widget widget-categories">
                  <h3 className="widget-title">Danh mục</h3>
                  <ul style={{ height: 190, overflow: "auto" }}>
                    {apisCategoryNew?.listPayload?.map((item, index) => (
                      <li key={item.id}>
                        <Link
                          href={item?.showChild ? `/blog-category-child/${item.nameCategory}/${item.id}` : `/blog-list/${item.id}`}>
                          {item?.nameCategory}
                        </Link>
                      </li>
                    ))}
                  </ul>
                </aside>
                <aside className="widget post-widget">
                  <h3 className="widget-title">Tin tức</h3>
                  <ul className="widget-post ttm-recent-post-list">
                    {apis?.listPayload.map((item, index) => (
                      <li key={item.id}>
                        <Link href={`/blog-detail/${item.id}`}>
                          <img
                            src={
                              item.avatar
                                ? NewsApis.getFileImageNews(
                                  item.id + "." + "jpg"
                                ) : (NoImage as any)
                            }
                            alt="post-img"
                            width={360}
                            height={220}
                            style={{
                              objectFit: "cover"
                            }}
                          />
                        </Link>
                        <Link href={`/blog-detail/${item.id}`}>
                          {item.title}
                        </Link>
                        <span className="post-date">
                          <i className="fa fa-calendar"></i>
                          {moment(item.createdDate).format("MMM DD, YYYY")}
                        </span>
                      </li>
                    ))}
                  </ul>
                </aside>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default BlogDetail;
