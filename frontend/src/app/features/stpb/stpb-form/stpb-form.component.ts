import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { StpbService } from '../../../core/services/stpb.service';
import { PpkBendaharaService } from '../../../core/services/ppk-bendahara.service';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { StpbDetailModalComponent } from '../stpb-detail-modal/stpb-detail-modal.component';
import { Stpb, CreateStpb, UpdateStpb } from '../../../core/models/stpb.model';
import { PpkBendaharaDto } from '../../../core/models/ppk-bendahara.model';
import { StpbDetailDto } from '../../../core/models/stpb-detail.model';
import { StpbStatus, getStatusDisplay, getStatusClass } from '../../../core/models/stpb-status.enum';

@Component({
  selector: 'app-stpb-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzInputNumberModule,
    NzButtonModule,
    NzDatePickerModule,
    NzSelectModule,
    NzCardModule,
    NzTableModule,
    NzModalModule,
    NzTagModule,
    NzPopconfirmModule,
    NzToolTipModule,
    NzIconModule,
    PageHeaderComponent,
    StpbDetailModalComponent
  ],
  templateUrl: './stpb-form.component.html',
  styleUrls: ['./stpb-form.component.scss']
})
export class StpbFormComponent implements OnInit {
  stpbForm: FormGroup;
  isLoading = false;
  isEditMode = false;
  stpbId: string | null = null;
  stpb: Stpb | null = null;

  ppkBendaharaList: PpkBendaharaDto[] = [];
  details: StpbDetailDto[] = [];
  
  detailModalVisible = false;
  editingDetail: StpbDetailDto | null = null;
  
  StpbStatus = StpbStatus;
  
  constructor(
    private fb: FormBuilder,
    private stpbService: StpbService,
    private ppkBendaharaService: PpkBendaharaService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService,
    private modal: NzModalService
  ) {
    this.stpbForm = this.fb.group({
      tahun: [new Date().getFullYear(), [Validators.required, Validators.min(2020), Validators.max(2099)]],
      tanggal: [new Date(), [Validators.required]],
      nomorSTPB: [{value: '', disabled: true}],
      keterangan: [''],
      ppkBendaharaId: [null]
    });
  }

  ngOnInit(): void {
    this.loadPpkBendahara();
    
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.stpbId = id;
      this.loadStpb(id);
    }
  }

  loadPpkBendahara(): void {
    this.ppkBendaharaService.getActive().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.ppkBendaharaList = response.data;
        }
      },
      error: (error) => {
        console.error('Error loading PPK/Bendahara:', error);
      }
    });
  }

  loadStpb(id: string): void {
    this.isLoading = true;
    this.stpbService.getById(id).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.stpb = response.data;
          this.details = response.data.details || [];
          
          this.stpbForm.patchValue({
            tahun: response.data.tahun,
            tanggal: new Date(response.data.tanggalSTPB),
            nomorSTPB: response.data.nomorSTPB,
            keterangan: response.data.keterangan,
            ppkBendaharaId: response.data.ppkBendaharaId
          });

          // Disable form if not in editable status
          if (response.data.status !== StpbStatus.Draft && 
              response.data.status !== StpbStatus.Dikembalikan) {
            this.stpbForm.disable();
          }
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.message.error('Gagal memuat data STPB');
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.stpbForm.valid) {
      this.isLoading = true;
      const formValue = this.stpbForm.value;

      if (this.isEditMode && this.stpbId) {
        const updateDto: UpdateStpb = {
          tahun: formValue.tahun,
          tanggalSTPB: formValue.tanggal,
          keterangan: formValue.keterangan,
          ppkBendaharaId: formValue.ppkBendaharaId
        };

        this.stpbService.update(this.stpbId, updateDto).subscribe({
          next: (response) => {
            if (response.success) {
              this.message.success('STPB berhasil diupdate');
              this.loadStpb(this.stpbId!);
            }
            this.isLoading = false;
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal mengupdate STPB');
            this.isLoading = false;
          }
        });
      } else {
        const createDto: CreateStpb = {
          tahun: formValue.tahun,
          tanggalSTPB: formValue.tanggal,
          keterangan: formValue.keterangan,
          ppkBendaharaId: formValue.ppkBendaharaId
        };

        this.stpbService.create(createDto).subscribe({
          next: (response) => {
            if (response.success && response.data) {
              this.message.success('STPB berhasil dibuat. Silakan tambahkan detail transaksi.');
              this.router.navigate(['/stpb/edit', response.data.id]);
            }
            this.isLoading = false;
          },
          error: (error) => {
            this.message.error(error.error?.message || 'Gagal membuat STPB');
            this.isLoading = false;
          }
        });
      }
    } else {
      Object.values(this.stpbForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  canEdit(): boolean {
    return !this.stpb || 
           this.stpb.status === StpbStatus.Draft || 
           this.stpb.status === StpbStatus.Dikembalikan;
  }

  canKirim(): boolean {
    return this.stpb !== null && 
           (this.stpb.status === StpbStatus.Draft || 
            this.stpb.status === StpbStatus.Dikembalikan) &&
           this.details.length > 0;
  }

  onKirim(): void {
    if (this.stpbId) {
      this.modal.confirm({
        nzTitle: 'Kirim STPB?',
        nzContent: 'STPB yang sudah dikirim tidak bisa diedit. Pastikan data sudah benar.',
        nzOnOk: () => {
          this.stpbService.kirim(this.stpbId!).subscribe({
            next: (response) => {
              if (response.success) {
                this.message.success('STPB berhasil dikirim');
                this.loadStpb(this.stpbId!);
              }
            },
            error: (error) => {
              this.message.error(error.error?.message || 'Gagal mengirim STPB');
            }
          });
        }
      });
    }
  }

  onApprove(): void {
    if (this.stpbId) {
      this.modal.confirm({
        nzTitle: 'Approve STPB?',
        nzContent: 'STPB yang sudah di-approve akan terkunci permanen.',
        nzOnOk: () => {
          this.stpbService.approve(this.stpbId!).subscribe({
            next: (response) => {
              if (response.success) {
                this.message.success('STPB berhasil di-approve');
                this.loadStpb(this.stpbId!);
              }
            },
            error: (error) => {
              this.message.error(error.error?.message || 'Gagal approve STPB');
            }
          });
        }
      });
    }
  }

  onKembalikan(): void {
    if (this.stpbId) {
      this.modal.create({
        nzTitle: 'Kembalikan STPB',
        nzContent: `
          <nz-form-item>
            <nz-form-label>Alasan</nz-form-label>
            <nz-form-control>
              <textarea nz-input rows="3" id="alasanKembalikan" placeholder="Masukkan alasan pengembalian"></textarea>
            </nz-form-control>
          </nz-form-item>
        `,
        nzOnOk: () => {
          const alasan = (document.getElementById('alasanKembalikan') as HTMLTextAreaElement)?.value || '';
          if (!alasan.trim()) {
            this.message.error('Alasan harus diisi');
            return false;
          }
          
          this.stpbService.kembalikan(this.stpbId!, alasan).subscribe({
            next: (response) => {
              if (response.success) {
                this.message.success('STPB berhasil dikembalikan');
                this.loadStpb(this.stpbId!);
              }
            },
            error: (error) => {
              this.message.error(error.error?.message || 'Gagal mengembalikan STPB');
            }
          });
          return true;
        }
      });
    }
  }

  onAddDetail(): void {
    this.editingDetail = null;
    this.detailModalVisible = true;
  }

  onEditDetail(detail: StpbDetailDto): void {
    this.editingDetail = detail;
    this.detailModalVisible = true;
  }

  onDeleteDetail(detail: StpbDetailDto): void {
    if (this.stpbId) {
      this.stpbService.deleteDetail(this.stpbId, detail.id).subscribe({
        next: (response) => {
          if (response.success) {
            this.message.success('Detail berhasil dihapus');
            this.loadStpb(this.stpbId!);
          }
        },
        error: (error) => {
          this.message.error(error.error?.message || 'Gagal menghapus detail');
        }
      });
    }
  }

  getStatusDisplay(status: StpbStatus): string {
    return getStatusDisplay(status);
  }

  getStatusClass(status: StpbStatus): string {
    return getStatusClass(status);
  }

  getTotalNilai(): number {
    return this.details.reduce((sum, detail) => sum + detail.jumlahHarga, 0);
  }

  onDetailModalSuccess(): void {
    console.log('Detail modal success - reloading STPB data');
    if (this.stpbId) {
      this.loadStpb(this.stpbId);
    }
    this.detailModalVisible = false;
  }

  onCancel(): void {
    this.router.navigate(['/stpb']);
  }
}
