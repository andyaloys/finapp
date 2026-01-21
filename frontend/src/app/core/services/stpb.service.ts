import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Stpb, CreateStpb, UpdateStpb } from '../models/stpb.model';
import { ApiResponse, PagedResult } from '../models/api-response.model';

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
}
