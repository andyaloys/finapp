import { Routes } from '@angular/router';

export const MONITORING_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'anggaran',
    pathMatch: 'full'
  },
  {
    path: 'anggaran',
    loadComponent: () => import('./monitoring-anggaran/monitoring-anggaran.component').then(m => m.MonitoringAnggaranComponent)
  }
];
