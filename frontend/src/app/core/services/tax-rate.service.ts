import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TaxRate, CreateTaxRateDto, UpdateTaxRateDto, TaxRatesForCalculation, ApiResponse } from '../models/tax-rate.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TaxRateService {
  private apiUrl = `${environment.apiUrl}/taxrate`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<TaxRate[]>> {
    return this.http.get<ApiResponse<TaxRate[]>>(this.apiUrl);
  }

  getAllActive(): Observable<ApiResponse<TaxRate[]>> {
    return this.http.get<ApiResponse<TaxRate[]>>(`${this.apiUrl}/active`);
  }

  getTaxRatesForCalculation(): Observable<TaxRatesForCalculation> {
    return this.getAllActive().pipe(
      map(response => {
        const rates: TaxRatesForCalculation = {
          ppn: 0,
          pph21: 0,
          pph22: 0,
          pph23: 0
        };
        
        if (response.success && response.data) {
          response.data.forEach((rate: TaxRate) => {
            const code = rate.taxCode.toLowerCase();
            if (code === 'ppn') rates.ppn = rate.rate;
            else if (code === 'pph21') rates.pph21 = rate.rate;
            else if (code === 'pph22') rates.pph22 = rate.rate;
            else if (code === 'pph23') rates.pph23 = rate.rate;
          });
        }
        
        return rates;
      })
    );
  }

  getById(id: number): Observable<ApiResponse<TaxRate>> {
    return this.http.get<ApiResponse<TaxRate>>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateTaxRateDto): Observable<ApiResponse<TaxRate>> {
    return this.http.post<ApiResponse<TaxRate>>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdateTaxRateDto): Observable<ApiResponse<TaxRate>> {
    return this.http.put<ApiResponse<TaxRate>>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`);
  }
}
