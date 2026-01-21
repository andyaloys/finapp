import { Routes } from '@angular/router';
import { AkunListComponent } from './akun-list.component';
import { AkunFormComponent } from './akun-form.component';

export const akunRoutes: Routes = [
  {
    path: '',
    component: AkunListComponent
  },
  {
    path: 'create',
    component: AkunFormComponent
  },
  {
    path: 'edit/:id',
    component: AkunFormComponent
  }
];
