import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    CommonModule,
    NzTableModule,
    NzButtonModule,
    NzIconModule,
    NzInputModule,
    NzModalModule,
    NzTagModule,
    FormsModule
  ],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  users: any[] = [];
  isLoading = false;
  searchText = '';
  pageIndex = 1;
  pageSize = 10;
  total = 0;

  constructor(
    private userService: UserService,
    private router: Router,
    private message: NzMessageService,
    private modal: NzModalService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.userService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.users = response.data;
          this.total = response.data.length;
        }
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.message.error('Gagal memuat data user');
      }
    });
  }

  onSearch(): void {
    this.loadUsers();
  }

  refresh(): void {
    this.searchText = '';
    this.loadUsers();
  }

  addUser(): void {
    this.router.navigate(['/user/create']);
  }

  editUser(id: string): void {
    this.router.navigate(['/user/edit', id]);
  }

  deleteUser(id: string): void {
    this.modal.confirm({
      nzTitle: 'Konfirmasi Hapus',
      nzContent: 'Apakah Anda yakin ingin menghapus user ini?',
      nzOkText: 'Hapus',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzCancelText: 'Batal',
      nzOnOk: () => {
        this.userService.delete(id).subscribe({
          next: () => {
            this.message.success('User berhasil dihapus');
            this.loadUsers();
          },
          error: () => {
            this.message.error('Gagal menghapus user');
          }
        });
      }
    });
  }

  get filteredUsers(): any[] {
    if (!this.searchText) {
      return this.users;
    }
    return this.users.filter(user =>
      user.username?.toLowerCase().includes(this.searchText.toLowerCase()) ||
      user.fullName?.toLowerCase().includes(this.searchText.toLowerCase()) ||
      user.email?.toLowerCase().includes(this.searchText.toLowerCase())
    );
  }
}
