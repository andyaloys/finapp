import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ItemDto, CreateItemDto, UpdateItemDto, PagedResult } from '../models/referensi.model';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private apiUrl = `${environment.apiUrl}/Item`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = ''): Observable<{ success: boolean; data: PagedResult<ItemDto> }> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<{ success: boolean; data: PagedResult<ItemDto> }>(this.apiUrl, { params });
  }

  getById(id: string): Observable<{ success: boolean; data: ItemDto }> {
    return this.http.get<{ success: boolean; data: ItemDto }>(`${this.apiUrl}/${id}`);
  }

  getByAkunId(akunId: string): Observable<{ success: boolean; data: ItemDto[] }> {
    return this.http.get<{ success: boolean; data: ItemDto[] }>(`${this.apiUrl}/akun/${akunId}`);
  }

  create(data: CreateItemDto): Observable<{ success: boolean; data: ItemDto }> {
    return this.http.post<{ success: boolean; data: ItemDto }>(this.apiUrl, data);
  }

  update(id: string, data: UpdateItemDto): Observable<{ success: boolean; data: ItemDto }> {
    return this.http.put<{ success: boolean; data: ItemDto }>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: string): Observable<{ success: boolean }> {
    return this.http.delete<{ success: boolean }>(`${this.apiUrl}/${id}`);
  }
}
