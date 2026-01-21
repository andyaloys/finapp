import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ProgramService } from '../../../core/services/program.service';

@Component({
  selector: 'app-program-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzSwitchModule
  ],
  templateUrl: './program-form.component.html',
  styleUrls: ['./program-form.component.scss']
})
export class ProgramFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  programId: string | null = null;
  loading = false;
  submitting = false;

  constructor(
    private fb: FormBuilder,
    private programService: ProgramService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.programId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.programId;

    if (this.isEditMode && this.programId) {
      this.loadProgram(this.programId);
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      kode: ['', [Validators.required, Validators.maxLength(20)]],
      nama: ['', [Validators.required, Validators.maxLength(500)]],
      isActive: [true]
    });
  }

  loadProgram(id: string): void {
    this.loading = true;
    this.programService.getById(id).subscribe({
      next: (response) => {
        this.form.patchValue(response.data);
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data program');
        this.loading = false;
        this.onCancel();
      }
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitting = true;
      const formValue = this.form.value;

      const request = this.isEditMode && this.programId
        ? this.programService.update(this.programId, formValue)
        : this.programService.create(formValue);

      request.subscribe({
        next: () => {
          this.message.success(`Program berhasil ${this.isEditMode ? 'diperbarui' : 'ditambahkan'}`);
          this.router.navigate(['/referensi/program']);
        },
        error: () => {
          this.message.error(`Gagal ${this.isEditMode ? 'memperbarui' : 'menambahkan'} program`);
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
    this.router.navigate(['/referensi/program']);
  }
}
