import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { KomponenDto, CreateKomponenDto, UpdateKomponenDto, PagedResult } from '../models/referensi.model';

@Injectable({
  providedIn: 'root'
})
export class KomponenService {
  private apiUrl = `${environment.apiUrl}/Komponen`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = ''): Observable<{ success: boolean; data: PagedResult<KomponenDto> }> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<{ success: boolean; data: PagedResult<KomponenDto> }>(this.apiUrl, { params });
  }

  getById(id: string): Observable<{ success: boolean; data: KomponenDto }> {
    return this.http.get<{ success: boolean; data: KomponenDto }>(`${this.apiUrl}/${id}`);
  }

  getBySuboutputId(suboutputId: string): Observable<{ success: boolean; data: KomponenDto[] }> {
    return this.http.get<{ success: boolean; data: KomponenDto[] }>(`${this.apiUrl}/suboutput/${suboutputId}`);
  }

  create(data: CreateKomponenDto): Observable<{ success: boolean; data: KomponenDto }> {
    return this.http.post<{ success: boolean; data: KomponenDto }>(this.apiUrl, data);
  }

  update(id: string, data: UpdateKomponenDto): Observable<{ success: boolean; data: KomponenDto }> {
    return this.http.put<{ success: boolean; data: KomponenDto }>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: string): Observable<{ success: boolean }> {
    return this.http.delete<{ success: boolean }>(`${this.apiUrl}/${id}`);
  }
}
