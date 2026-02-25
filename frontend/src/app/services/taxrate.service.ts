import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TaxRateDto, CreateTaxRateDto, UpdateTaxRateDto } from '../models/taxrate.model';
import { map } from 'rxjs/operators';

interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

@Injectable({
  providedIn: 'root'
})
export class TaxRateService {
  private apiUrl = `${environment.apiUrl}/taxrate`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<TaxRateDto[]> {
    return this.http.get<ApiResponse<TaxRateDto[]>>(this.apiUrl).pipe(
      map(response => response.data)
    );
  }

  getAllActive(): Observable<TaxRateDto[]> {
    return this.http.get<ApiResponse<TaxRateDto[]>>(`${this.apiUrl}/active`).pipe(
      map(response => response.data)
    );
  }

  getById(id: string): Observable<TaxRateDto> {
    return this.http.get<ApiResponse<TaxRateDto>>(`${this.apiUrl}/${id}`).pipe(
      map(response => response.data)
    );
  }

  getByTaxType(taxType: string): Observable<TaxRateDto[]> {
    return this.http.get<ApiResponse<TaxRateDto[]>>(`${this.apiUrl}/by-type/${taxType}`).pipe(
      map(response => response.data)
    );
  }

  create(data: CreateTaxRateDto): Observable<TaxRateDto> {
    return this.http.post<ApiResponse<TaxRateDto>>(this.apiUrl, data).pipe(
      map(response => response.data)
    );
  }

  update(id: string, data: UpdateTaxRateDto): Observable<TaxRateDto> {
    return this.http.put<ApiResponse<TaxRateDto>>(`${this.apiUrl}/${id}`, data).pipe(
      map(response => response.data)
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`).pipe(
      map(() => {})
    );
  }
}
