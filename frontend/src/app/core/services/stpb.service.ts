import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Stpb, CreateStpb, UpdateStpb } from '../models/stpb.model';
import { ApiResponse, PagedResult } from '../models/api-response.model';
import { CreateStpbDetailDto, StpbDetailDto } from '../models/stpb-detail.model';

@Injectable({
  providedIn: 'root'
})
export class StpbService {
  private apiUrl = `${environment.apiUrl}/stpb`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 10, searchTerm?: string): Observable<ApiResponse<PagedResult<Stpb>>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<ApiResponse<PagedResult<Stpb>>>(this.apiUrl, { params });
  }

  getById(id: string): Observable<ApiResponse<Stpb>> {
    return this.http.get<ApiResponse<Stpb>>(`${this.apiUrl}/${id}`);
  }

  create(stpb: CreateStpb): Observable<ApiResponse<Stpb>> {
    return this.http.post<ApiResponse<Stpb>>(this.apiUrl, stpb);
  }

  update(id: string, stpb: UpdateStpb): Observable<ApiResponse<Stpb>> {
    return this.http.put<ApiResponse<Stpb>>(`${this.apiUrl}/${id}`, stpb);
  }

  delete(id: string): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.apiUrl}/${id}`);
  }

  getMyStpbs(): Observable<ApiResponse<Stpb[]>> {
    return this.http.get<ApiResponse<Stpb[]>>(`${this.apiUrl}/my`);
  }

  // Workflow methods
  kirim(id: string): Observable<ApiResponse<Stpb>> {
    return this.http.post<ApiResponse<Stpb>>(`${this.apiUrl}/${id}/kirim`, {});
  }

  approve(id: string): Observable<ApiResponse<Stpb>> {
    return this.http.post<ApiResponse<Stpb>>(`${this.apiUrl}/${id}/approve`, {});
  }

  kembalikan(id: string, alasan: string): Observable<ApiResponse<Stpb>> {
    return this.http.post<ApiResponse<Stpb>>(`${this.apiUrl}/${id}/kembalikan`, { alasan });
  }

  // Detail management methods
  addDetail(stpbId: string, detail: CreateStpbDetailDto): Observable<ApiResponse<StpbDetailDto>> {
    return this.http.post<ApiResponse<StpbDetailDto>>(`${this.apiUrl}/${stpbId}/details`, detail);
  }

  updateDetail(stpbId: string, detailId: string, detail: CreateStpbDetailDto): Observable<ApiResponse<StpbDetailDto>> {
    return this.http.put<ApiResponse<StpbDetailDto>>(`${this.apiUrl}/${stpbId}/details/${detailId}`, detail);
  }

  deleteDetail(stpbId: string, detailId: string): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.apiUrl}/${stpbId}/details/${detailId}`);
  }

  // PDF download method
  downloadPdf(id: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${id}/pdf`, { responseType: 'blob' });
  }
}

