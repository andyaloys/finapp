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
import { AuthService } from '../../../core/services/auth.service';
import { Stpb } from '../../../core/models/stpb.model';
import { StpbStatus, getStatusClass, getStatusDisplay } from '../../../core/models/stpb-status.enum';
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
    private authService: AuthService,
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

  canEdit(stpb: Stpb): boolean {
    return stpb.status === StpbStatus.Draft || stpb.status === StpbStatus.Dikembalikan;
  }

  canApprove(stpb: Stpb): boolean {
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser) return false;
    
    const role = currentUser.role;
    // Only PPK, Bendahara, or Admin can approve
    return (role === 'PPK' || role === 'Bendahara' || role === 'Admin') && 
           stpb.status === StpbStatus.Kirim;
  }

  approve(id: string, nomor: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Approve',
      nzContent: `Apakah Anda yakin ingin meng-approve STPB ${nomor}?`,
      nzOnOk: () => {
        this.stpbService.approve(id).subscribe({
          next: () => {
            this.message.success('STPB berhasil di-approve');
            this.loadStpbs();
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal meng-approve STPB');
          }
        });
      }
    });
  }

  kembalikan(id: string, nomor: string): void {
    this.modal.confirm({
      nzTitle: 'Kembalikan STPB',
      nzContent: 'Masukkan alasan pengembalian:',
      nzOkText: 'Kembalikan',
      nzCancelText: 'Batal',
      nzOnOk: (instance: any) => {
        const alasan = instance.nzContent || 'Tidak ada alasan';
        this.stpbService.kembalikan(id, alasan).subscribe({
          next: () => {
            this.message.success('STPB berhasil dikembalikan');
            this.loadStpbs();
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal mengembalikan STPB');
          }
        });
      }
    });
  }

  getStatusClass(status: StpbStatus): string {
    return getStatusClass(status);
  }

  getStatusDisplay(status: StpbStatus): string {
    return getStatusDisplay(status);
  }
}
