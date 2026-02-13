import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzCardModule,
    NzCheckboxModule,
    NzIconModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;
  passwordVisible = false;
  rememberMe = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private message: NzMessageService
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          if (response.success) {
            this.message.success('Login berhasil!');
            
            // Get user permissions and navigate to first accessible menu
            const user = this.authService.getCurrentUser();
            let redirectUrl = '/no-access';
            
            if (user && user.menuPermissions && user.menuPermissions.length > 0) {
              // Map menu keys to routes
              const menuRouteMap: { [key: string]: string } = {
                'transaksi-stpb': '/stpb',
                'anggaran-list': '/anggaran',
                'monitoring': '/monitoring',
                'admin-ppk-bendahara': '/ppkbendahara',
                'admin-users': '/user',
                'admin-roles': '/role'
              };
              
              // Find first accessible child menu (not parent)
              for (const menuKey of user.menuPermissions) {
                if (menuRouteMap[menuKey]) {
                  redirectUrl = menuRouteMap[menuKey];
                  break;
                }
              }
            }
            
            // Navigate after a short delay to ensure token is stored
            setTimeout(() => {
              this.router.navigate([redirectUrl]).then(() => {
                this.isLoading = false;
              });
            }, 100);
          } else {
            this.isLoading = false;
            this.message.error('Login gagal. Periksa username dan password Anda.');
          }
        },
        error: (error) => {
          this.isLoading = false;
          this.message.error('Login gagal. Periksa username dan password Anda.');
        }
      });
    } else {
      Object.values(this.loginForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
