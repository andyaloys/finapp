import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

import { PpkBendaharaService } from '../../../core/services/ppk-bendahara.service';
import { PpkBendaharaDto, JabatanType } from '../../../core/models/ppk-bendahara.model';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-ppk-bendahara-list',
  standalone: true,
  imports: [
    CommonModule,
    NzTableModule,
    NzButtonModule,
    NzIconModule,
    NzTagModule,
    NzCardModule,
    NzToolTipModule,
    PageHeaderComponent
  ],
  templateUrl: './ppk-bendahara-list.component.html',
  styleUrls: ['./ppk-bendahara-list.component.scss']
})
export class PpkBendaharaListComponent implements OnInit {
  ppkBendaharaList: PpkBendaharaDto[] = [];
  loading = false;

  constructor(
    private ppkBendaharaService: PpkBendaharaService,
    private router: Router,
    private message: NzMessageService,
    private modal: NzModalService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.ppkBendaharaService.getAll().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.ppkBendaharaList = response.data;
        }
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data PPK/Bendahara');
        this.loading = false;
      }
    });
  }

  createNew(): void {
    this.router.navigate(['/ppkbendahara/create']);
  }

  edit(id: string): void {
    this.router.navigate(['/ppkbendahara/edit', id]);
  }

  delete(id: string, nama: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: `Apakah Anda yakin ingin menghapus ${nama}?`,
      nzOnOk: () => {
        this.ppkBendaharaService.delete(id).subscribe({
          next: () => {
            this.message.success('Data berhasil dihapus');
            this.loadData();
          },
          error: () => {
            this.message.error('Gagal menghapus data');
          }
        });
      }
    });
  }

  getJabatanDisplay(jabatan: JabatanType): string {
    return jabatan === JabatanType.PPK ? 'PPK' : 'Bendahara';
  }

  getJabatanColor(jabatan: JabatanType): string {
    return jabatan === JabatanType.PPK ? 'blue' : 'green';
  }
}
