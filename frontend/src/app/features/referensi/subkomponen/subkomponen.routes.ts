import { Routes } from '@angular/router';
import { SubkomponenListComponent } from './subkomponen-list.component';
import { SubkomponenFormComponent } from './subkomponen-form.component';

export const subkomponenRoutes: Routes = [
  {
    path: '',
    component: SubkomponenListComponent
  },
  {
    path: 'create',
    component: SubkomponenFormComponent
  },
  {
    path: 'edit/:id',
    component: SubkomponenFormComponent
  }
];
