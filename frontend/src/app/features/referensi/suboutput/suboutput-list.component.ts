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
import { SuboutputService } from '../../../core/services/suboutput.service';
import { SuboutputDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-suboutput-list',
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
  templateUrl: './suboutput-list.component.html',
  styleUrls: ['./suboutput-list.component.scss']
})
export class SuboutputListComponent implements OnInit {
  suboutputs: SuboutputDto[] = [];
  loading = false;
  searchTerm = '';
  pageIndex = 1;
  pageSize = 10;
  total = 0;

  constructor(
    private suboutputService: SuboutputService,
    private router: Router,
    private modal: NzModalService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadSuboutputs();
  }

  loadSuboutputs(): void {
    this.loading = true;
    this.suboutputService.getAll(this.pageIndex, this.pageSize, this.searchTerm).subscribe({
      next: (response) => {
        this.suboutputs = response.data.items;
        this.total = response.data.totalCount;
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data suboutput');
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 1;
    this.loadSuboutputs();
  }

  onPageChange(pageIndex: number): void {
    this.pageIndex = pageIndex;
    this.loadSuboutputs();
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 1;
    this.loadSuboutputs();
  }

  onCreate(): void {
    this.router.navigate(['/referensi/suboutput/create']);
  }

  onEdit(id: string): void {
    this.router.navigate(['/referensi/suboutput/edit', id]);
  }

  onDelete(id: string, nama: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: `Apakah Anda yakin ingin menghapus suboutput "${nama}"?`,
      nzOkText: 'Hapus',
      nzOkDanger: true,
      nzOnOk: () => {
        this.suboutputService.delete(id).subscribe({
          next: () => {
            this.message.success('Suboutput berhasil dihapus');
            this.loadSuboutputs();
          },
          error: () => {
            this.message.error('Gagal menghapus suboutput');
          }
        });
      }
    });
  }
}
