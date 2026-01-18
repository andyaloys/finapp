import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';

@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [CommonModule, NzPageHeaderModule],
  template: `
    <nz-page-header [nzTitle]="title" [nzSubtitle]="subtitle">
      <ng-content></ng-content>
    </nz-page-header>
  `,
  styles: [`
    :host {
      display: block;
      margin-bottom: 16px;
    }
  `]
})
export class PageHeaderComponent {
  @Input() title: string = '';
  @Input() subtitle: string = '';
}
