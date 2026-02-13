import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { menuPermissionGuard } from './core/guards/menu-permission.guard';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'login',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'no-access',
        loadComponent: () => import('./shared/components/no-access.component').then(m => m.NoAccessComponent)
      },
      {
        path: 'stpb',
        canActivate: [menuPermissionGuard],
        data: { menuKey: 'transaksi-stpb' },
        loadChildren: () => import('./features/stpb/stpb.routes').then(m => m.STPB_ROUTES)
      },
      {
        path: 'user',
        canActivate: [menuPermissionGuard],
        data: { menuKey: 'admin-users' },
        loadChildren: () => import('./features/user/user.routes').then(m => m.USER_ROUTES)
      },
      {
        path: 'role',
        canActivate: [menuPermissionGuard],
        data: { menuKey: 'admin-roles' },
        loadChildren: () => import('./features/role/role.routes').then(m => m.ROLE_ROUTES)
      },
      {
        path: 'anggaran',
        canActivate: [menuPermissionGuard],
        data: { menuKey: 'anggaran-list' },
        loadChildren: () => import('./features/anggaran/anggaran.routes').then(m => m.ANGGARAN_ROUTES)
      },
      {
        path: 'ppkbendahara',
        canActivate: [menuPermissionGuard],
        data: { menuKey: 'admin-ppk-bendahara' },
        loadChildren: () => import('./features/ppk-bendahara/ppk-bendahara.routes').then(m => m.ppkBendaharaRoutes)
      },
      {
        path: 'monitoring',
        canActivate: [menuPermissionGuard],
        data: { menuKey: 'monitoring' },
        loadChildren: () => import('./features/monitoring/monitoring.routes').then(m => m.MONITORING_ROUTES)
      },
      {
        path: 'supplier',
        canActivate: [menuPermissionGuard],
        data: { menuKey: 'master-supplier' },
        loadComponent: () => import('./features/supplier/supplier.component').then(m => m.SupplierComponent)
      },
      {
        path: 'tax-rate',
        canActivate: [menuPermissionGuard],
        data: { menuKey: 'master-taxrate' },
        loadComponent: () => import('./features/tax-rate/tax-rate.component').then(m => m.TaxRateComponent)
      },
      // Referensi routes dinonaktifkan
      /*
      {
        path: 'referensi/program',
        loadChildren: () => import('./features/referensi/program/program.routes').then(m => m.programRoutes)
      },
      {
        path: 'referensi/kegiatan',
        loadChildren: () => import('./features/referensi/kegiatan/kegiatan.routes').then(m => m.kegiatanRoutes)
      },
      {
        path: 'referensi/output',
        loadChildren: () => import('./features/referensi/output/output.routes').then(m => m.outputRoutes)
      },
      {
        path: 'referensi/suboutput',
        loadChildren: () => import('./features/referensi/suboutput/suboutput.routes').then(m => m.suboutputRoutes)
      },
      {
        path: 'referensi/komponen',
        loadChildren: () => import('./features/referensi/komponen/komponen.routes').then(m => m.komponenRoutes)
      },
      {
        path: 'referensi/subkomponen',
        loadChildren: () => import('./features/referensi/subkomponen/subkomponen.routes').then(m => m.subkomponenRoutes)
      },
      {
        path: 'referensi/akun',
        loadChildren: () => import('./features/referensi/akun/akun.routes').then(m => m.akunRoutes)
      },
      {
        path: 'referensi/item',
        loadChildren: () => import('./features/referensi/item/item.routes').then(m => m.itemRoutes)
      },
      */
    ]
  },
  { path: '**', redirectTo: '/login' }
];
