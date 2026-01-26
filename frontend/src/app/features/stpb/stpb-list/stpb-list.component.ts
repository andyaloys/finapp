import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { FormsModule } from '@angular/forms';
import { StpbService } from '../../../core/services/stpb.service';
import { Stpb } from '../../../core/models/stpb.model';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-stpb-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzTableModule,
    NzButtonModule,
    NzIconModule,
    NzModalModule,
    NzInputModule,
    NzTagModule,
    NzCardModule,
    NzToolTipModule,
    PageHeaderComponent
  ],
  templateUrl: './stpb-list.component.html',
  styleUrls: ['./stpb-list.component.scss']
})
export class StpbListComponent implements OnInit {
  stpbs: Stpb[] = [];
  loading = false;
  pageIndex = 1;
  pageSize = 10;
  total = 0;
  searchTerm = '';
  pdfModalVisible = false;
  pdfUrl: SafeResourceUrl | null = null;

  constructor(
    private stpbService: StpbService,
    private router: Router,
    private message: NzMessageService,
    private modal: NzModalService,
    private http: HttpClient,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.loadStpbs();
  }

  loadStpbs(): void {
    this.loading = true;
    this.stpbService.getAll(this.pageIndex, this.pageSize, this.searchTerm).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.stpbs = response.data.items;
          this.total = response.data.totalCount;
        }
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.message.error('Gagal memuat data STPB');
      }
    });
  }

  onPageChange(pageIndex: number): void {
    this.pageIndex = pageIndex;
    this.loadStpbs();
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 1;
    this.loadStpbs();
  }

  onSearch(): void {
    this.pageIndex = 1;
    this.loadStpbs();
  }

  createNew(): void {
    this.router.navigate(['/stpb/create']);
  }

  edit(id: string): void {
    this.router.navigate(['/stpb/edit', id]);
  }

  delete(id: string, nomor: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: `Apakah Anda yakin ingin menghapus STPB ${nomor}?`,
      nzOnOk: () => {
        this.stpbService.delete(id).subscribe({
          next: () => {
            this.message.success('STPB berhasil dihapus');
            this.loadStpbs();
          },
          error: () => {
            this.message.error('Gagal menghapus STPB');
          }
        });
      }
    });
  }
printPdf(id: string): void {
    const url = `${environment.apiUrl}/Stpb/${id}/pdf`;
    const token = localStorage.getItem('token');
    
    this.http.get(url, {
      headers: {
        'Authorization': `Bearer ${token}`
      },
      responseType: 'blob'
    }).subscribe({
      next: (blob) => {
        const blobUrl = window.URL.createObjectURL(blob);
        this.pdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(blobUrl);
        this.pdfModalVisible = true;
      },
      error: () => {
        this.message.error('Gagal membuka PDF');
      }
    });
  }

  closePdfModal(): void {
    this.pdfModalVisible = false;
    this.pdfUrl = null;
  }

  
  getStatusColor(status: string): string {
    const colors: Record<string, string> = {
      'Aktif': 'success',
      'Nonaktif': 'error',
      'Draft': 'default',
      'Submitted': 'processing',
      'Approved': 'success',
      'Rejected': 'error'
    };
    return colors[status] || 'default';
  }
}
