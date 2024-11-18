const lang = "vi";
const listUrl = {
  vi: "https://admin.daiphatdat.com:81",
  en: "https://admin.daiphatdat.com:81/en"
} as const;
export const globalVariable = {
  // urlServerApi: listUrl[lang as keyof typeof listUrl],
  urlServerApi: "http://beqlnsnote.dpdtech.vn",
  // urlServerApi: "https://localhost:7142",
  domain: "https://tuyendung.daiphatdat.com/",
  // domainEnglish: "https://en.daiphatdat.com"
} as const;
