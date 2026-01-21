import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzMessageService } from 'ng-zorro-antd/message';
import { SubkomponenService } from '../../../core/services/subkomponen.service';
import { KomponenService } from '../../../core/services/komponen.service';
import { KomponenDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-subkomponen-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzSwitchModule,
    NzSelectModule
  ],
  templateUrl: './subkomponen-form.component.html',
  styleUrls: ['./subkomponen-form.component.scss']
})
export class SubkomponenFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  subkomponenId: string | null = null;
  loading = false;
  submitting = false;
  komponens: KomponenDto[] = [];

  constructor(
    private fb: FormBuilder,
    private subkomponenService: SubkomponenService,
    private komponenService: KomponenService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.subkomponenId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.subkomponenId;

    this.loadKomponens();

    if (this.isEditMode && this.subkomponenId) {
      this.loadSubkomponen(this.subkomponenId);
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      kode: ['', [Validators.required, Validators.maxLength(20)]],
      nama: ['', [Validators.required, Validators.maxLength(500)]],
      komponenId: ['', [Validators.required]],
      isActive: [true]
    });
  }

  loadKomponens(): void {
    this.komponenService.getAll(1, 1000).subscribe({
      next: (response) => {
        this.komponens = response.data.items.filter((k: KomponenDto) => k.isActive);
      },
      error: () => {
        this.message.error('Gagal memuat data komponen');
      }
    });
  }

  loadSubkomponen(id: string): void {
    this.loading = true;
    this.subkomponenService.getById(id).subscribe({
      next: (subkomponen) => {
        this.form.patchValue(subkomponen);
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data subkomponen');
        this.loading = false;
        this.onCancel();
      }
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitting = true;
      const formValue = this.form.value;

      const request = this.isEditMode && this.subkomponenId
        ? this.subkomponenService.update(this.subkomponenId, formValue)
        : this.subkomponenService.create(formValue);

      request.subscribe({
        next: () => {
          this.message.success(`Subkomponen berhasil ${this.isEditMode ? 'diperbarui' : 'ditambahkan'}`);
          this.router.navigate(['/referensi/subkomponen']);
        },
        error: () => {
          this.message.error(`Gagal ${this.isEditMode ? 'memperbarui' : 'menambahkan'} subkomponen`);
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
    this.router.navigate(['/referensi/subkomponen']);
  }
}
