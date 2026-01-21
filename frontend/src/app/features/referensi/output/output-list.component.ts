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
import { OutputService } from '../../../core/services/output.service';
import { OutputDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-output-list',
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
  templateUrl: './output-list.component.html',
  styleUrls: ['./output-list.component.scss']
})
export class OutputListComponent implements OnInit {
  outputs: OutputDto[] = [];
  loading = false;
  searchTerm = '';
  pageIndex = 1;
  pageSize = 10;
  total = 0;

  constructor(
    private outputService: OutputService,
    private router: Router,
    private modal: NzModalService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadOutputs();
  }

  loadOutputs(): void {
    this.loading = true;
    this.outputService.getAll(this.pageIndex, this.pageSize, this.searchTerm).subscribe({
      next: (response) => {
        this.outputs = response.data.items;
        this.total = response.data.totalCount;
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data output');
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 1;
    this.loadOutputs();
  }

  onPageChange(pageIndex: number): void {
    this.pageIndex = pageIndex;
    this.loadOutputs();
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 1;
    this.loadOutputs();
  }

  onCreate(): void {
    this.router.navigate(['/referensi/output/create']);
  }

  onEdit(id: string): void {
    this.router.navigate(['/referensi/output/edit', id]);
  }

  onDelete(id: string, nama: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: `Apakah Anda yakin ingin menghapus output "${nama}"?`,
      nzOkText: 'Hapus',
      nzOkDanger: true,
      nzOnOk: () => {
        this.outputService.delete(id).subscribe({
          next: () => {
            this.message.success('Output berhasil dihapus');
            this.loadOutputs();
          },
          error: () => {
            this.message.error('Gagal menghapus output');
          }
        });
      }
    });
  }
}
