import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzMessageService } from 'ng-zorro-antd/message';
import { MonitoringService } from '../../../core/services/monitoring.service';
import { YearService } from '../../../core/services/year.service';
import { MonitoringAnggaran, StpbDetailMonitoring } from '../../../core/models/monitoring.model';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-monitoring-anggaran',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzTableModule,
    NzButtonModule,
    NzIconModule,
    NzInputModule,
    NzSelectModule,
    NzCardModule,
    NzStatisticModule,
    PageHeaderComponent
  ],
  templateUrl: './monitoring-anggaran.component.html',
  styleUrls: ['./monitoring-anggaran.component.scss']
})
export class MonitoringAnggaranComponent implements OnInit {
  data: MonitoringAnggaran[] = [];
  filteredData: MonitoringAnggaran[] = [];
  loading = false;
  searchTerm = '';
  selectedTahun: number = new Date().getFullYear();
  tahunOptions: number[] = [];
  
  // Summary statistics
  totalPagu = 0;
  totalRealisasi = 0;
  totalSisa = 0;
  persenRealisasiTotal = 0;

  // Expandable rows
  expandSet = new Set<number>();
  stpbDetailsMap = new Map<number, StpbDetailMonitoring[]>();
  loadingDetailsMap = new Map<number, boolean>();

  constructor(
    private monitoringService: MonitoringService,
    private yearService: YearService,
    private message: NzMessageService
  ) {
    this.selectedTahun = this.yearService.getSelectedYear();
    // Generate tahun options (current year and previous 2 years)
    const currentYear = new Date().getFullYear();
    for (let i = 0; i < 3; i++) {
      this.tahunOptions.push(currentYear - i);
    }
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.monitoringService.getMonitoringAnggaran(this.selectedTahun).subscribe({
      next: (response) => {
        this.data = response.data;
        this.filteredData = [...this.data];
        this.calculateSummary();
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data monitoring');
        this.loading = false;
      }
    });
  }

  onTahunChange(): void {
    this.loadData();
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase();
    if (!term) {
      this.filteredData = [...this.data];
    } else {
      this.filteredData = this.data.filter(item => 
        item.coa.toLowerCase().includes(term) ||
        item.namaItem.toLowerCase().includes(term) ||
        item.namaAkun.toLowerCase().includes(term) ||
        item.namaProgram.toLowerCase().includes(term)
      );
    }
    this.calculateSummary();
  }

  calculateSummary(): void {
    this.totalPagu = this.filteredData.reduce((sum, item) => sum + item.paguAnggaran, 0);
    this.totalRealisasi = this.filteredData.reduce((sum, item) => sum + item.realisasi, 0);
    this.totalSisa = this.filteredData.reduce((sum, item) => sum + item.sisaAnggaran, 0);
    this.persenRealisasiTotal = this.totalPagu > 0 ? (this.totalRealisasi / this.totalPagu) * 100 : 0;
  }

  onExpandChange(index: number, checked: boolean, item: MonitoringAnggaran): void {
    if (checked) {
      this.expandSet.add(index);
      if (!this.stpbDetailsMap.has(index)) {
        this.loadStpbDetails(index, item);
      }
    } else {
      this.expandSet.delete(index);
    }
  }

  loadStpbDetails(index: number, item: MonitoringAnggaran): void {
    this.loadingDetailsMap.set(index, true);
    this.monitoringService.getStpbDetails(item).subscribe({
      next: (response) => {
        this.stpbDetailsMap.set(index, response.data);
        this.loadingDetailsMap.set(index, false);
      },
      error: () => {
        this.message.error('Gagal memuat detail STPB');
        this.loadingDetailsMap.set(index, false);
      }
    });
  }

  getPercentageClass(persen: number): string {
    if (persen < 50) return 'success';
    if (persen < 80) return 'warning';
    return 'danger';
  }

  exportToExcel(): void {
    try {
      // Prepare data for export
      const exportData: any[] = this.filteredData.map((item, index) => ({
        'No': index + 1,
        'COA': item.coa,
        'Nama Item': item.namaItem,
        'Nama Akun': item.namaAkun,
        'Program': item.namaProgram,
        'Kegiatan': item.namaKegiatan,
        'Output': item.namaOutput,
        'Suboutput': item.namaSuboutput,
        'Komponen': item.namaKomponen,
        'Subkomponen': item.namaSubkomponen,
        'Pagu Anggaran': item.paguAnggaran,
        'Realisasi': item.realisasi,
        'Sisa Anggaran': item.sisaAnggaran,
        'Persentase Realisasi (%)': Number(item.persenRealisasi.toFixed(2))
      }));

      // Add summary row
      exportData.push({
        'No': '',
        'COA': '',
        'Nama Item': '',
        'Nama Akun': '',
        'Program': '',
        'Kegiatan': '',
        'Output': '',
        'Suboutput': '',
        'Komponen': '',
        'Subkomponen': 'TOTAL',
        'Pagu Anggaran': this.totalPagu,
        'Realisasi': this.totalRealisasi,
        'Sisa Anggaran': this.totalSisa,
        'Persentase Realisasi (%)': Number(this.persenRealisasiTotal.toFixed(2))
      });

      // Create worksheet
      const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);

      // Set column widths
      ws['!cols'] = [
        { wch: 5 },   // No
        { wch: 40 },  // COA
        { wch: 30 },  // Nama Item
        { wch: 25 },  // Nama Akun
        { wch: 30 },  // Program
        { wch: 30 },  // Kegiatan
        { wch: 30 },  // Output
        { wch: 30 },  // Suboutput
        { wch: 30 },  // Komponen
        { wch: 30 },  // Subkomponen
        { wch: 18 },  // Pagu Anggaran
        { wch: 18 },  // Realisasi
        { wch: 18 },  // Sisa Anggaran
        { wch: 20 }   // Persentase Realisasi
      ];

      // Create workbook
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, `Monitoring ${this.selectedTahun}`);

      // Generate filename
      const fileName = `Monitoring_Anggaran_${this.selectedTahun}_${new Date().toISOString().split('T')[0]}.xlsx`;

      // Save file
      XLSX.writeFile(wb, fileName);

      this.message.success('File Excel berhasil diunduh');
    } catch (error) {
      console.error('Error exporting to Excel:', error);
      this.message.error('Gagal mengekspor data ke Excel');
    }
  }
}
