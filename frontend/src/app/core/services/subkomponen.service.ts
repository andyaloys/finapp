import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SubkomponenDto, CreateSubkomponenDto, UpdateSubkomponenDto, PagedResult } from '../models/referensi.model';

@Injectable({
  providedIn: 'root'
})
export class SubkomponenService {
  private apiUrl = `${environment.apiUrl}/Subkomponen`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = ''): Observable<{ success: boolean; data: PagedResult<SubkomponenDto> }> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<{ success: boolean; data: PagedResult<SubkomponenDto> }>(this.apiUrl, { params });
  }

  getById(id: string): Observable<{ success: boolean; data: SubkomponenDto }> {
    return this.http.get<{ success: boolean; data: SubkomponenDto }>(`${this.apiUrl}/${id}`);
  }

  getByKomponenId(komponenId: string): Observable<{ success: boolean; data: SubkomponenDto[] }> {
    return this.http.get<{ success: boolean; data: SubkomponenDto[] }>(`${this.apiUrl}/komponen/${komponenId}`);
  }

  create(data: CreateSubkomponenDto): Observable<{ success: boolean; data: SubkomponenDto }> {
    return this.http.post<{ success: boolean; data: SubkomponenDto }>(this.apiUrl, data);
  }

  update(id: string, data: UpdateSubkomponenDto): Observable<{ success: boolean; data: SubkomponenDto }> {
    return this.http.put<{ success: boolean; data: SubkomponenDto }>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: string): Observable<{ success: boolean }> {
    return this.http.delete<{ success: boolean }>(`${this.apiUrl}/${id}`);
  }
}
