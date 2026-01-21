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
import { KomponenService } from '../../../core/services/komponen.service';
import { KomponenDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-komponen-list',
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
  templateUrl: './komponen-list.component.html',
  styleUrls: ['./komponen-list.component.scss']
})
export class KomponenListComponent implements OnInit {
  komponens: KomponenDto[] = [];
  loading = false;
  searchTerm = '';
  pageIndex = 1;
  pageSize = 10;
  total = 0;

  constructor(
    private komponenService: KomponenService,
    private router: Router,
    private modal: NzModalService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadKomponens();
  }

  loadKomponens(): void {
    this.loading = true;
    this.komponenService.getAll(this.pageIndex, this.pageSize, this.searchTerm).subscribe({
      next: (response) => {
        this.komponens = response.data.items;
        this.total = response.data.totalCount;
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data komponen');
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 1;
    this.loadKomponens();
  }

  onPageChange(pageIndex: number): void {
    this.pageIndex = pageIndex;
    this.loadKomponens();
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 1;
    this.loadKomponens();
  }

  onCreate(): void {
    this.router.navigate(['/referensi/komponen/create']);
  }

  onEdit(id: string): void {
    this.router.navigate(['/referensi/komponen/edit', id]);
  }

  onDelete(id: string, nama: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: `Apakah Anda yakin ingin menghapus komponen "${nama}"?`,
      nzOkText: 'Hapus',
      nzOkDanger: true,
      nzOnOk: () => {
        this.komponenService.delete(id).subscribe({
          next: () => {
            this.message.success('Komponen berhasil dihapus');
            this.loadKomponens();
          },
          error: () => {
            this.message.error('Gagal menghapus komponen');
          }
        });
      }
    });
  }
}
