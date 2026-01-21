import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { FormsModule } from '@angular/forms';
import { KegiatanService } from '../../../core/services/kegiatan.service';
import { KegiatanDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-kegiatan-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzTableModule,
    NzButtonModule,
    NzInputModule,
    NzIconModule,
    NzModalModule,
    NzTagModule
  ],
  templateUrl: './kegiatan-list.component.html',
  styleUrls: ['./kegiatan-list.component.scss']
})
export class KegiatanListComponent implements OnInit {
  kegiatans: KegiatanDto[] = [];
  loading = false;
  searchTerm = '';
  pageIndex = 1;
  pageSize = 10;
  total = 0;

  constructor(
    private kegiatanService: KegiatanService,
    private router: Router,
    private modal: NzModalService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadKegiatans();
  }

  loadKegiatans(): void {
    this.loading = true;
    this.kegiatanService.getAll(this.pageIndex, this.pageSize, this.searchTerm).subscribe({
      next: (response) => {
        this.kegiatans = response.data.items;
        this.total = response.data.totalCount;
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data kegiatan');
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 1;
    this.loadKegiatans();
  }

  onPageChange(pageIndex: number): void {
    this.pageIndex = pageIndex;
    this.loadKegiatans();
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 1;
    this.loadKegiatans();
  }

  onCreate(): void {
    this.router.navigate(['/referensi/kegiatan/create']);
  }

  onEdit(id: string): void {
    this.router.navigate(['/referensi/kegiatan/edit', id]);
  }

  onDelete(id: string, nama: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: `Apakah Anda yakin ingin menghapus kegiatan "${nama}"?`,
      nzOkText: 'Hapus',
      nzOkDanger: true,
      nzOnOk: () => {
        this.kegiatanService.delete(id).subscribe({
          next: () => {
            this.message.success('Kegiatan berhasil dihapus');
            this.loadKegiatans();
          },
          error: () => {
            this.message.error('Gagal menghapus kegiatan');
          }
        });
      }
    });
  }
}
