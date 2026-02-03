import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { MonitoringAnggaran, StpbDetailMonitoring } from '../models/monitoring.model';

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

  getStpbDetails(anggaran: MonitoringAnggaran): Observable<ApiResponse<StpbDetailMonitoring[]>> {
    let params = new HttpParams()
      .set('kodeProgram', anggaran.kodeProgram)
      .set('kodeKegiatan', anggaran.kodeKegiatan)
      .set('kodeOutput', anggaran.kodeOutput)
      .set('kodeSuboutput', anggaran.kodeSuboutput)
      .set('kodeKomponen', anggaran.kodeKomponen)
      .set('kodeSubkomponen', anggaran.kodeSubkomponen)
      .set('kodeAkun', anggaran.kodeAkun)
      .set('noItem', anggaran.noItem)
      .set('tahun', anggaran.tahunAnggaran.toString());
    
    return this.http.get<ApiResponse<StpbDetailMonitoring[]>>(`${this.apiUrl}/stpb-details`, { params });
  }
}
