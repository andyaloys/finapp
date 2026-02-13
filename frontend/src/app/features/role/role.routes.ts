import { Routes } from '@angular/router';

export const ROLE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./role-list.component').then((m) => m.RoleListComponent),
  },
  {
    path: 'create',
    loadComponent: () =>
      import('./role-form.component').then((m) => m.RoleFormComponent),
  },
  {
    path: 'edit/:id',
    loadComponent: () =>
      import('./role-form.component').then((m) => m.RoleFormComponent),
  },
  {
    path: 'assign/:id',
    loadComponent: () =>
      import('./role-assign.component').then((m) => m.RoleAssignComponent),
  },
  {
    path: 'permissions/:id',
    loadComponent: () =>
      import('./role-permissions.component').then((m) => m.RolePermissionsComponent),
  },
];
