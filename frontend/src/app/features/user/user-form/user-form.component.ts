import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzCardModule } from 'ng-zorro-antd/card';
import { UserService } from '../../../core/services/user.service';
import { RoleService } from '../../../core/services/role.service';
import { PpkBendaharaService } from '../../../core/services/ppk-bendahara.service';
import { Role } from '../../../core/models/role.model';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzSelectModule,
    NzSwitchModule,
    NzCardModule
  ],
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
  userForm!: FormGroup;
  isEditMode = false;
  userId?: string;
  isLoading = false;
  roles: Role[] = [];
  ppkBendaharas: any[] = [];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private roleService: RoleService,
    private ppkBendaharaService: PpkBendaharaService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadRoles();
    this.loadPpkBendaharas();
    
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.userId = id;
      this.isEditMode = true;
      this.loadUser(id);
    }
  }

  loadRoles(): void {
    this.roleService.getAllRoles().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.roles = response.data;
        }
      },
      error: (error) => {
        console.error('Error loading roles:', error);
        this.message.error('Gagal memuat data role');
      }
    });
  }

  initForm(): void {
    this.userForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', this.isEditMode ? [] : [Validators.required, Validators.minLength(6)]],
      fullName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      roleId: [null, [Validators.required]],
      ppkBendaharaId: [null],
      isActive: [true]
    });
  }

  loadPpkBendaharas(): void {
    this.ppkBendaharaService.getActive().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.ppkBendaharas = response.data;
        }
      },
      error: (error) => {
        console.error('Error loading PPK/Bendahara:', error);
        this.message.error('Gagal memuat data PPK/Bendahara');
      }
    });
  }

  get selectedRole(): Role | undefined {
    const roleId = this.userForm.get('roleId')?.value;
    return this.roles.find(r => r.id === roleId);
  }

  get showPpkBendaharaField(): boolean {
    const role = this.selectedRole;
    return role?.name === 'PPK' || role?.name === 'Bendahara';
  }
  
  get filteredPpkBendaharas(): any[] {
    const role = this.selectedRole;
    if (!role) return [];
    return this.ppkBendaharas.filter(p => p.jabatanName === role.name);
  }

  onRoleChange(): void {
    const ppkBendaharaControl = this.userForm.get('ppkBendaharaId');
    
    // Reset and update validators based on role
    if (this.showPpkBendaharaField) {
      ppkBendaharaControl?.setValidators([Validators.required]);
    } else {
      ppkBendaharaControl?.clearValidators();
      this.userForm.patchValue({ ppkBendaharaId: null });
    }
    ppkBendaharaControl?.updateValueAndValidity();
  }

  loadUser(id: string): void {
    this.isLoading = true;
    this.userService.getById(id).subscribe({
      next: (response) => {
        if (response.success) {
          this.userForm.patchValue({
            username: response.data.username,
            fullName: response.data.fullName,
            email: response.data.email,
            roleId: response.data.roleId,
            ppkBendaharaId: response.data.ppkBendaharaId,
            isActive: response.data.isActive
          });
          // Update validators after setting role
          this.onRoleChange();
          // Make username readonly in edit mode
          this.userForm.get('username')?.disable();
        }
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.message.error('Gagal memuat data user');
        this.router.navigate(['/user']);
      }
    });
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      this.isLoading = true;
      const formData = this.userForm.getRawValue();
      
      // Remove password if empty in edit mode
      if (this.isEditMode && !formData.password) {
        delete formData.password;
      }

      const request = this.isEditMode && this.userId
        ? this.userService.update(this.userId, formData)
        : this.userService.create(formData);

      request.subscribe({
        next: () => {
          this.message.success(`User berhasil ${this.isEditMode ? 'diupdate' : 'dibuat'}`);
          this.router.navigate(['/user']);
        },
        error: (error) => {
          this.isLoading = false;
          const errorMsg = error.error?.message || `Gagal ${this.isEditMode ? 'mengupdate' : 'membuat'} user`;
          this.message.error(errorMsg);
        }
      });
    } else {
      Object.values(this.userForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/user']);
  }
}
