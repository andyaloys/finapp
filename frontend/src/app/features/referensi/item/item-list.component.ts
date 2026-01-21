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
import { ItemService } from '../../../core/services/item.service';
import { ItemDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-item-list',
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
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.scss']
})
export class ItemListComponent implements OnInit {
  items: ItemDto[] = [];
  loading = false;
  searchTerm = '';
  pageIndex = 1;
  pageSize = 10;
  total = 0;

  constructor(
    private itemService: ItemService,
    private router: Router,
    private modal: NzModalService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.loading = true;
    this.itemService.getAll(this.pageIndex, this.pageSize, this.searchTerm).subscribe({
      next: (response) => {
        this.items = response.data.items;
        this.total = response.data.totalCount;
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data item');
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 1;
    this.loadItems();
  }

  onPageChange(pageIndex: number): void {
    this.pageIndex = pageIndex;
    this.loadItems();
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 1;
    this.loadItems();
  }

  onCreate(): void {
    this.router.navigate(['/referensi/item/create']);
  }

  onEdit(id: string): void {
    this.router.navigate(['/referensi/item/edit', id]);
  }

  onDelete(id: string, nama: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: `Apakah Anda yakin ingin menghapus item "${nama}"?`,
      nzOkText: 'Hapus',
      nzOkDanger: true,
      nzOnOk: () => {
        this.itemService.delete(id).subscribe({
          next: () => {
            this.message.success('Item berhasil dihapus');
            this.loadItems();
          },
          error: () => {
            this.message.error('Gagal menghapus item');
          }
        });
      }
    });
  }
}
