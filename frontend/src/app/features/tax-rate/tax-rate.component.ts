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
import { NzSelectModule } from 'ng-zorro-antd/select';
import { TaxRateService } from '../../services/taxrate.service';
import { TaxRateDto as TaxRate, CreateTaxRateDto, UpdateTaxRateDto } from '../../models/taxrate.model';

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
    NzTagModule,
    NzSelectModule
  ],
  templateUrl: './tax-rate.component.html',
  styleUrls: ['./tax-rate.component.scss']
})
export class TaxRateComponent implements OnInit {
  taxRates: TaxRate[] = [];
  isLoading = false;
  isModalVisible = false;
  isEditMode = false;
  currentTaxRateId?: string;
  taxRateForm!: FormGroup;

  taxTypes = [
    { label: 'PPN', value: 'PPN' },
    { label: 'PPH21', value: 'PPH21' },
    { label: 'PPH22', value: 'PPH22' },
    { label: 'PPH23', value: 'PPH23' }
  ];

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
      taxType: ['', [Validators.required]],
      category: ['', [Validators.required, Validators.maxLength(100)]],
      rate: [0, [Validators.required, Validators.min(0.01), Validators.max(100)]],
      description: ['', [Validators.maxLength(500)]],
      referenceCode: ['', [Validators.maxLength(50)]],
      isDefault: [false],
      isActive: [true],
      displayOrder: [1, [Validators.required, Validators.min(1)]]
    });
  }

  loadTaxRates(): void {
    this.isLoading = true;
    this.taxRateService.getAll().subscribe({
      next: (data) => {
        this.taxRates = data;
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
      taxType: '',
      category: '',
      rate: 0,
      description: '',
      referenceCode: '',
      isDefault: false,
      isActive: true,
      displayOrder: 1
    });
    this.taxRateForm.get('taxType')?.enable();
    this.isModalVisible = true;
  }

  showEditModal(taxRate: TaxRate): void {
    this.isEditMode = true;
    this.currentTaxRateId = taxRate.id;
    this.taxRateForm.patchValue({
      taxType: taxRate.taxType,
      category: taxRate.category,
      rate: taxRate.rate,
      description: taxRate.description || '',
      referenceCode: taxRate.referenceCode || '',
      isDefault: taxRate.isDefault,
      isActive: taxRate.isActive,
      displayOrder: taxRate.displayOrder
    });
    this.taxRateForm.get('taxType')?.disable();
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
      taxType: this.taxRateForm.value.taxType,
      category: this.taxRateForm.value.category,
      rate: this.taxRateForm.value.rate,
      description: this.taxRateForm.value.description,
      referenceCode: this.taxRateForm.value.referenceCode,
      isDefault: this.taxRateForm.value.isDefault,
      isActive: this.taxRateForm.value.isActive,
      displayOrder: this.taxRateForm.value.displayOrder
    };

    this.taxRateService.create(dto).subscribe({
      next: () => {
        this.message.success('Tarif pajak berhasil ditambahkan');
        this.isModalVisible = false;
        this.loadTaxRates();
      },
      error: (error) => {
        this.message.error(error.error?.message || 'Gagal menambahkan tarif pajak');
      }
    });
  }

  updateTaxRate(): void {
    const dto: UpdateTaxRateDto = {
      category: this.taxRateForm.value.category,
      rate: this.taxRateForm.value.rate,
      description: this.taxRateForm.value.description,
      referenceCode: this.taxRateForm.value.referenceCode,
      isDefault: this.taxRateForm.value.isDefault,
      isActive: this.taxRateForm.value.isActive,
      displayOrder: this.taxRateForm.value.displayOrder
    };

    this.taxRateService.update(this.currentTaxRateId!, dto).subscribe({
      next: () => {
        this.message.success('Tarif pajak berhasil diupdate');
        this.isModalVisible = false;
        this.loadTaxRates();
      },
      error: (error) => {
        this.message.error(error.error?.message || 'Gagal mengupdate tarif pajak');
      }
    });
  }

  deleteTaxRate(id: string): void {
    this.taxRateService.delete(id).subscribe({
      next: () => {
        this.message.success('Tarif pajak berhasil dihapus');
        this.loadTaxRates();
      },
      error: (error: any) => {
        this.message.error(error.error?.message || 'Gagal menghapus tarif pajak');
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

  handleCancel(): void {
    this.isModalVisible = false;
    this.taxRateForm.reset();
  }
}
