import { Routes } from '@angular/router';

export const ppkBendaharaRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./ppk-bendahara-list/ppk-bendahara-list.component').then(
        (m) => m.PpkBendaharaListComponent
      ),
  },
  {
    path: 'create',
    loadComponent: () =>
      import('./ppk-bendahara-form/ppk-bendahara-form.component').then(
        (m) => m.PpkBendaharaFormComponent
      ),
  },
  {
    path: 'edit/:id',
    loadComponent: () =>
      import('./ppk-bendahara-form/ppk-bendahara-form.component').then(
        (m) => m.PpkBendaharaFormComponent
      ),
  },
];
