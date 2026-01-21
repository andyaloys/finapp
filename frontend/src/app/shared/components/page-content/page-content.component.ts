import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-page-content',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="page-content-wrapper">
      <ng-content></ng-content>
    </div>
  `,
  styles: [`
    .page-content-wrapper {
      background: white;
      padding: 24px 32px;
      min-height: calc(100vh - 64px - 100px);
    }
  `]
})
export class PageContentComponent {}
