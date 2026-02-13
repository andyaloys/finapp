import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-no-access',
  standalone: true,
  imports: [CommonModule, NzResultModule, NzButtonModule],
  template: `
    <div class="no-access-container">
      <nz-result
        nzStatus="403"
        nzTitle="Akses Ditolak"
        nzSubTitle="Maaf, Anda tidak memiliki akses ke halaman ini atau tidak memiliki menu yang dapat diakses."
      >
        <div nz-result-extra>
          <button nz-button nzType="primary" (click)="goToFirstAvailableMenu()">
            Coba Menu Lain
          </button>
          <button nz-button (click)="logout()">
            Logout
          </button>
        </div>
      </nz-result>
    </div>
  `,
  styles: [`
    .no-access-container {
      display: flex;
      align-items: center;
      justify-content: center;
      min-height: calc(100vh - 64px);
      padding: 24px;
    }
  `]
})
export class NoAccessComponent {
  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  goToFirstAvailableMenu(): void {
    const user = this.authService.getCurrentUser();
    if (user && user.menuPermissions && user.menuPermissions.length > 0) {
      // Try to navigate to first available menu
      const firstMenu = user.menuPermissions[0];
      const routeMap: { [key: string]: string } = {
        'transaksi-stpb': '/stpb',
        'anggaran-list': '/anggaran',
        'monitoring': '/monitoring',
        'master-ppkbendahara': '/ppkbendahara',
        'admin-users': '/user',
        'admin-roles': '/role',
        'admin-ppk-bendahara': '/ppkbendahara'
      };
      
      const route = routeMap[firstMenu] || '/';
      this.router.navigate([route]);
    } else {
      this.logout();
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
