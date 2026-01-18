import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzMessageService } from 'ng-zorro-antd/message';
import { StpbService } from '../../../core/services/stpb.service';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-stpb-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzDatePickerModule,
    NzSelectModule,
    NzInputNumberModule,
    PageHeaderComponent
  ],
  templateUrl: './stpb-form.component.html',
  styleUrls: ['./stpb-form.component.scss']
})
export class StpbFormComponent implements OnInit {
  stpbForm: FormGroup;
  isLoading = false;
  isEditMode = false;
  stpbId: number | null = null;

  statusOptions = [
    { label: 'Draft', value: 'Draft' },
    { label: 'Submitted', value: 'Submitted' },
    { label: 'Approved', value: 'Approved' },
    { label: 'Rejected', value: 'Rejected' }
  ];

  constructor(
    private fb: FormBuilder,
    private stpbService: StpbService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.stpbForm = this.fb.group({
      nomorSTPB: ['', [Validators.required]],
      tanggal: [new Date(), [Validators.required]],
      deskripsi: [''],
      nilaiTotal: [0, [Validators.required, Validators.min(1)]],
      status: ['Draft', [Validators.required]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.stpbId = +id;
      this.loadStpb(this.stpbId);
    }
  }

  loadStpb(id: number): void {
    this.isLoading = true;
    this.stpbService.getById(id).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.stpbForm.patchValue({
            nomorSTPB: response.data.nomorSTPB,
            tanggal: new Date(response.data.tanggal),
            deskripsi: response.data.deskripsi,
            nilaiTotal: response.data.nilaiTotal,
            status: response.data.status
          });
        }
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.message.error('Gagal memuat data STPB');
        this.router.navigate(['/dashboard/stpb']);
      }
    });
  }

  onSubmit(): void {
    if (this.stpbForm.valid) {
      this.isLoading = true;
      const formData = this.stpbForm.value;

      const request = this.isEditMode && this.stpbId
        ? this.stpbService.update(this.stpbId, formData)
        : this.stpbService.create(formData);

      request.subscribe({
        next: () => {
          this.message.success(`STPB berhasil ${this.isEditMode ? 'diupdate' : 'dibuat'}`);
          this.router.navigate(['/dashboard/stpb']);
        },
        error: () => {
          this.isLoading = false;
          this.message.error(`Gagal ${this.isEditMode ? 'mengupdate' : 'membuat'} STPB`);
        }
      });
    } else {
      Object.values(this.stpbForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/dashboard/stpb']);
  }

  formatterIDR = (value: number): string => `Rp ${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  parserIDR = (value: string): string => value.replace(/Rp\s?|(,*)/g, '');
}
