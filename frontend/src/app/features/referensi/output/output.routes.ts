import { Routes } from '@angular/router';
import { OutputListComponent } from './output-list.component';
import { OutputFormComponent } from './output-form.component';

export const outputRoutes: Routes = [
  {
    path: '',
    component: OutputListComponent
  },
  {
    path: 'create',
    component: OutputFormComponent
  },
  {
    path: 'edit/:id',
    component: OutputFormComponent
  }
];
