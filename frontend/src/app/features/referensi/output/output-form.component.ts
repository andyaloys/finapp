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
import { OutputService } from '../../../core/services/output.service';
import { KegiatanService } from '../../../core/services/kegiatan.service';
import { KegiatanDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-output-form',
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
  templateUrl: './output-form.component.html',
  styleUrls: ['./output-form.component.scss']
})
export class OutputFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  outputId: string | null = null;
  loading = false;
  submitting = false;
  kegiatans: KegiatanDto[] = [];

  constructor(
    private fb: FormBuilder,
    private outputService: OutputService,
    private kegiatanService: KegiatanService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.outputId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.outputId;

    this.loadKegiatans();

    if (this.isEditMode && this.outputId) {
      this.loadOutput(this.outputId);
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      kode: ['', [Validators.required, Validators.maxLength(20)]],
      nama: ['', [Validators.required, Validators.maxLength(500)]],
      kegiatanId: ['', [Validators.required]],
      isActive: [true]
    });
  }

  loadKegiatans(): void {
    this.kegiatanService.getAll(1, 1000).subscribe({
      next: (response) => {
        this.kegiatans = response.data.items.filter((k: KegiatanDto) => k.isActive);
      },
      error: () => {
        this.message.error('Gagal memuat data kegiatan');
      }
    });
  }

  loadOutput(id: string): void {
    this.loading = true;
    this.outputService.getById(id).subscribe({
      next: (output) => {
        this.form.patchValue(output);
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data output');
        this.loading = false;
        this.onCancel();
      }
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitting = true;
      const formValue = this.form.value;

      const request = this.isEditMode && this.outputId
        ? this.outputService.update(this.outputId, formValue)
        : this.outputService.create(formValue);

      request.subscribe({
        next: () => {
          this.message.success(`Output berhasil ${this.isEditMode ? 'diperbarui' : 'ditambahkan'}`);
          this.router.navigate(['/referensi/output']);
        },
        error: () => {
          this.message.error(`Gagal ${this.isEditMode ? 'memperbarui' : 'menambahkan'} output`);
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
    this.router.navigate(['/referensi/output']);
  }
}
