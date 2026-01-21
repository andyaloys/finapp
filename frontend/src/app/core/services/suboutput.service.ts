import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SuboutputDto, CreateSuboutputDto, UpdateSuboutputDto, PagedResult } from '../models/referensi.model';

@Injectable({
  providedIn: 'root'
})
export class SuboutputService {
  private apiUrl = `${environment.apiUrl}/Suboutput`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = ''): Observable<{ success: boolean; data: PagedResult<SuboutputDto> }> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<{ success: boolean; data: PagedResult<SuboutputDto> }>(this.apiUrl, { params });
  }

  getById(id: string): Observable<{ success: boolean; data: SuboutputDto }> {
    return this.http.get<{ success: boolean; data: SuboutputDto }>(`${this.apiUrl}/${id}`);
  }

  getByOutputId(outputId: string): Observable<{ success: boolean; data: SuboutputDto[] }> {
    return this.http.get<{ success: boolean; data: SuboutputDto[] }>(`${this.apiUrl}/output/${outputId}`);
  }

  create(data: CreateSuboutputDto): Observable<{ success: boolean; data: SuboutputDto }> {
    return this.http.post<{ success: boolean; data: SuboutputDto }>(this.apiUrl, data);
  }

  update(id: string, data: UpdateSuboutputDto): Observable<{ success: boolean; data: SuboutputDto }> {
    return this.http.put<{ success: boolean; data: SuboutputDto }>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: string): Observable<{ success: boolean }> {
    return this.http.delete<{ success: boolean }>(`${this.apiUrl}/${id}`);
  }
}
