import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { MenuPermissionService } from '../../core/services/menu-permission.service';
import { RoleService } from '../../core/services/role.service';
import { Menu } from '../../core/models/user.model';

@Component({
  selector: 'app-role-permissions',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzCardModule,
    NzCheckboxModule,
    NzButtonModule,
    NzSpinModule
  ],
  template: `
    <div class="page-container">
      <nz-card [nzTitle]="'Kelola Menu Permission - ' + roleName" [nzLoading]="loading">
        <div class="permissions-container" *ngIf="!loading">
          <div class="menu-group" *ngFor="let group of menuGroups">
            <h3>
              <label nz-checkbox
                [(ngModel)]="group.checked"
                [nzIndeterminate]="group.indeterminate"
                (ngModelChange)="onGroupChange(group)">
                {{ group.label }}
              </label>
            </h3>
            <div class="menu-children" *ngIf="group.children.length > 0">
              <label nz-checkbox
                *ngFor="let child of group.children"
                [(ngModel)]="child.checked"
                (ngModelChange)="onChildChange(group)">
                {{ child.label }}
              </label>
            </div>
          </div>
        </div>
        
        <div class="action-buttons">
          <button nz-button nzType="primary" (click)="savePermissions()" [nzLoading]="submitting">
            Simpan Permissions
          </button>
          <button nz-button (click)="goBack()" style="margin-left: 8px;">
            Kembali
          </button>
        </div>
      </nz-card>
    </div>
  `,
  styles: [`
    .page-container {
      padding: 24px;
      max-width: 1000px;
    }

    .permissions-container {
      margin-bottom: 24px;
    }

    .menu-group {
      margin-bottom: 24px;
      padding: 16px;
      background: #fafafa;
      border-radius: 4px;

      h3 {
        margin: 0 0 12px 0;
        font-size: 16px;
        font-weight: 600;
      }
    }

    .menu-children {
      display: flex;
      flex-direction: column;
      gap: 8px;
      margin-left: 24px;

      label {
        padding: 4px 0;
      }
    }

    .action-buttons {
      padding-top: 16px;
      border-top: 1px solid #f0f0f0;
    }
  `]
})
export class RolePermissionsComponent implements OnInit {
  roleId: string = '';
  roleName: string = '';
  loading = false;
  submitting = false;
  
  allMenus: Menu[] = [];
  selectedMenuKeys: string[] = [];
  menuGroups: MenuGroup[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private menuPermissionService: MenuPermissionService,
    private roleService: RoleService,
    private message: NzMessageService
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
        if (response.success && response.data) {
          this.roleName = response.data.name;
        }
      }
    });

    // Load all menus
    this.menuPermissionService.getAllMenus().subscribe({
      next: (response) => {
        if (response.success) {
          this.allMenus = response.data;
          
          // Load current permissions
          this.menuPermissionService.getRolePermissions(this.roleId).subscribe({
            next: (permResponse) => {
              if (permResponse.success) {
                this.selectedMenuKeys = permResponse.data;
                this.buildMenuGroups();
              }
              this.loading = false;
            }
          });
        }
      },
      error: () => {
        this.message.error('Gagal memuat data menu');
        this.loading = false;
      }
    });
  }

  buildMenuGroups(): void {
    const parents = this.allMenus.filter(m => !m.parentKey);
    
    this.menuGroups = parents.map(parent => {
      const children = this.allMenus
        .filter(m => m.parentKey === parent.key)
        .map(child => ({
          key: child.key,
          label: child.label,
          checked: this.selectedMenuKeys.includes(child.key)
        }));

      const parentChecked = this.selectedMenuKeys.includes(parent.key);
      
      // If no children, use parent checked state directly
      if (children.length === 0) {
        return {
          key: parent.key,
          label: parent.label,
          checked: parentChecked,
          indeterminate: false,
          children
        };
      }
      
      // If has children, parent checkbox state based on children
      const allChildrenChecked = children.every(c => c.checked);
      const someChildrenChecked = children.some(c => c.checked);

      return {
        key: parent.key,
        label: parent.label,
        checked: allChildrenChecked,
        indeterminate: someChildrenChecked && !allChildrenChecked,
        children
      };
    });
  }

  onGroupChange(group: MenuGroup): void {
    group.children.forEach(child => child.checked = group.checked);
    group.indeterminate = false;
  }

  onChildChange(group: MenuGroup): void {
    const allChecked = group.children.every(c => c.checked);
    const someChecked = group.children.some(c => c.checked);
    
    group.checked = allChecked;
    group.indeterminate = someChecked && !allChecked;
  }

  savePermissions(): void {
    this.submitting = true;
    
    const menuKeys: string[] = [];
    
    this.menuGroups.forEach(group => {
      if (group.checked || group.indeterminate) {
        menuKeys.push(group.key);
      }
      group.children.forEach(child => {
        if (child.checked) {
          menuKeys.push(child.key);
        }
      });
    });

    this.menuPermissionService.updateRolePermissions(this.roleId, menuKeys).subscribe({
      next: (response) => {
        if (response.success) {
          this.message.success('Permissions berhasil disimpan');
          this.router.navigate(['/role']);
        } else {
          this.message.error(response.message || 'Gagal menyimpan permissions');
        }
        this.submitting = false;
      },
      error: () => {
        this.message.error('Gagal menyimpan permissions');
        this.submitting = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/role']);
  }
}

interface MenuGroup {
  key: string;
  label: string;
  checked: boolean;
  indeterminate: boolean;
  children: MenuChild[];
}

interface MenuChild {
  key: string;
  label: string;
  checked: boolean;
}
