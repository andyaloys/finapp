import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="page-header-wrapper">
      <div class="page-header-content">
        <div class="page-header-left">
          <h1 class="page-title">{{ title }}</h1>
          <p class="page-subtitle" *ngIf="subtitle">{{ subtitle }}</p>
        </div>
        <div class="page-header-right">
          <ng-content></ng-content>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .page-header-wrapper {
      background: white;
      padding: 20px 32px;
      margin-bottom: 0;
      border-bottom: 1px solid #f0f0f0;
    }

    .page-header-content {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .page-header-left {
      flex: 1;
    }

    .page-title {
      font-size: 20px;
      font-weight: 600;
      color: #262626;
      margin: 0 0 4px 0;
    }

    .page-subtitle {
      font-size: 13px;
      color: #8c8c8c;
      margin: 0;
    }

    .page-header-right {
      display: flex;
      align-items: center;
      gap: 12px;
    }
  `]
})
export class PageHeaderComponent {
  @Input() title: string = '';
  @Input() subtitle: string = '';
}
