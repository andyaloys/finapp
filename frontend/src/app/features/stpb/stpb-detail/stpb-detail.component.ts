import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzInputModule } from 'ng-zorro-antd/input';
import { FormsModule } from '@angular/forms';
import { StpbService } from '../../../core/services/stpb.service';
import { AuthService } from '../../../core/services/auth.service';
import { Stpb } from '../../../core/models/stpb.model';
import { StpbDetail } from '../../../core/models/stpb-detail.model';
import { StpbStatus, getStatusClass, getStatusDisplay } from '../../../core/models/stpb-status.enum';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-stpb-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzCardModule,
    NzButtonModule,
    NzIconModule,
    NzTableModule,
    NzTagModule,
    NzModalModule,
    NzDescriptionsModule,
    NzDividerModule,
    NzInputModule,
    PageHeaderComponent
  ],
  templateUrl: './stpb-detail.component.html',
  styleUrls: ['./stpb-detail.component.scss']
})
export class StpbDetailComponent implements OnInit {
  stpb: Stpb | null = null;
  loading = false;
  currentUser: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private stpbService: StpbService,
    private authService: AuthService,
    private message: NzMessageService,
    private modal: NzModalService
  ) {
    this.currentUser = this.authService.getCurrentUser();
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadStpb(id);
    }
  }

  loadStpb(id: string): void {
    this.loading = true;
    this.stpbService.getById(id).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.stpb = response.data;
          console.log('STPB Detail loaded:', this.stpb);
          console.log('Details:', this.stpb.details);
        }
        this.loading = false;
      },
      error: () => {
        this.message.error('Gagal memuat detail SPTB');
        this.loading = false;
      }
    });
  }

  canApprove(): boolean {
    if (!this.stpb || !this.currentUser) return false;
    
    const role = this.currentUser.role;
    return (role === 'PPK' || role === 'Bendahara') && 
           this.stpb.status === StpbStatus.Kirim;
  }

  approve(): void {
    if (!this.stpb) return;

    this.modal.confirm({
      nzTitle: 'Konfirmasi Approve',
      nzContent: `Apakah Anda yakin ingin meng-approve SPTB ${this.stpb.nomorSTPB}?`,
      nzOnOk: () => {
        this.stpbService.approve(this.stpb!.id).subscribe({
          next: () => {
            this.message.success('SPTB berhasil di-approve');
            this.router.navigate(['/stpb']);
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal meng-approve SPTB');
          }
        });
      }
    });
  }

  kembalikan(): void {
    if (!this.stpb) return;

    this.modal.confirm({
      nzTitle: 'Konfirmasi Kembalikan',
      nzContent: `Apakah Anda yakin ingin mengembalikan SPTB ${this.stpb.nomorSTPB}?`,
      nzOnOk: () => {
        this.stpbService.kembalikan(this.stpb!.id, '').subscribe({
          next: () => {
            this.message.success('SPTB berhasil dikembalikan');
            this.router.navigate(['/stpb']);
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal mengembalikan SPTB');
          }
        });
      }
    });
  }

  back(): void {
    this.router.navigate(['/stpb']);
  }

  print(): void {
    if (this.stpb) {
      // Reuse print logic from list component
      window.open(`/stpb/print/${this.stpb.id}`, '_blank');
    }
  }

  getStatusClass(status: StpbStatus): string {
    return getStatusClass(status);
  }

  getStatusDisplay(status: StpbStatus): string {
    return getStatusDisplay(status);
  }

  getTotalNilai(): number {
    return this.stpb?.details?.reduce((sum, d) => sum + d.jumlahHarga, 0) || 0;
  }

  getTotalPPN(): number {
    return this.stpb?.details?.reduce((sum, d) => sum + d.ppn, 0) || 0;
  }

  getTotalPPH(): number {
    return this.stpb?.details?.reduce((sum, d) => sum + (d.ppH21 + d.ppH22 + d.ppH23), 0) || 0;
  }
}
