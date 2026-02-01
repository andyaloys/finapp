import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { MonitoringAnggaran } from '../models/monitoring.model';

@Injectable({
  providedIn: 'root'
})
export class MonitoringService {
  private apiUrl = `${environment.apiUrl}/monitoring`;

  constructor(private http: HttpClient) {}

  getMonitoringAnggaran(tahun?: number): Observable<ApiResponse<MonitoringAnggaran[]>> {
    let params = new HttpParams();
    if (tahun) {
      params = params.set('tahun', tahun.toString());
    }
    return this.http.get<ApiResponse<MonitoringAnggaran[]>>(`${this.apiUrl}/anggaran`, { params });
  }
}
