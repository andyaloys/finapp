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

  getByTaxType(taxType: string): Observable<ApiResponse<TaxRate[]>> {
    return this.http.get<ApiResponse<TaxRate[]>>(`${this.apiUrl}/by-type/${taxType}`);
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
          // Get default rate for each tax type
          const ppnDefault = response.data.find(r => r.taxType === 'PPN' && r.isDefault);
          const pph21Default = response.data.find(r => r.taxType === 'PPH21' && r.isDefault);
          const pph22Default = response.data.find(r => r.taxType === 'PPH22' && r.isDefault);
          const pph23Default = response.data.find(r => r.taxType === 'PPH23' && r.isDefault);
          
          if (ppnDefault) rates.ppn = ppnDefault.rate;
          if (pph21Default) rates.pph21 = pph21Default.rate;
          if (pph22Default) rates.pph22 = pph22Default.rate;
          if (pph23Default) rates.pph23 = pph23Default.rate;
        }
        
        return rates;
      })
    );
  }

  getById(id: string): Observable<ApiResponse<TaxRate>> {
    return this.http.get<ApiResponse<TaxRate>>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateTaxRateDto): Observable<ApiResponse<TaxRate>> {
    return this.http.post<ApiResponse<TaxRate>>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateTaxRateDto): Observable<ApiResponse<TaxRate>> {
    return this.http.put<ApiResponse<TaxRate>>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`);
  }
}
