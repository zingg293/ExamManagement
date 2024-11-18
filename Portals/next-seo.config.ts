import { globalVariable } from '~/globalVariable';

export const SeoDefault = {
  title: 'CÔNG TY TNHH HỆ THỐNG THÔNG TIN ĐẠI PHÁT ĐẠT',
  description:
    'Đại Phát Đạt Là công ty công nghệ chuyên gia công hệ thống phần mềm và các giải pháp chuyển đổi số cho doanh nghiệp.',
  canonical: globalVariable.domain,
  openGraph: {
    type: 'website',
    locale: 'vi_VN',
    url: globalVariable.domain,
    site_name: 'Đại Phát Đạt Company',
    title: 'CÔNG TY TNHH HỆ THỐNG THÔNG TIN ĐẠI PHÁT ĐẠT',
    description:
      'Đại Phát Đạt Là công ty công nghệ chuyên gia công hệ thống phần mềm và các giải pháp chuyển đổi số cho doanh nghiệp.',
    images: [
      {
        url: 'https://admin.daiphatdat.com:81/api/ContactAndIntroduction/GetFileImageIntroduction?fileNameId=708f5aaa-13ee-4e64-b571-31be3cddb576.jpg', // URL của hình ảnh
        width: 1200, // Chiều rộng của hình ảnh (pixels)
        height: 630, // Chiều cao của hình ảnh (pixels)
        alt: 'Mô tả hình ảnh', // Mô tả của hình ảnh
      },
    ],
  },
  twitter: {
    handle: '@handle', // Tài khoản Twitter của trang
    site: '@site', // Tài khoản Twitter của trang
    cardType: 'summary_large_image', // Loại thẻ Twitter Card (summary, summary_large_image, app, player)
  },
};
export const SeoArticleJsonLd = {
  useAppDir: true,
  url: globalVariable.domain,
  title: 'CÔNG TY TNHH HỆ THỐNG THÔNG TIN ĐẠI PHÁT ĐẠT',
  images: [
    'https://admin.daiphatdat.com:81/api/ContactAndIntroduction/GetFileImageIntroduction?fileNameId=708f5aaa-13ee-4e64-b571-31be3cddb576.jpg',
  ],
  datePublished: '2022-01-01T00:00:00Z',
  dateModified: '2023-01-01T00:00:00Z',
  authorName: 'Đại Phát Đạt Company',
  publisherName: 'Đại Phát Đạt Company',
  description:
    'Đại Phát Đạt Là công ty công nghệ chuyên gia công hệ thống phần mềm và các giải pháp chuyển đổi số cho doanh nghiệp.',
};
