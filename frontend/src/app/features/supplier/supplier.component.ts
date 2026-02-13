import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { PenerimaService } from '../../core/services/penerima.service';
import { Penerima, CreatePenerimaDto, UpdatePenerimaDto } from '../../core/models/penerima.model';

@Component({
  selector: 'app-supplier',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzTableModule,
    NzButtonModule,
    NzIconModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzSwitchModule,
    NzPopconfirmModule,
    NzSpaceModule,
    NzTagModule
  ],
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss']
})
export class SupplierComponent implements OnInit {
  suppliers: Penerima[] = [];
  isLoading = false;
  isModalVisible = false;
  isEditMode = false;
  modalTitle = '';
  supplierForm!: FormGroup;
  currentSupplierId?: number;

  constructor(
    private penerimaService: PenerimaService,
    private fb: FormBuilder,
    private message: NzMessageService
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.loadSuppliers();
  }

  initForm(): void {
    this.supplierForm = this.fb.group({
      nama: ['', [Validators.required, Validators.maxLength(200)]],
      npwp: ['', Validators.maxLength(20)],
      alamat: ['', Validators.maxLength(500)],
      isActive: [true]
    });
  }

  loadSuppliers(): void {
    this.isLoading = true;
    this.penerimaService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.suppliers = response.data;
        }
        this.isLoading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data supplier');
        this.isLoading = false;
      }
    });
  }

  showAddModal(): void {
    this.isEditMode = false;
    this.modalTitle = 'Tambah Supplier';
    this.supplierForm.reset({ isActive: true });
    this.isModalVisible = true;
  }

  showEditModal(supplier: Penerima): void {
    this.isEditMode = true;
    this.modalTitle = 'Edit Supplier';
    this.currentSupplierId = supplier.id;
    this.supplierForm.patchValue({
      nama: supplier.nama,
      npwp: supplier.npwp,
      alamat: supplier.alamat,
      isActive: supplier.isActive
    });
    this.isModalVisible = true;
  }

  handleOk(): void {
    if (this.supplierForm.valid) {
      this.isLoading = true;
      
      if (this.isEditMode && this.currentSupplierId) {
        const dto: UpdatePenerimaDto = this.supplierForm.value;
        this.penerimaService.update(this.currentSupplierId, dto).subscribe({
          next: (response) => {
            if (response.success) {
              this.message.success('Supplier berhasil diupdate');
              this.loadSuppliers();
              this.handleCancel();
            }
            this.isLoading = false;
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal update supplier');
            this.isLoading = false;
          }
        });
      } else {
        const dto: CreatePenerimaDto = {
          nama: this.supplierForm.value.nama,
          npwp: this.supplierForm.value.npwp,
          alamat: this.supplierForm.value.alamat
        };
        this.penerimaService.create(dto).subscribe({
          next: (response) => {
            if (response.success) {
              this.message.success('Supplier berhasil ditambahkan');
              this.loadSuppliers();
              this.handleCancel();
            }
            this.isLoading = false;
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal menambah supplier');
            this.isLoading = false;
          }
        });
      }
    } else {
      Object.values(this.supplierForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  handleCancel(): void {
    this.isModalVisible = false;
    this.supplierForm.reset();
    this.currentSupplierId = undefined;
  }

  deleteSupplier(id: number): void {
    this.penerimaService.delete(id).subscribe({
      next: (response) => {
        if (response.success) {
          this.message.success('Supplier berhasil dihapus');
          this.loadSuppliers();
        }
      },
      error: (error) => {
        this.message.error(error.error?.message || 'Gagal menghapus supplier');
      }
    });
  }
}
