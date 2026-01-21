import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { OutputDto, CreateOutputDto, UpdateOutputDto, PagedResult } from '../models/referensi.model';

@Injectable({
  providedIn: 'root'
})
export class OutputService {
  private apiUrl = `${environment.apiUrl}/Output`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = ''): Observable<{ success: boolean; data: PagedResult<OutputDto> }> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<{ success: boolean; data: PagedResult<OutputDto> }>(this.apiUrl, { params });
  }

  getById(id: string): Observable<{ success: boolean; data: OutputDto }> {
    return this.http.get<{ success: boolean; data: OutputDto }>(`${this.apiUrl}/${id}`);
  }

  getByKegiatanId(kegiatanId: string): Observable<{ success: boolean; data: OutputDto[] }> {
    return this.http.get<{ success: boolean; data: OutputDto[] }>(`${this.apiUrl}/kegiatan/${kegiatanId}`);
  }

  create(data: CreateOutputDto): Observable<{ success: boolean; data: OutputDto }> {
    return this.http.post<{ success: boolean; data: OutputDto }>(this.apiUrl, data);
  }

  update(id: string, data: UpdateOutputDto): Observable<{ success: boolean; data: OutputDto }> {
    return this.http.put<{ success: boolean; data: OutputDto }>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: string): Observable<{ success: boolean }> {
    return this.http.delete<{ success: boolean }>(`${this.apiUrl}/${id}`);
  }
}
