import { Routes } from '@angular/router';
import { SuboutputListComponent } from './suboutput-list.component';
import { SuboutputFormComponent } from './suboutput-form.component';

export const suboutputRoutes: Routes = [
  {
    path: '',
    component: SuboutputListComponent
  },
  {
    path: 'create',
    component: SuboutputFormComponent
  },
  {
    path: 'edit/:id',
    component: SuboutputFormComponent
  }
];
