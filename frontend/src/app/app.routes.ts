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
    path: 'dashboard',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'stpb', pathMatch: 'full' },
      {
        path: 'stpb',
        loadChildren: () => import('./features/stpb/stpb.routes').then(m => m.STPB_ROUTES)
      }
    ]
  },
  { path: '**', redirectTo: '/login' }
];
