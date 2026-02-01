import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PpkBendaharaDto, CreatePpkBendaharaDto } from '../models/ppk-bendahara.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class PpkBendaharaService {
  private apiUrl = `${environment.apiUrl}/ppkbendahara`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<PpkBendaharaDto[]>> {
    return this.http.get<ApiResponse<PpkBendaharaDto[]>>(this.apiUrl);
  }

  getActive(): Observable<ApiResponse<PpkBendaharaDto[]>> {
    return this.http.get<ApiResponse<PpkBendaharaDto[]>>(`${this.apiUrl}/active`);
  }

  getById(id: string): Observable<ApiResponse<PpkBendaharaDto>> {
    return this.http.get<ApiResponse<PpkBendaharaDto>>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreatePpkBendaharaDto): Observable<ApiResponse<PpkBendaharaDto>> {
    return this.http.post<ApiResponse<PpkBendaharaDto>>(this.apiUrl, dto);
  }

  update(id: string, dto: CreatePpkBendaharaDto): Observable<ApiResponse<PpkBendaharaDto>> {
    return this.http.put<ApiResponse<PpkBendaharaDto>>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.apiUrl}/${id}`);
  }
}
