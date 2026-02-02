import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { RoleService } from '../../core/services/role.service';

@Component({
  selector: 'app-role-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzSwitchModule,
    NzCardModule
  ],
  template: `
    <div class="page-container">
      <nz-card [nzTitle]="isEditMode ? 'Edit Role' : 'Tambah Role Baru'">
        <form nz-form [formGroup]="roleForm" (ngSubmit)="onSubmit()">
          <nz-form-item>
            <nz-form-label [nzSpan]="6" nzRequired>Nama Role</nz-form-label>
            <nz-form-control [nzSpan]="14" nzErrorTip="Nama role wajib diisi">
              <input nz-input formControlName="name" placeholder="Masukkan nama role" />
            </nz-form-control>
          </nz-form-item>

          <nz-form-item>
            <nz-form-label [nzSpan]="6">Deskripsi</nz-form-label>
            <nz-form-control [nzSpan]="14">
              <textarea
                nz-input
                formControlName="description"
                placeholder="Masukkan deskripsi role"
                [nzAutosize]="{ minRows: 3, maxRows: 6 }"
              ></textarea>
            </nz-form-control>
          </nz-form-item>

          <nz-form-item>
            <nz-form-label [nzSpan]="6">Admin Role</nz-form-label>
            <nz-form-control [nzSpan]="14">
              <nz-switch formControlName="isAdmin"></nz-switch>
              <span style="margin-left: 8px; color: #8c8c8c;">
                Admin memiliki akses penuh ke semua data
              </span>
            </nz-form-control>
          </nz-form-item>

          <nz-form-item>
            <nz-form-control [nzSpan]="14" [nzOffset]="6">
              <button
                nz-button
                nzType="primary"
                type="submit"
                [disabled]="!roleForm.valid || submitting"
                [nzLoading]="submitting"
              >
                {{ isEditMode ? 'Update' : 'Simpan' }}
              </button>
              <button
                nz-button
                type="button"
                (click)="goBack()"
                style="margin-left: 8px;"
              >
                Batal
              </button>
            </nz-form-control>
          </nz-form-item>
        </form>
      </nz-card>
    </div>
  `,
  styles: [`
    .page-container {
      padding: 24px;
      max-width: 800px;
    }
  `]
})
export class RoleFormComponent implements OnInit {
  roleForm: FormGroup;
  isEditMode = false;
  roleId?: string;
  submitting = false;

  constructor(
    private fb: FormBuilder,
    private roleService: RoleService,
    private message: NzMessageService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.roleForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      isAdmin: [false]
    });
  }

  ngOnInit(): void {
    this.roleId = this.route.snapshot.paramMap.get('id') || undefined;
    this.isEditMode = !!this.roleId;

    if (this.isEditMode && this.roleId) {
      this.loadRole(this.roleId);
    }
  }

  loadRole(id: string): void {
    this.roleService.getRoleById(id).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.roleForm.patchValue({
            name: response.data.name,
            description: response.data.description,
            isAdmin: response.data.isAdmin
          });
        }
      },
      error: (error) => {
        this.message.error('Gagal memuat data role');
        console.error(error);
      }
    });
  }

  onSubmit(): void {
    if (this.roleForm.valid) {
      this.submitting = true;
      const formValue = this.roleForm.value;

      const request$ = this.isEditMode && this.roleId
        ? this.roleService.updateRole(this.roleId, formValue)
        : this.roleService.createRole(formValue);

      request$.subscribe({
        next: (response) => {
          if (response.success) {
            this.message.success(
              this.isEditMode ? 'Role berhasil diupdate' : 'Role berhasil ditambahkan'
            );
            this.router.navigate(['/role']);
          } else {
            this.message.error(response.message || 'Gagal menyimpan role');
          }
          this.submitting = false;
        },
        error: (error) => {
          this.message.error('Gagal menyimpan role');
          console.error(error);
          this.submitting = false;
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/role']);
  }
}
