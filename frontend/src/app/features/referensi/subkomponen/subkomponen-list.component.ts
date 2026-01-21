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
import { SubkomponenService } from '../../../core/services/subkomponen.service';
import { SubkomponenDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-subkomponen-list',
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
  templateUrl: './subkomponen-list.component.html',
  styleUrls: ['./subkomponen-list.component.scss']
})
export class SubkomponenListComponent implements OnInit {
  subkomponens: SubkomponenDto[] = [];
  loading = false;
  searchTerm = '';
  pageIndex = 1;
  pageSize = 10;
  total = 0;

  constructor(
    private subkomponenService: SubkomponenService,
    private router: Router,
    private modal: NzModalService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadSubkomponens();
  }

  loadSubkomponens(): void {
    this.loading = true;
    this.subkomponenService.getAll(this.pageIndex, this.pageSize, this.searchTerm).subscribe({
      next: (response) => {
        this.subkomponens = response.data.items;
        this.total = response.data.totalCount;
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data subkomponen');
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 1;
    this.loadSubkomponens();
  }

  onPageChange(pageIndex: number): void {
    this.pageIndex = pageIndex;
    this.loadSubkomponens();
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 1;
    this.loadSubkomponens();
  }

  onCreate(): void {
    this.router.navigate(['/referensi/subkomponen/create']);
  }

  onEdit(id: string): void {
    this.router.navigate(['/referensi/subkomponen/edit', id]);
  }

  onDelete(id: string, nama: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: `Apakah Anda yakin ingin menghapus subkomponen "${nama}"?`,
      nzOkText: 'Hapus',
      nzOkDanger: true,
      nzOnOk: () => {
        this.subkomponenService.delete(id).subscribe({
          next: () => {
            this.message.success('Subkomponen berhasil dihapus');
            this.loadSubkomponens();
          },
          error: () => {
            this.message.error('Gagal menghapus subkomponen');
          }
        });
      }
    });
  }
}
