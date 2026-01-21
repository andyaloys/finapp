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
import { SuboutputService } from '../../../core/services/suboutput.service';
import { OutputService } from '../../../core/services/output.service';
import { OutputDto } from '../../../core/models/referensi.model';

@Component({
  selector: 'app-suboutput-form',
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
  templateUrl: './suboutput-form.component.html',
  styleUrls: ['./suboutput-form.component.scss']
})
export class SuboutputFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  suboutputId: string | null = null;
  loading = false;
  submitting = false;
  outputs: OutputDto[] = [];

  constructor(
    private fb: FormBuilder,
    private suboutputService: SuboutputService,
    private outputService: OutputService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.suboutputId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.suboutputId;

    this.loadOutputs();

    if (this.isEditMode && this.suboutputId) {
      this.loadSuboutput(this.suboutputId);
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      kode: ['', [Validators.required, Validators.maxLength(20)]],
      nama: ['', [Validators.required, Validators.maxLength(500)]],
      outputId: ['', [Validators.required]],
      isActive: [true]
    });
  }

  loadOutputs(): void {
    this.outputService.getAll(1, 1000).subscribe({
      next: (response) => {
        this.outputs = response.data.items.filter((o: OutputDto) => o.isActive);
      },
      error: () => {
        this.message.error('Gagal memuat data output');
      }
    });
  }

  loadSuboutput(id: string): void {
    this.loading = true;
    this.suboutputService.getById(id).subscribe({
      next: (suboutput) => {
        this.form.patchValue(suboutput);
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data suboutput');
        this.loading = false;
        this.onCancel();
      }
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.submitting = true;
      const formValue = this.form.value;

      const request = this.isEditMode && this.suboutputId
        ? this.suboutputService.update(this.suboutputId, formValue)
        : this.suboutputService.create(formValue);

      request.subscribe({
        next: () => {
          this.message.success(`Suboutput berhasil ${this.isEditMode ? 'diperbarui' : 'ditambahkan'}`);
          this.router.navigate(['/referensi/suboutput']);
        },
        error: () => {
          this.message.error(`Gagal ${this.isEditMode ? 'memperbarui' : 'menambahkan'} suboutput`);
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
    this.router.navigate(['/referensi/suboutput']);
  }
}
