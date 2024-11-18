import { ArticleJsonLd, DefaultSeo } from "next-seo";
import { SeoArticleJsonLd, SeoDefault } from "~/next-seo.config";
import { PageTitle } from "@components/pageTille";
import { globalVariable } from "~/globalVariable";
import { candidateApis } from "@apis/api/CandidateApis";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import { CategoryCityApis } from "@apis/api/CategoryCityApis";
import { CategoryDistrictApis } from "@apis/api/CategoryDistrictApis";
import { CategoryWardApis } from "@apis/api/CategoryWardApis";

export default function Candidate() {
  const router = useRouter();
  const [categoryCity, setCategoryCity] = useState<any[]>([]);
  const [categoryDistrict, setCategoryDistrict] = useState<any[]>([]);
  const [categoryWard, setCategoryWard] = useState<any[]>([]);
  useEffect(() => {
    (async () => {
      const res = await CategoryCityApis.getListCategoryCityAvailable();
      setCategoryCity(res?.listPayload);
    })();
  }, []);
  const handleGetListDistrict = async (code: any) => {
    try {
      const res = await CategoryDistrictApis.getCategoryDistrictByCityCode(code);
      setCategoryDistrict(res?.listPayload);
    } catch (e) {
      console.log(e);
    }
  };
  const handleGetListWard = async (code: any) => {
    try {
      const res = await CategoryWardApis.getCategoryWardByDistrictCode(code);
      setCategoryWard(res?.listPayload);
    } catch (e) {
      console.log(e);
    }
  };
  const SeoConfig = {
    ...SeoDefault,
    title: "Người tìm việc",
    description: "Tổng hợp tin tức tuyển dụng mới nhất",
    canonical: `${globalVariable.domain}/candidate`,
    openGraph: {
      ...SeoDefault.openGraph,
      title: "Người tìm việc",
      description: "Tổng hợp tin tức tuyển dụng mới nhất",
      url: `${globalVariable.domain}/candidate`
    }
  };
  const handleSubmit = async (event: any) => {
    try {
      event.preventDefault();
      const form = event.target;
      const formData = new FormData(form);
      await candidateApis.insertCandidate(formData);
      window.alert("Thông tin của bạn đã được ghi nhận");
      router.reload();

    } catch (e) {
      console.log(e);
    }
  };
  return <div className={"candidate"}>
    <DefaultSeo {...SeoConfig} />
    <ArticleJsonLd {...SeoArticleJsonLd} />
    <PageTitle
      title={"Người tìm việc"}
      subTitle={"Người tìm việc"}
      breadcrumb={"Home"}
      breadcrumbLink="/"
    />
    <div className="site-main">

      <section className="ttm-row grid-section ttm-bgcolor-grey clearfix">
        <div className="container">
          <div className="row">
            <div className="col-lg-12">

              <form
                onSubmit={handleSubmit}
                //   action={
                //   globalVariable.urlServerApi + "/api/v1/Candidate/insertCandidate"
                // }
                //       method={"post"}
                //       encType="multipart/form-data"
              >
                <div className="row">
                  <div className="col-md-6">
                    <div className="mb-3 form-group">
                      <label htmlFor="name" className="form-label">Tên:</label>
                      <input type="text" className="form-control" id="name" name="Name" />
                    </div>
                    <div className="mb-3 form-group">
                      <label htmlFor="sex" className="form-label">Giới tính:</label>
                      <div className="form-check">
                        <input className="form-check-input" type="radio" name="Sex" id="male" value="true" />
                        <label className="form-check-label" htmlFor="male">
                          Nam
                        </label>
                      </div>
                      <div className="form-check">
                        <input className="form-check-input" type="radio" name="Sex" id="female" value="false" />
                        <label className="form-check-label" htmlFor="female">
                          Nữ
                        </label>
                      </div>
                    </div>
                    <div className="mb-3 form-group">
                      <label htmlFor="birthday" className="form-label">Ngày sinh:</label>
                      <input type="date" className="form-control" id="birthday" name="Birthday"
                      />
                    </div>
                    <div className="mb-3 form-group">
                      <label htmlFor="phone" className="form-label">Số điện thoại:</label>
                      <input type="text" className="form-control" id="phone" name="Phone" />
                    </div>
                    <div className="mb-3 form-group">
                      <label htmlFor="email" className="form-label">Email:</label>
                      <input type="text" className="form-control" id="email" name="Email" />
                    </div>
                  </div>
                  <div className="col-md-6">

                    <div className="mb-3 form-group">
                      <label htmlFor="city" className="form-label">Tỉnh/Thành phố:</label>
                      <select className="form-select" id="city" name="IdCity"
                              onChange={async (e) => {
                                const code = e.target.options[e.target.selectedIndex].dataset.code;
                                setCategoryDistrict([]);
                                setCategoryWard([]);
                                if (code)
                                  await handleGetListDistrict(code);
                              }}>
                        <option value={undefined} defaultChecked>-- Chọn --</option>
                        {categoryCity?.map((item) => (
                          <option key={item.id} value={item.id} data-code={item.cityCode}>{item.cityName}</option>
                        ))}
                      </select>
                    </div>
                    <div className="mb-3 form-group">
                      <label htmlFor="district" className="form-label">Quận/Huyện:</label>
                      <select className="form-select" id="district" name="IdDistrict" onChange={
                        async (e) => {
                          const code = e.target.options[e.target.selectedIndex].dataset.code;
                          setCategoryWard([]);
                          await handleGetListWard(code);
                        }
                      }>
                        {categoryDistrict?.map((item) => (
                          <option key={item.id} value={item.id}
                                  data-code={item.districtCode}>{item.districtName}</option>
                        ))}
                      </select>
                    </div>
                    <div className="mb-3 form-group">
                      <label htmlFor="ward" className="form-label">Phường/Xã:</label>
                      <select className="form-select" id="ward" name="IdWard">
                        {categoryWard?.map((item) => (
                          <option key={item.id} value={item.id} data-code={item.wardCode}>{item.wardName}</option>
                        ))}
                      </select>
                    </div>
                    <div className="mb-3 form-group">
                      <label htmlFor="address" className="form-label">Địa chỉ:</label>
                      <input type="text" className="form-control" id="address" name="Address" />
                    </div>
                    <div className="mb-3 form-group">
                      <label htmlFor="note" className="form-label">Ghi chú:</label>
                      <textarea className="form-control" id="note" name="Note"></textarea>
                    </div>
                    <div className="mb-3 form-group">
                      <label htmlFor="files" className="form-label">CV:</label>
                      <input className="form-control" type="file" id="Files" name="Files" />
                    </div>
                  </div>
                </div>
                <button type="submit" className="btn btn-primary">Gửi</button>
              </form>
            </div>
          </div>
        </div>
      </section>
    </div>
  </div>;
  ;
}