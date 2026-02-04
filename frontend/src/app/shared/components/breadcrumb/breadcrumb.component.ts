import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, NavigationEnd, Router, RouterModule } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { filter, distinctUntilChanged } from 'rxjs/operators';

interface Breadcrumb {
  label: string;
  url: string;
}

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [CommonModule, RouterModule, NzBreadCrumbModule],
  template: `
    <nz-breadcrumb>
      <nz-breadcrumb-item>
        <a routerLink="/dashboard">
          <i nz-icon nzType="home"></i>
        </a>
      </nz-breadcrumb-item>
      <nz-breadcrumb-item *ngFor="let breadcrumb of breadcrumbs">
        <a [routerLink]="breadcrumb.url">{{ breadcrumb.label }}</a>
      </nz-breadcrumb-item>
    </nz-breadcrumb>
  `,
  styles: [`
    :host {
      display: block;
      padding: 16px 24px;
      background: #fff;
      margin-bottom: 16px;
      position: sticky;
      top: 64px;
      z-index: 8;
      box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);
    }
  `]
})
export class BreadcrumbComponent implements OnInit {
  breadcrumbs: Breadcrumb[] = [];

  private routeLabels: { [key: string]: string } = {
    'dashboard': 'Dashboard',
    'master': 'Master Data',
    'akun': 'Akun',
    'item': 'Item',
    'program': 'Program',
    'kegiatan': 'Kegiatan',
    'output': 'Output',
    'suboutput': 'Suboutput',
    'komponen': 'Komponen',
    'subkomponen': 'Subkomponen',
    'anggaran': 'Anggaran Master',
    'stpb': 'SPTB',
    'user': 'User',
    'monitoring': 'Monitoring',
    'monitoring-anggaran': 'Monitoring Anggaran',
    'create': 'Tambah',
    'edit': 'Edit',
    'list': 'Daftar'
  };

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.breadcrumbs = this.buildBreadcrumbs(this.activatedRoute.root);
      });

    // Initial load
    this.breadcrumbs = this.buildBreadcrumbs(this.activatedRoute.root);
  }

  private buildBreadcrumbs(
    route: ActivatedRoute,
    url: string = '',
    breadcrumbs: Breadcrumb[] = []
  ): Breadcrumb[] {
    const children: ActivatedRoute[] = route.children;

    if (children.length === 0) {
      return breadcrumbs;
    }

    for (const child of children) {
      const routeURL: string = child.snapshot.url.map(segment => segment.path).join('/');
      if (routeURL !== '') {
        url += `/${routeURL}`;
      }

      const label = this.getLabel(child);
      if (label && !breadcrumbs.some(b => b.url === url)) {
        breadcrumbs.push({ label, url });
      }

      return this.buildBreadcrumbs(child, url, breadcrumbs);
    }

    return breadcrumbs;
  }

  private getLabel(route: ActivatedRoute): string {
    // Check if route has custom breadcrumb data
    if (route.snapshot.data['breadcrumb']) {
      return route.snapshot.data['breadcrumb'];
    }

    // Get label from path
    const path = route.snapshot.url[0]?.path;
    if (path && this.routeLabels[path]) {
      return this.routeLabels[path];
    }

    // If it's a dynamic route (like ID), don't show in breadcrumb
    if (path && /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(path)) {
      return '';
    }

    return path || '';
  }
}
