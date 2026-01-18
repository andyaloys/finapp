import { Routes } from '@angular/router';
import { StpbListComponent } from './stpb-list/stpb-list.component';
import { StpbFormComponent } from './stpb-form/stpb-form.component';

export const STPB_ROUTES: Routes = [
  {
    path: '',
    component: StpbListComponent
  },
  {
    path: 'create',
    component: StpbFormComponent
  },
  {
    path: 'edit/:id',
    component: StpbFormComponent
  }
];
