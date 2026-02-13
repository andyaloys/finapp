import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { Penerima, CreatePenerimaDto, UpdatePenerimaDto } from '../models/penerima.model';

@Injectable({
  providedIn: 'root'
})
export class PenerimaService {
  private apiUrl = `${environment.apiUrl}/penerima`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<ApiResponse<Penerima[]>> {
    return this.http.get<ApiResponse<Penerima[]>>(this.apiUrl);
  }

  getAllActive(): Observable<ApiResponse<Penerima[]>> {
    return this.http.get<ApiResponse<Penerima[]>>(`${this.apiUrl}/active`);
  }

  getById(id: number): Observable<ApiResponse<Penerima>> {
    return this.http.get<ApiResponse<Penerima>>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreatePenerimaDto): Observable<ApiResponse<Penerima>> {
    return this.http.post<ApiResponse<Penerima>>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdatePenerimaDto): Observable<ApiResponse<Penerima>> {
    return this.http.put<ApiResponse<Penerima>>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`);
  }
}
