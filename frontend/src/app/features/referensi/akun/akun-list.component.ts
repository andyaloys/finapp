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
import { AkunService } from '../../../core/services/akun.service';
import { AkunDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-akun-list',
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
  templateUrl: './akun-list.component.html',
  styleUrls: ['./akun-list.component.scss']
})
export class AkunListComponent implements OnInit {
  akuns: AkunDto[] = [];
  loading = false;
  searchTerm = '';
  pageIndex = 1;
  pageSize = 10;
  total = 0;

  constructor(
    private akunService: AkunService,
    private router: Router,
    private modal: NzModalService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadAkuns();
  }

  loadAkuns(): void {
    this.loading = true;
    this.akunService.getAll(this.pageIndex, this.pageSize, this.searchTerm).subscribe({
      next: (response) => {
        this.akuns = response.data.items;
        this.total = response.data.totalCount;
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data akun');
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 1;
    this.loadAkuns();
  }

  onPageChange(pageIndex: number): void {
    this.pageIndex = pageIndex;
    this.loadAkuns();
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 1;
    this.loadAkuns();
  }

  onCreate(): void {
    this.router.navigate(['/referensi/akun/create']);
  }

  onEdit(id: string): void {
    this.router.navigate(['/referensi/akun/edit', id]);
  }

  onDelete(id: string, nama: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: `Apakah Anda yakin ingin menghapus akun "${nama}"?`,
      nzOkText: 'Hapus',
      nzOkDanger: true,
      nzOnOk: () => {
        this.akunService.delete(id).subscribe({
          next: () => {
            this.message.success('Akun berhasil dihapus');
            this.loadAkuns();
          },
          error: () => {
            this.message.error('Gagal menghapus akun');
          }
        });
      }
    });
  }
}
