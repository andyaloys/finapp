import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { NzInputModule } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-kembalikan-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, NzInputModule],
  template: `
    <div>
      <p>Apakah Anda yakin ingin mengembalikan SPTB {{ nomorSTPB }}?</p>
      <p style="margin-top: 16px; margin-bottom: 8px; font-weight: 500;">
        Alasan Pengembalian: <span style="color: red;">*</span>
      </p>
      <textarea 
        nz-input
        [(ngModel)]="alasan"
        rows="4"
        placeholder="Masukkan alasan pengembalian..."></textarea>
    </div>
  `
})
export class KembalikanModalComponent {
  readonly nzModalData = inject(NZ_MODAL_DATA);
  
  nomorSTPB: string = this.nzModalData.nomorSTPB;
  alasan: string = this.nzModalData.alasan || '';
}
