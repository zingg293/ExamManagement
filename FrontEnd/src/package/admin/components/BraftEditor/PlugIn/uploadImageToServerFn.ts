import { globalVariable } from "~/globalVariable";
import { getFileImageNews } from "@API/services/NewsApis.service";

export const uploadImageToServerFn = (param: any) => {
  const serverURL = `${globalVariable.urlServerApi}/api/v1/news/saveNewsImage`; // URL của service upload ảnh lên server
  const xhr = new XMLHttpRequest();
  const fd = new FormData();
  const successFn = (response: any) => {
    const url = getFileImageNews(response);
    param.success({
      url: url,
      meta: {
        id: response,
        title: param?.file?.name,
        alt: param?.file?.name,
        loop: false, // Bạn có thể thay đổi giá trị này để phù hợp với yêu cầu của bạn
        autoPlay: false, // Bạn có thể thay đổi giá trị này để phù hợp với yêu cầu của bạn
        controls: true // Bạn có thể thay đổi giá trị này để phù hợp với yêu cầu của bạn
      }
    });
  };
  const errorFn = (response: any) => {
    param.error({
      msg: `${response} Không thể tải lên hình ảnh.`
    });
  };

  const progressFn = (event: any) => {
    param.progress((event.loaded / event.total) * 100);
  };
  fd.append("image", param.file);
  xhr.onerror = () => {
    param.error({
      msg: "Lỗi trong quá trình tải lên hình ảnh."
    });
  };
  xhr.onreadystatechange = () => {
    if (xhr.readyState === 4) {
      if (xhr.status === 200) {
        successFn(xhr.responseText);
      } else {
        errorFn(xhr.responseText);
      }
    }
  };
  xhr.upload.onprogress = progressFn;
  xhr.open("POST", serverURL, true);
  xhr.send(fd);
};
