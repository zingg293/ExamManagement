// interface cover respones data

export interface Pagination {
  pageNumber: number;
  pageSize: number;
}
export interface ListResponse<T> {
  payload: T;
  listPayload: T[];
  message?: string;
  success?: boolean;
  fail?: boolean;
  pageNumber?: number;
  pageSize?: number;
  totalElement?: number;
  totalPages?: number;
  loading?: boolean;
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

export type PartialRequired<T, K extends keyof T> = Omit<T, K> &
  Required<Pick<T, K>>;

export type PartialRequiredId<T, K extends keyof T> = Omit<T, K> &
  Required<Pick<T, K>> & { id: string };
