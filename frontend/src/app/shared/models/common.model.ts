export interface ApiResponse<T> {
  isSuccess: boolean;
  message?: string;
  data: T;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages?: number;
}
