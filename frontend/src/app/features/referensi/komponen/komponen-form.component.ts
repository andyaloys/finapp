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
import { KomponenService } from '../../../core/services/komponen.service';
import { SuboutputService } from '../../../core/services/suboutput.service';
import { SuboutputDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-komponen-form',
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
  templateUrl: './komponen-form.component.html',
  styleUrls: ['./komponen-form.component.scss']
})
export class KomponenFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  komponenId: string | null = null;
  loading = false;
  submitting = false;
  suboutputs: SuboutputDto[] = [];

  constructor(
    private fb: FormBuilder,
    private komponenService: KomponenService,
    private suboutputService: SuboutputService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.komponenId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.komponenId;

    this.loadSuboutputs();

    if (this.isEditMode && this.komponenId) {
      this.loadKomponen(this.komponenId);
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      kode: ['', [Validators.required, Validators.maxLength(20)]],
      nama: ['', [Validators.required, Validators.maxLength(500)]],
      suboutputId: ['', [Validators.required]],
      isActive: [true]
    });
  }

  loadSuboutputs(): void {
    this.suboutputService.getAll(1, 1000).subscribe({
      next: (response) => {
        this.suboutputs = response.data.items.filter((s: SuboutputDto) => s.isActive);
      },
      error: () => {
        this.message.error('Gagal memuat data suboutput');
      }
    });
  }

  loadKomponen(id: string): void {
    this.loading = true;
    this.komponenService.getById(id).subscribe({
      next: (komponen) => {
        this.form.patchValue(komponen);
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data komponen');
        this.loading = false;
        this.onCancel();
      }
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitting = true;
      const formValue = this.form.value;

      const request = this.isEditMode && this.komponenId
        ? this.komponenService.update(this.komponenId, formValue)
        : this.komponenService.create(formValue);

      request.subscribe({
        next: () => {
          this.message.success(`Komponen berhasil ${this.isEditMode ? 'diperbarui' : 'ditambahkan'}`);
          this.router.navigate(['/referensi/komponen']);
        },
        error: () => {
          this.message.error(`Gagal ${this.isEditMode ? 'memperbarui' : 'menambahkan'} komponen`);
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
    this.router.navigate(['/referensi/komponen']);
  }
}
