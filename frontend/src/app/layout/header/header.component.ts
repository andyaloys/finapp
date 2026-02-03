import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { AuthService } from '../../core/services/auth.service';
import { YearService } from '../../core/services/year.service';
import { NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, NzDropDownModule, NzIconModule, NzAvatarModule, NzBadgeModule, NzDividerModule, NzModalModule, NzTagModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  currentUser$ = this.authService.currentUser$;
  selectedYear: number = new Date().getFullYear();
  availableYears: number[] = [];

  constructor(
    private authService: AuthService,
    private yearService: YearService,
    private modal: NzModalService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.availableYears = this.yearService.getAvailableYears();
    this.yearService.selectedYear$.subscribe(year => {
      this.selectedYear = year;
    });
  }

  onYearChange(newYear: number): void {
    if (newYear === this.selectedYear) {
      return;
    }

    this.modal.confirm({
      nzTitle: '<div style="display: flex; align-items: center; gap: 12px;"><span style="font-size: 24px;">🔄</span><span>Ganti Tahun Anggaran</span></div>',
      nzContent: `
        <div style="padding: 16px 0;">
          <div style="background: linear-gradient(135deg, rgba(102, 126, 234, 0.1) 0%, rgba(118, 75, 162, 0.1) 100%); padding: 20px; border-radius: 12px; margin-bottom: 16px; border-left: 4px solid #667eea;">
            <div style="display: flex; align-items: center; justify-content: center; gap: 20px; font-size: 18px; font-weight: 600;">
              <div style="display: flex; align-items: center; gap: 12px;">
                <span style="color: #8c8c8c; font-size: 14px;">DARI</span>
                <div style="background: white; padding: 12px 24px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08);">
                  <span style="color: #667eea;">${this.selectedYear}</span>
                </div>
              </div>
              <i style="color: #667eea; font-size: 24px;">→</i>
              <div style="display: flex; align-items: center; gap: 12px;">
                <span style="color: #8c8c8c; font-size: 14px;">KE</span>
                <div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 12px 24px; border-radius: 8px; box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);">
                  <span style="color: white; font-weight: 700;">${newYear}</span>
                </div>
              </div>
            </div>
          </div>
          <div style="padding: 12px 20px; background: #fffbe6; border-radius: 8px; border: 1px solid #ffe58f;">
            <div style="display: flex; gap: 8px; align-items: start;">
              <span style="color: #faad14; font-size: 16px;">⚠️</span>
              <div style="flex: 1;">
                <div style="color: #595959; font-size: 13px; line-height: 1.6;">
                  Semua data yang ditampilkan akan berubah sesuai tahun anggaran yang dipilih. Halaman akan di-refresh secara otomatis.
                </div>
              </div>
            </div>
          </div>
        </div>
      `,
      nzOkText: 'Ya, Ganti Tahun',
      nzCancelText: 'Batal',
      nzOkDanger: false,
      nzOkType: 'primary',
      nzWidth: 520,
      nzOnOk: () => {
        this.yearService.setSelectedYear(newYear);
        // Reload current page to refresh data
        window.location.reload();
      }
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
