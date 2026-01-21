import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AkunDto, CreateAkunDto, UpdateAkunDto, PagedResult } from '../models/referensi.model';

@Injectable({
  providedIn: 'root'
})
export class AkunService {
  private apiUrl = `${environment.apiUrl}/Akun`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = ''): Observable<{ success: boolean; data: PagedResult<AkunDto> }> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<{ success: boolean; data: PagedResult<AkunDto> }>(this.apiUrl, { params });
  }

  getById(id: string): Observable<{ success: boolean; data: AkunDto }> {
    return this.http.get<{ success: boolean; data: AkunDto }>(`${this.apiUrl}/${id}`);
  }

  getBySubkomponenId(subkomponenId: string): Observable<{ success: boolean; data: AkunDto[] }> {
    return this.http.get<{ success: boolean; data: AkunDto[] }>(`${this.apiUrl}/subkomponen/${subkomponenId}`);
  }

  create(data: CreateAkunDto): Observable<{ success: boolean; data: AkunDto }> {
    return this.http.post<{ success: boolean; data: AkunDto }>(this.apiUrl, data);
  }

  update(id: string, data: UpdateAkunDto): Observable<{ success: boolean; data: AkunDto }> {
    return this.http.put<{ success: boolean; data: AkunDto }>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: string): Observable<{ success: boolean }> {
    return this.http.delete<{ success: boolean }>(`${this.apiUrl}/${id}`);
  }
}
