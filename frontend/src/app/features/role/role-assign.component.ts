import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTransferModule } from 'ng-zorro-antd/transfer';
import { NzMessageService } from 'ng-zorro-antd/message';
import { RoleService } from '../../core/services/role.service';
import { RoleSuboutput } from '../../core/models/role.model';
import { AnggaranMasterService } from '../../core/services/anggaran-master.service';

interface TransferItem {
  key: string;
  title: string;
  direction: 'left' | 'right';
}

@Component({
  selector: 'app-role-assign',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzCardModule,
    NzButtonModule,
    NzTransferModule
  ],
  template: `
    <div class="page-container">
      <nz-card [nzTitle]="'Assign Suboutputs - ' + roleName" [nzLoading]="loading">
        <p style="margin-bottom: 16px; color: #8c8c8c;">
          Pilih suboutput yang dapat diakses oleh role ini
        </p>
        
        <nz-transfer
          [nzDataSource]="transferData"
          [nzTitles]="['Available', 'Assigned']"
          [nzShowSearch]="true"
          (nzChange)="onChange($event)"
          (nzSearchChange)="onSearch($event)"
        ></nz-transfer>

        <div style="margin-top: 24px;">
          <button
            nz-button
            nzType="primary"
            (click)="onSubmit()"
            [nzLoading]="submitting"
            [disabled]="submitting"
          >
            Simpan
          </button>
          <button
            nz-button
            type="button"
            (click)="goBack()"
            style="margin-left: 8px;"
          >
            Batal
          </button>
        </div>
      </nz-card>
    </div>
  `,
  styles: [`
    .page-container {
      padding: 24px;
    }

    ::ng-deep .ant-transfer {
      display: flex;
      justify-content: center;
    }
  `]
})
export class RoleAssignComponent implements OnInit {
  roleId: string = '';
  roleName: string = '';
  loading = false;
  submitting = false;
  transferData: TransferItem[] = [];
  assignedSuboutputs: string[] = [];

  constructor(
    private roleService: RoleService,
    private anggaranService: AnggaranMasterService,
    private message: NzMessageService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.roleId = this.route.snapshot.paramMap.get('id') || '';
    if (this.roleId) {
      this.loadData();
    }
  }

  loadData(): void {
    this.loading = true;

    // Load role info
    this.roleService.getRoleById(this.roleId).subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.roleName = response.data.name;
        }
      },
      error: (error) => console.error(error)
    });

    // Load assigned suboutputs
    this.roleService.getRoleSuboutputs(this.roleId).subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.assignedSuboutputs = response.data.map((rs: RoleSuboutput) => rs.kodeSuboutput);
          this.loadAllSuboutputs();
        }
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }

  loadAllSuboutputs(): void {
    // Get all distinct suboutputs from anggaran
    this.anggaranService.getAllSuboutputs().subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.transferData = response.data.map((item: any) => ({
            key: item.kdSOutput,
            title: `${item.kdSOutput} - ${item.nmSOutput}`,
            direction: this.assignedSuboutputs.includes(item.kdSOutput) ? 'right' : 'left'
          }));
        }
        this.loading = false;
      },
      error: (error) => {
        this.message.error('Gagal memuat data suboutput');
        console.error(error);
        this.loading = false;
      }
    });
  }

  onChange(event: any): void {
    // Transfer component handles the data update automatically
  }

  onSearch(event: any): void {
    // Search is handled by nz-transfer component
  }

  onSubmit(): void {
    this.submitting = true;
    
    // Get all items in the right panel (assigned)
    const assignedKeys = this.transferData
      .filter(item => item.direction === 'right')
      .map(item => item.key);

    this.roleService.assignSuboutputs(this.roleId, { kodeSuboutputs: assignedKeys }).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.message.success('Suboutput berhasil di-assign');
          this.router.navigate(['/role']);
        } else {
          this.message.error(response.message || 'Gagal menyimpan assignment');
        }
        this.submitting = false;
      },
      error: (error) => {
        this.message.error('Gagal menyimpan assignment');
        console.error(error);
        this.submitting = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/role']);
  }
}
