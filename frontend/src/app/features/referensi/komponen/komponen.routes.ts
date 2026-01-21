import { Routes } from '@angular/router';
import { KomponenListComponent } from './komponen-list.component';
import { KomponenFormComponent } from './komponen-form.component';

export const komponenRoutes: Routes = [
  {
    path: '',
    component: KomponenListComponent
  },
  {
    path: 'create',
    component: KomponenFormComponent
  },
  {
    path: 'edit/:id',
    component: KomponenFormComponent
  }
];
