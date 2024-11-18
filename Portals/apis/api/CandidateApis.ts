import { axiosClient } from "@apis/api/axiosClient";

export const candidateApis = {
  insertCandidate: (candidate: any) => {
    return axiosClient.post("/candidate/insertCandidate", candidate, {
      headers: {
        "Content-Type": "multipart/form-data"
      }
    });
  }
};