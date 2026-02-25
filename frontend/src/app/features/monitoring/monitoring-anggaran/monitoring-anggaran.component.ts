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
import * as ExcelJS from 'exceljs';

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
    this.loading = true;
    this.message.loading('Memuat detail transaksi...', { nzDuration: 0 });

    // Load all details for all filtered items
    const detailRequests = this.filteredData.map((item, index) => 
      this.monitoringService.getStpbDetails(item).toPromise()
        .then(response => ({ index, item, details: response?.data || [] }))
        .catch(() => ({ index, item, details: [] }))
    );

    Promise.all(detailRequests).then(results => {
      try {
        this.message.remove();

        // Create workbook using ExcelJS
        const workbook = new ExcelJS.Workbook();
        const worksheet = workbook.addWorksheet(`Monitoring ${this.selectedTahun}`);

        // Define columns
        worksheet.columns = [
          { header: 'No', key: 'no', width: 8 },
          { header: 'Kode Program', key: 'kodeProgram', width: 12 },
          { header: 'Kode Kegiatan', key: 'kodeKegiatan', width: 12 },
          { header: 'Kode Output', key: 'kodeOutput', width: 12 },
          { header: 'Kode Suboutput', key: 'kodeSuboutput', width: 12 },
          { header: 'Kode Komponen', key: 'kodeKomponen', width: 12 },
          { header: 'Kode Subkomponen', key: 'kodeSubkomponen', width: 15 },
          { header: 'Kode Akun', key: 'kodeAkun', width: 12 },
          { header: 'No Item', key: 'noItem', width: 10 },
          { header: 'Nama Item', key: 'namaItem', width: 35 },
          { header: 'Nama Akun', key: 'namaAkun', width: 40 },
          { header: 'Program', key: 'program', width: 30 },
          { header: 'Kegiatan', key: 'kegiatan', width: 20 },
          { header: 'Output', key: 'output', width: 18 },
          { header: 'Suboutput', key: 'suboutput', width: 20 },
          { header: 'Komponen', key: 'komponen', width: 18 },
          { header: 'Subkomponen', key: 'subkomponen', width: 25 },
          { header: 'Pagu Anggaran', key: 'paguAnggaran', width: 18 },
          { header: 'Realisasi', key: 'realisasi', width: 18 },
          { header: 'Sisa Anggaran', key: 'sisaAnggaran', width: 18 },
          { header: 'Persentase Realisasi (%)', key: 'persenRealisasi', width: 20 }
        ];

        // Style header row
        worksheet.getRow(1).font = { bold: true };
        worksheet.getRow(1).fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'FFE0E0E0' }
        };

        let rowNumber = 1;
        let totalDetails = 0;

        // Add data rows
        results.forEach(result => {
          const item = result.item;
          
          // Parse COA string to extract individual codes
          const coaParts = item.coa.split('.');
          
          // Add main anggaran row
          const anggaranRow = worksheet.addRow({
            no: rowNumber++,
            kodeProgram: coaParts[0] || '',
            kodeKegiatan: coaParts[1] || '',
            kodeOutput: coaParts[2] || '',
            kodeSuboutput: coaParts[3] || '',
            kodeKomponen: coaParts[4] || '',
            kodeSubkomponen: coaParts[5] || '',
            kodeAkun: coaParts[6] || '',
            noItem: coaParts[7] || '',
            namaItem: item.namaItem,
            namaAkun: item.namaAkun,
            program: item.namaProgram,
            kegiatan: item.namaKegiatan,
            output: item.namaOutput,
            suboutput: item.namaSuboutput,
            komponen: item.namaKomponen,
            subkomponen: item.namaSubkomponen,
            paguAnggaran: item.paguAnggaran,
            realisasi: item.realisasi,
            sisaAnggaran: item.sisaAnggaran,
            persenRealisasi: Number(item.persenRealisasi.toFixed(2))
          });

          // Format number cells
          anggaranRow.getCell('paguAnggaran').numFmt = '#,##0';
          anggaranRow.getCell('realisasi').numFmt = '#,##0';
          anggaranRow.getCell('sisaAnggaran').numFmt = '#,##0';
          anggaranRow.getCell('persenRealisasi').numFmt = '0.00';

          // Add detail rows below if any
          if (result.details && result.details.length > 0) {
            result.details.forEach((detail) => {
              totalDetails++;
              const detailRow = worksheet.addRow({
                no: '',
                kodeProgram: detail.noStpb,
                kodeKegiatan: new Date(detail.tanggalStpb).toLocaleDateString('id-ID'),
                kodeOutput: detail.keterangan,
                kodeSuboutput: detail.penerima || '-',
                kodeKomponen: detail.nilaiKotor,
                kodeSubkomponen: '',
                kodeAkun: '',
                noItem: '',
                namaItem: '',
                namaAkun: '',
                program: '',
                kegiatan: '',
                output: '',
                suboutput: '',
                komponen: '',
                subkomponen: '',
                paguAnggaran: '',
                realisasi: '',
                sisaAnggaran: '',
                persenRealisasi: ''
              });

              // Apply italic, gray, and thin style to detail rows
              detailRow.font = {
                italic: true,
                color: { argb: 'FF808080' }
              };

              // Format number cells for details
              detailRow.getCell('kodeKomponen').numFmt = '#,##0';
            });
          }
        });

        // Add summary row at the end
        const summaryRow = worksheet.addRow({
          no: '',
          kodeProgram: '',
          kodeKegiatan: '',
          kodeOutput: '',
          kodeSuboutput: '',
          kodeKomponen: '',
          kodeSubkomponen: '',
          kodeAkun: '',
          noItem: '',
          namaItem: '',
          namaAkun: '',
          program: '',
          kegiatan: '',
          output: '',
          suboutput: '',
          komponen: '',
          subkomponen: 'TOTAL',
          paguAnggaran: this.totalPagu,
          realisasi: this.totalRealisasi,
          sisaAnggaran: this.totalSisa,
          persenRealisasi: Number(this.persenRealisasiTotal.toFixed(2))
        });

        // Style summary row
        summaryRow.font = { bold: true };
        summaryRow.getCell('paguAnggaran').numFmt = '#,##0';
        summaryRow.getCell('realisasi').numFmt = '#,##0';
        summaryRow.getCell('sisaAnggaran').numFmt = '#,##0';
        summaryRow.getCell('persenRealisasi').numFmt = '0.00';

        // Generate filename
        const fileName = `Monitoring_Anggaran_${this.selectedTahun}_${new Date().toISOString().split('T')[0]}.xlsx`;

        // Save file
        workbook.xlsx.writeBuffer().then((buffer) => {
          const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          const url = window.URL.createObjectURL(blob);
          const anchor = document.createElement('a');
          anchor.href = url;
          anchor.download = fileName;
          anchor.click();
          window.URL.revokeObjectURL(url);

          this.loading = false;
          this.message.success(`File Excel berhasil diunduh dengan ${totalDetails} detail transaksi`);
        });
      } catch (error) {
        console.error('Error exporting to Excel:', error);
        this.loading = false;
        this.message.error('Gagal mengekspor data ke Excel');
      }
    }).catch(error => {
      console.error('Error loading details:', error);
      this.message.remove();
      this.loading = false;
      this.message.error('Gagal memuat detail transaksi');
    });
  }
}
