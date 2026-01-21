import { Routes } from '@angular/router';
import { ItemListComponent } from './item-list.component';
import { ItemFormComponent } from './item-form.component';

export const itemRoutes: Routes = [
  {
    path: '',
    component: ItemListComponent
  },
  {
    path: 'create',
    component: ItemFormComponent
  },
  {
    path: 'edit/:id',
    component: ItemFormComponent
  }
];
