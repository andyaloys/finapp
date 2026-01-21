import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ItemService } from '../../../core/services/item.service';
import { AkunService } from '../../../core/services/akun.service';
import { AkunDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-item-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzInputNumberModule,
    NzButtonModule,
    NzSwitchModule,
    NzSelectModule
  ],
  templateUrl: './item-form.component.html',
  styleUrls: ['./item-form.component.scss']
})
export class ItemFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  itemId: string | null = null;
  loading = false;
  submitting = false;
  akuns: AkunDto[] = [];

  constructor(
    private fb: FormBuilder,
    private itemService: ItemService,
    private akunService: AkunService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.itemId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.itemId;

    this.loadAkuns();

    if (this.isEditMode && this.itemId) {
      this.loadItem(this.itemId);
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      nama: ['', [Validators.required, Validators.maxLength(500)]],
      satuan: ['', [Validators.required, Validators.maxLength(50)]],
      hargaSatuan: [0, [Validators.required, Validators.min(0)]],
      akunId: ['', [Validators.required]],
      isActive: [true]
    });
  }

  loadAkuns(): void {
    this.akunService.getAll(1, 1000).subscribe({
      next: (response) => {
        this.akuns = response.data.items.filter((a: AkunDto) => a.isActive);
      },
      error: () => {
        this.message.error('Gagal memuat data akun');
      }
    });
  }

  loadItem(id: string): void {
    this.loading = true;
    this.itemService.getById(id).subscribe({
      next: (item) => {
        this.form.patchValue(item);
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data item');
        this.loading = false;
        this.onCancel();
      }
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitting = true;
      const formValue = this.form.value;

      const request = this.isEditMode && this.itemId
        ? this.itemService.update(this.itemId, formValue)
        : this.itemService.create(formValue);

      request.subscribe({
        next: () => {
          this.message.success(`Item berhasil ${this.isEditMode ? 'diperbarui' : 'ditambahkan'}`);
          this.router.navigate(['/referensi/item']);
        },
        error: () => {
          this.message.error(`Gagal ${this.isEditMode ? 'memperbarui' : 'menambahkan'} item`);
          this.submitting = false;
        }
      });
    } else {
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/referensi/item']);
  }
}
