import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { TaxRateService } from '../../core/services/tax-rate.service';
import { TaxRate, CreateTaxRateDto, UpdateTaxRateDto } from '../../core/models/tax-rate.model';

@Component({
  selector: 'app-tax-rate',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzTableModule,
    NzButtonModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzInputNumberModule,
    NzSwitchModule,
    NzPopconfirmModule,
    NzIconModule,
    NzTagModule
  ],
  templateUrl: './tax-rate.component.html',
  styleUrls: ['./tax-rate.component.scss']
})
export class TaxRateComponent implements OnInit {
  taxRates: TaxRate[] = [];
  isLoading = false;
  isModalVisible = false;
  isEditMode = false;
  currentTaxRateId?: number;
  taxRateForm!: FormGroup;

  constructor(
    private taxRateService: TaxRateService,
    private fb: FormBuilder,
    private message: NzMessageService
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.loadTaxRates();
  }

  initForm(): void {
    this.taxRateForm = this.fb.group({
      taxCode: ['', [Validators.required, Validators.maxLength(20)]],
      taxName: ['', [Validators.required, Validators.maxLength(100)]],
      rate: [0, [Validators.required, Validators.min(0.01), Validators.max(100)]],
      isActive: [true]
    });
  }

  loadTaxRates(): void {
    this.isLoading = true;
    this.taxRateService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.taxRates = response.data;
        }
        this.isLoading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data tarif pajak');
        this.isLoading = false;
      }
    });
  }

  showAddModal(): void {
    this.isEditMode = false;
    this.currentTaxRateId = undefined;
    this.taxRateForm.reset({
      taxCode: '',
      taxName: '',
      rate: 0,
      isActive: true
    });
    this.taxRateForm.get('taxCode')?.enable();
    this.isModalVisible = true;
  }

  showEditModal(taxRate: TaxRate): void {
    this.isEditMode = true;
    this.currentTaxRateId = taxRate.id;
    this.taxRateForm.patchValue({
      taxCode: taxRate.taxCode,
      taxName: taxRate.taxName,
      rate: taxRate.rate,
      isActive: taxRate.isActive
    });
    this.taxRateForm.get('taxCode')?.disable();
    this.isModalVisible = true;
  }

  handleOk(): void {
    if (this.taxRateForm.valid) {
      if (this.isEditMode && this.currentTaxRateId) {
        this.updateTaxRate();
      } else {
        this.createTaxRate();
      }
    } else {
      Object.values(this.taxRateForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  createTaxRate(): void {
    const dto: CreateTaxRateDto = {
      taxCode: this.taxRateForm.value.taxCode,
      taxName: this.taxRateForm.value.taxName,
      rate: this.taxRateForm.value.rate
    };

    this.taxRateService.create(dto).subscribe({
      next: (response) => {
        if (response.success) {
          this.message.success('Tarif pajak berhasil ditambahkan');
          this.isModalVisible = false;
          this.loadTaxRates();
        } else {
          this.message.error(response.message || 'Gagal menambahkan tarif pajak');
        }
      },
      error: (error) => {
        this.message.error(error.error?.message || 'Gagal menambahkan tarif pajak');
      }
    });
  }

  updateTaxRate(): void {
    const dto: UpdateTaxRateDto = {
      taxName: this.taxRateForm.value.taxName,
      rate: this.taxRateForm.value.rate,
      isActive: this.taxRateForm.value.isActive
    };

    this.taxRateService.update(this.currentTaxRateId!, dto).subscribe({
      next: (response) => {
        if (response.success) {
          this.message.success('Tarif pajak berhasil diupdate');
          this.isModalVisible = false;
          this.loadTaxRates();
        } else {
          this.message.error(response.message || 'Gagal mengupdate tarif pajak');
        }
      },
      error: (error) => {
        this.message.error(error.error?.message || 'Gagal mengupdate tarif pajak');
      }
    });
  }

  deleteTaxRate(id: number): void {
    this.taxRateService.delete(id).subscribe({
      next: (response) => {
        if (response.success) {
          this.message.success('Tarif pajak berhasil dihapus');
          this.loadTaxRates();
        } else {
          this.message.error(response.message || 'Gagal menghapus tarif pajak');
        }
      },
      error: (error) => {
        this.message.error(error.error?.message || 'Gagal menghapus tarif pajak');
      }
    });
  }

  handleCancel(): void {
    this.isModalVisible = false;
  }
}
