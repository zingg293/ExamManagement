// interface cover responses data
export interface ListResponse<T> {
  payload: T;
  listPayload?: T[];
  message: string;
  success: boolean;
  fail: boolean;
  pageNumber: number;
  pageSize: number;
  totalElement: number;
  totalPages: number;
  loading: boolean;
}

export interface ListResponseFormData {
  data: {
    message: string;
    success: boolean;
  };
}

export type MergeTypes<A, B> = {
  [key in keyof A]: key extends keyof B ? B[key] : A[key];
} & B;

export interface ResponseStatus {
  message: string;
  success: boolean;
  fail: boolean;
}

export interface ResponseStatusWithToken extends ResponseStatus {
  token: string;
}

export interface payloadResult {
  message: string;
  success: boolean;
}

export interface pagination {
  pageSize: number;
  pageNumber: number;
}
