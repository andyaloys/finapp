import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AnggaranMasterService {
  private apiUrl = `${environment.apiUrl}/anggaranmasterquery`;
  private uploadApiUrl = `${environment.apiUrl}/AnggaranMaster`;

  constructor(private http: HttpClient) {}

  getAnggaranSummary(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/summary`);
  }

  getAnggaranDetail(tahun: number, revisi: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/detail?tahun=${tahun}&revisi=${revisi}`);
  }

  getDistinctTahun(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-tahun`);
  }

  getDistinctRevisi(tahun: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-revisi?tahunAnggaran=${tahun}`);
  }

  getDistinctSuboutputs(tahun: number, revisi: number, kdProgram: string, kdGiat: string, kdOutput: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-suboutputs?tahun=${tahun}&revisi=${revisi}&kdProgram=${kdProgram}&kdGiat=${kdGiat}&kdOutput=${kdOutput}`);
  }

  getAllSuboutputs(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/all-suboutputs`);
  }

  getDistinctKomponens(tahun: number, revisi: number, kdProgram: string, kdGiat: string, kdOutput: string, kdSOutput: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-komponens?tahun=${tahun}&revisi=${revisi}&kdProgram=${kdProgram}&kdGiat=${kdGiat}&kdOutput=${kdOutput}&kdSOutput=${kdSOutput}`);
  }

  getDistinctSubkomponens(tahun: number, revisi: number, kdProgram: string, kdGiat: string, kdOutput: string, kdSOutput: string, kdKmpnen: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-subkomponens?tahun=${tahun}&revisi=${revisi}&kdProgram=${kdProgram}&kdGiat=${kdGiat}&kdOutput=${kdOutput}&kdSOutput=${kdSOutput}&kdKmpnen=${kdKmpnen}`);
  }

  getDistinctAkuns(tahun: number, revisi: number, kdProgram: string, kdGiat: string, kdOutput: string, kdSOutput: string, kdKmpnen: string, kdSkmpnen: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-akuns?tahun=${tahun}&revisi=${revisi}&kdProgram=${kdProgram}&kdGiat=${kdGiat}&kdOutput=${kdOutput}&kdSOutput=${kdSOutput}&kdKmpnen=${kdKmpnen}&kdSkmpnen=${kdSkmpnen}`);
  }

  getDistinctItems(tahun: number, revisi: number, kdProgram: string, kdGiat: string, kdOutput: string, kdSOutput: string, kdKmpnen: string, kdSkmpnen: string, kdAkun: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-items?tahun=${tahun}&revisi=${revisi}&kdProgram=${kdProgram}&kdGiat=${kdGiat}&kdOutput=${kdOutput}&kdSOutput=${kdSOutput}&kdKmpnen=${kdKmpnen}&kdSkmpnen=${kdSkmpnen}&kdAkun=${kdAkun}`);
  }

  checkPagu(tahun: number, revisi: number, kdProgram: string, kdGiat: string, kdOutput: string, kdSOutput: string, kdKmpnen: string, kdSkmpnen: string, kdAkun: string, noItem: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/check-pagu?tahun=${tahun}&revisi=${revisi}&kdProgram=${kdProgram}&kdGiat=${kdGiat}&kdOutput=${kdOutput}&kdSOutput=${kdSOutput}&kdKmpnen=${kdKmpnen}&kdSkmpnen=${kdSkmpnen}&kdAkun=${kdAkun}&noItem=${noItem}`);
  }

  getDistinctKegiatans(tahun: number, revisi: number, kdProgram: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-kegiatans?tahun=${tahun}&revisi=${revisi}&kdProgram=${kdProgram}`);
  }

  getDistinctOutputs(tahun: number, revisi: number, kdProgram: string, kdGiat: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-outputs?tahun=${tahun}&revisi=${revisi}&kdProgram=${kdProgram}&kdGiat=${kdGiat}`);
  }

  getItems(tahun: number, revisi: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/items?tahun=${tahun}&revisi=${revisi}`);
  }

  getDistinctPrograms(tahun: number, revisi: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/distinct-programs?tahun=${tahun}&revisi=${revisi}`);
  }

  uploadAnggaran(file: File, tahunAnggaran: number): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('tahunAnggaran', tahunAnggaran.toString());
    return this.http.post(`${this.uploadApiUrl}/upload`, formData);
  }

  // Tambahkan method untuk kegiatan, output, dst jika perlu cascading
}
