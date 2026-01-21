import { Routes } from '@angular/router';
import { ProgramListComponent } from './program-list.component';
import { ProgramFormComponent } from './program-form.component';

export const programRoutes: Routes = [
  {
    path: '',
    component: ProgramListComponent
  },
  {
    path: 'create',
    component: ProgramFormComponent
  },
  {
    path: 'edit/:id',
    component: ProgramFormComponent
  }
];
