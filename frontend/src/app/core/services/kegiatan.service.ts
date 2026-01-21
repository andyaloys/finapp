import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { KegiatanDto, CreateKegiatanDto, UpdateKegiatanDto, PagedResult } from '../models/referensi.model';

@Injectable({
  providedIn: 'root'
})
export class KegiatanService {
  private apiUrl = `${environment.apiUrl}/Kegiatan`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = ''): Observable<{ success: boolean; data: PagedResult<KegiatanDto> }> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<{ success: boolean; data: PagedResult<KegiatanDto> }>(this.apiUrl, { params });
  }

  getById(id: string): Observable<{ success: boolean; data: KegiatanDto }> {
    return this.http.get<{ success: boolean; data: KegiatanDto }>(`${this.apiUrl}/${id}`);
  }

  getByProgramId(programId: string): Observable<{ success: boolean; data: KegiatanDto[] }> {
    return this.http.get<{ success: boolean; data: KegiatanDto[] }>(`${this.apiUrl}/program/${programId}`);
  }

  create(data: CreateKegiatanDto): Observable<{ success: boolean; data: KegiatanDto }> {
    return this.http.post<{ success: boolean; data: KegiatanDto }>(this.apiUrl, data);
  }

  update(id: string, data: UpdateKegiatanDto): Observable<{ success: boolean; data: KegiatanDto }> {
    return this.http.put<{ success: boolean; data: KegiatanDto }>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: string): Observable<{ success: boolean }> {
    return this.http.delete<{ success: boolean }>(`${this.apiUrl}/${id}`);
  }
}
