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
import { KegiatanService } from '../../../core/services/kegiatan.service';
import { ProgramService } from '../../../core/services/program.service';
import { ProgramDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-kegiatan-form',
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
  templateUrl: './kegiatan-form.component.html',
  styleUrls: ['./kegiatan-form.component.scss']
})
export class KegiatanFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  kegiatanId: string | null = null;
  loading = false;
  submitting = false;
  programs: ProgramDto[] = [];

  constructor(
    private fb: FormBuilder,
    private kegiatanService: KegiatanService,
    private programService: ProgramService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.kegiatanId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.kegiatanId;

    this.loadPrograms();

    if (this.isEditMode && this.kegiatanId) {
      this.loadKegiatan(this.kegiatanId);
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      kode: ['', [Validators.required, Validators.maxLength(20)]],
      nama: ['', [Validators.required, Validators.maxLength(500)]],
      programId: ['', [Validators.required]],
      isActive: [true]
    });
  }

  loadPrograms(): void {
    this.programService.getAll(1, 1000).subscribe({
      next: (response) => {
        this.programs = response.data.items.filter((p: ProgramDto) => p.isActive);
      },
      error: () => {
        this.message.error('Gagal memuat data program');
      }
    });
  }

  loadKegiatan(id: string): void {
    this.loading = true;
    this.kegiatanService.getById(id).subscribe({
      next: (kegiatan) => {
        this.form.patchValue(kegiatan);
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data kegiatan');
        this.loading = false;
        this.onCancel();
      }
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitting = true;
      const formValue = this.form.value;

      const request = this.isEditMode && this.kegiatanId
        ? this.kegiatanService.update(this.kegiatanId, formValue)
        : this.kegiatanService.create(formValue);

      request.subscribe({
        next: () => {
          this.message.success(`Kegiatan berhasil ${this.isEditMode ? 'diperbarui' : 'ditambahkan'}`);
          this.router.navigate(['/referensi/kegiatan']);
        },
        error: () => {
          this.message.error(`Gagal ${this.isEditMode ? 'memperbarui' : 'menambahkan'} kegiatan`);
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
    this.router.navigate(['/referensi/kegiatan']);
  }
}
