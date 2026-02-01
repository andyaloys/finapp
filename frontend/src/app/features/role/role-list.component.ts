import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { RoleService } from '../../core/services/role.service';
import { Role } from '../../core/models/role.model';

@Component({
  selector: 'app-role-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzTableModule,
    NzButtonModule,
    NzIconModule,
    NzInputModule,
    NzModalModule,
    NzTagModule
  ],
  template: `
    <div class="page-container">
      <div class="page-header">
        <h2>Role Management</h2>
        <button nz-button nzType="primary" (click)="navigateToCreate()">
          <span nz-icon nzType="plus"></span>
          Tambah Role
        </button>
      </div>

      <div class="search-container">
        <nz-input-group [nzSuffix]="suffixIconSearch">
          <input
            type="text"
            nz-input
            placeholder="Cari role..."
            [(ngModel)]="searchTerm"
            (ngModelChange)="onSearch()"
          />
        </nz-input-group>
        <ng-template #suffixIconSearch>
          <span nz-icon nzType="search"></span>
        </ng-template>
      </div>

      <nz-table
        #roleTable
        [nzData]="roles"
        [nzLoading]="loading"
        [nzTotal]="totalCount"
        [nzPageSize]="pageSize"
        [nzPageIndex]="pageNumber"
        [nzFrontPagination]="false"
        (nzQueryParams)="onQueryParamsChange($event)"
      >
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Type</th>
            <th nzWidth="200px">Action</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let role of roles">
            <td>{{ role.name }}</td>
            <td>{{ role.description || '-' }}</td>
            <td>
              <nz-tag [nzColor]="role.isAdmin ? 'red' : 'blue'">
                {{ role.isAdmin ? 'Admin' : 'User' }}
              </nz-tag>
            </td>
            <td>
              <button
                nz-button
                nzType="default"
                nzSize="small"
                (click)="navigateToEdit(role.id)"
                style="margin-right: 8px;"
              >
                <span nz-icon nzType="edit"></span>
              </button>
              <button
                nz-button
                nzType="default"
                nzSize="small"
                (click)="navigateToAssign(role.id)"
                style="margin-right: 8px;"
              >
                <span nz-icon nzType="setting"></span>
                Suboutputs
              </button>
              <button
                nz-button
                nzType="primary"
                nzDanger
                nzSize="small"
                (click)="confirmDelete(role)"
                [disabled]="role.isAdmin"
              >
                <span nz-icon nzType="delete"></span>
              </button>
            </td>
          </tr>
        </tbody>
      </nz-table>
    </div>
  `,
  styles: [`
    .page-container {
      padding: 24px;
    }

    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 24px;
    }

    .page-header h2 {
      margin: 0;
    }

    .search-container {
      margin-bottom: 16px;
      max-width: 400px;
    }
  `]
})
export class RoleListComponent implements OnInit {
  roles: Role[] = [];
  loading = false;
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;
  searchTerm = '';

  constructor(
    private roleService: RoleService,
    private message: NzMessageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.loading = true;
    this.roleService.getRoles(this.pageNumber, this.pageSize, this.searchTerm).subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.roles = response.data.items;
          this.totalCount = response.data.totalCount;
        }
        this.loading = false;
      },
      error: (error) => {
        this.message.error('Gagal memuat data role');
        console.error(error);
        this.loading = false;
      }
    });
  }

  onQueryParamsChange(params: any): void {
    this.pageNumber = params.pageIndex;
    this.pageSize = params.pageSize;
    this.loadRoles();
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadRoles();
  }

  navigateToCreate(): void {
    this.router.navigate(['/role/create']);
  }

  navigateToEdit(id: string): void {
    this.router.navigate(['/role/edit', id]);
  }

  navigateToAssign(id: string): void {
    this.router.navigate(['/role/assign', id]);
  }

  confirmDelete(role: Role): void {
    if (role.isAdmin) {
      this.message.warning('Admin role tidak dapat dihapus');
      return;
    }

    const modal = document.createElement('div');
    // Using NzModal service would be better, but for simplicity:
    if (confirm(`Apakah Anda yakin ingin menghapus role "${role.name}"?`)) {
      this.deleteRole(role.id);
    }
  }

  deleteRole(id: string): void {
    this.roleService.deleteRole(id).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.message.success('Role berhasil dihapus');
          this.loadRoles();
        } else {
          this.message.error(response.message || 'Gagal menghapus role');
        }
      },
      error: (error) => {
        this.message.error('Gagal menghapus role');
        console.error(error);
      }
    });
  }
}
