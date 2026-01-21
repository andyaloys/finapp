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
import { AkunService } from '../../../core/services/akun.service';
import { SubkomponenService } from '../../../core/services/subkomponen.service';
import { SubkomponenDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-akun-form',
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
  templateUrl: './akun-form.component.html',
  styleUrls: ['./akun-form.component.scss']
})
export class AkunFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  akunId: string | null = null;
  loading = false;
  submitting = false;
  subkomponens: SubkomponenDto[] = [];

  constructor(
    private fb: FormBuilder,
    private akunService: AkunService,
    private subkomponenService: SubkomponenService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.akunId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.akunId;

    this.loadSubkomponens();

    if (this.isEditMode && this.akunId) {
      this.loadAkun(this.akunId);
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      kode: ['', [Validators.required, Validators.maxLength(20)]],
      nama: ['', [Validators.required, Validators.maxLength(500)]],
      subkomponenId: ['', [Validators.required]],
      isActive: [true]
    });
  }

  loadSubkomponens(): void {
    this.subkomponenService.getAll(1, 1000).subscribe({
      next: (response) => {
        this.subkomponens = response.data.items.filter((s: SubkomponenDto) => s.isActive);
      },
      error: () => {
        this.message.error('Gagal memuat data subkomponen');
      }
    });
  }

  loadAkun(id: string): void {
    this.loading = true;
    this.akunService.getById(id).subscribe({
      next: (akun) => {
        this.form.patchValue(akun);
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data akun');
        this.loading = false;
        this.onCancel();
      }
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitting = true;
      const formValue = this.form.value;

      const request = this.isEditMode && this.akunId
        ? this.akunService.update(this.akunId, formValue)
        : this.akunService.create(formValue);

      request.subscribe({
        next: () => {
          this.message.success(`Akun berhasil ${this.isEditMode ? 'diperbarui' : 'ditambahkan'}`);
          this.router.navigate(['/referensi/akun']);
        },
        error: () => {
          this.message.error(`Gagal ${this.isEditMode ? 'memperbarui' : 'menambahkan'} akun`);
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
    this.router.navigate(['/referensi/akun']);
  }
}
