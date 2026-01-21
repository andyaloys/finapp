import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ProgramDto, CreateProgramDto, UpdateProgramDto, PagedResult } from '../models/referensi.model';

@Injectable({
  providedIn: 'root'
})
export class ProgramService {
  private apiUrl = `${environment.apiUrl}/program`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 10, searchTerm?: string): Observable<{ success: boolean; data: PagedResult<ProgramDto> }> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<{ success: boolean; data: PagedResult<ProgramDto> }>(this.apiUrl, { params });
  }

  getById(id: string): Observable<{ success: boolean; data: ProgramDto }> {
    return this.http.get<{ success: boolean; data: ProgramDto }>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateProgramDto): Observable<ProgramDto> {
    return this.http.post<ProgramDto>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateProgramDto): Observable<ProgramDto> {
    return this.http.put<ProgramDto>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
