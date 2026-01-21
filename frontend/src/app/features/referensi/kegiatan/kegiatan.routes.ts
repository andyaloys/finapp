import { Routes } from '@angular/router';
import { KegiatanListComponent } from './kegiatan-list.component';
import { KegiatanFormComponent } from './kegiatan-form.component';

export const kegiatanRoutes: Routes = [
  {
    path: '',
    component: KegiatanListComponent
  },
  {
    path: 'create',
    component: KegiatanFormComponent
  },
  {
    path: 'edit/:id',
    component: KegiatanFormComponent
  }
];
