import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { FormsModule } from '@angular/forms';
import { TaxRateService } from '../../services/taxrate.service';
import { TaxRateDto } from '../../models/taxrate.model';

@Component({
  selector: 'app-taxrate-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzTableModule,
    NzButtonModule,
    NzIconModule,
    NzTagModule,
    NzModalModule,
    NzSelectModule
  ],
  templateUrl: './taxrate-list.component.html',
  styleUrls: ['./taxrate-list.component.scss']
})
export class TaxRateListComponent implements OnInit {
  taxRates: TaxRateDto[] = [];
  filteredTaxRates: TaxRateDto[] = [];
  loading = false;
  filterTaxType: string | null = null;

  taxTypes = [
    { label: 'PPN', value: 'PPN' },
    { label: 'PPH21', value: 'PPH21' },
    { label: 'PPH22', value: 'PPH22' },
    { label: 'PPH23', value: 'PPH23' }
  ];

  constructor(
    private taxRateService: TaxRateService,
    private modal: NzModalService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadTaxRates();
  }

  loadTaxRates(): void {
    this.loading = true;
    this.taxRateService.getAll().subscribe({
      next: (data) => {
        this.taxRates = data;
        this.applyFilter();
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading tax rates:', err);
        this.message.error('Gagal memuat data tarif pajak');
        this.loading = false;
      }
    });
  }

  applyFilter(): void {
    if (this.filterTaxType) {
      this.filteredTaxRates = this.taxRates.filter(
        tr => tr.taxType === this.filterTaxType
      );
    } else {
      this.filteredTaxRates = [...this.taxRates];
    }
  }

  onFilterChange(): void {
    this.applyFilter();
  }

  clearFilter(): void {
    this.filterTaxType = null;
    this.applyFilter();
  }

  showCreateModal(): void {
    // TODO: Implement create modal
    this.message.info('Fitur tambah data akan segera hadir');
  }

  showEditModal(taxRate: TaxRateDto): void {
    // TODO: Implement edit modal
    this.message.info('Fitur edit data akan segera hadir');
  }

  deleteTaxRate(id: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: 'Apakah Anda yakin ingin menghapus tarif pajak ini?',
      nzOkText: 'Hapus',
      nzOkDanger: true,
      nzCancelText: 'Batal',
      nzOnOk: () => {
        this.loading = true;
        this.taxRateService.delete(id).subscribe({
          next: () => {
            this.message.success('Tarif pajak berhasil dihapus');
            this.loadTaxRates();
          },
          error: (err) => {
            console.error('Error deleting tax rate:', err);
            this.message.error('Gagal menghapus tarif pajak');
            this.loading = false;
          }
        });
      }
    });
  }

  getTaxTypeColor(taxType: string): string {
    const colors: { [key: string]: string } = {
      'PPN': 'blue',
      'PPH21': 'green',
      'PPH22': 'orange',
      'PPH23': 'purple'
    };
    return colors[taxType] || 'default';
  }
}
