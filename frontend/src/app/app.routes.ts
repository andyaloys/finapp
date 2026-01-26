import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
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
        path: 'stpb',
        loadChildren: () => import('./features/stpb/stpb.routes').then(m => m.STPB_ROUTES)
      },
      {
        path: 'user',
        loadChildren: () => import('./features/user/user.routes').then(m => m.USER_ROUTES)
      },
      {
        path: 'anggaran',
        loadChildren: () => import('./features/anggaran/anggaran.routes').then(m => m.ANGGARAN_ROUTES)
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
