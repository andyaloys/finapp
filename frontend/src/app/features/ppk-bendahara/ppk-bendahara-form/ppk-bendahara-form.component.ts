import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzCardModule } from 'ng-zorro-antd/card';

import { PpkBendaharaService } from '../../../core/services/ppk-bendahara.service';
import { CreatePpkBendaharaDto, JabatanType } from '../../../core/models/ppk-bendahara.model';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-ppk-bendahara-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzSelectModule,
    NzSwitchModule,
    NzCardModule,
    PageHeaderComponent
  ],
  templateUrl: './ppk-bendahara-form.component.html',
  styleUrls: ['./ppk-bendahara-form.component.scss']
})
export class PpkBendaharaFormComponent implements OnInit {
  ppkForm: FormGroup;
  isLoading = false;
  isEditMode = false;
  ppkId: string | null = null;

  jabatanOptions = [
    { value: JabatanType.PPK, label: 'PPK' },
    { value: JabatanType.Bendahara, label: 'Bendahara' }
  ];

  constructor(
    private fb: FormBuilder,
    private ppkBendaharaService: PpkBendaharaService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.ppkForm = this.fb.group({
      nama: ['', [Validators.required, Validators.maxLength(200)]],
      nip: ['', [Validators.required, Validators.pattern('^[0-9]+$'), Validators.maxLength(20)]],
      jabatan: [JabatanType.PPK, [Validators.required]],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.ppkId = id;
      this.loadData(id);
    }
  }

  loadData(id: string): void {
    this.isLoading = true;
    this.ppkBendaharaService.getById(id).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.ppkForm.patchValue({
            nama: response.data.nama,
            nip: response.data.nip,
            jabatan: response.data.jabatan,
            isActive: response.data.isActive
          });
        }
        this.isLoading = false;
      },
      error: () => {
        this.message.error('Gagal memuat data');
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.ppkForm.valid) {
      this.isLoading = true;
      const formValue = this.ppkForm.value;

      const dto: CreatePpkBendaharaDto = {
        nama: formValue.nama,
        nip: formValue.nip,
        jabatan: formValue.jabatan,
        isActive: formValue.isActive
      };

      if (this.isEditMode && this.ppkId) {
        this.ppkBendaharaService.update(this.ppkId, dto).subscribe({
          next: (response) => {
            if (response.success) {
              this.message.success('Data berhasil diupdate');
              this.router.navigate(['/ppkbendahara']);
            }
            this.isLoading = false;
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal mengupdate data');
            this.isLoading = false;
          }
        });
      } else {
        this.ppkBendaharaService.create(dto).subscribe({
          next: (response) => {
            if (response.success) {
              this.message.success('Data berhasil ditambahkan');
              this.router.navigate(['/ppkbendahara']);
            }
            this.isLoading = false;
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal menambah data');
            this.isLoading = false;
          }
        });
      }
    } else {
      Object.values(this.ppkForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/ppkbendahara']);
  }
}
