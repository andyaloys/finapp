import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageModule, NzMessageService } from 'ng-zorro-antd/message';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { AnggaranMasterService } from '../../core/services/anggaran-master.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { NzSpinModule } from 'ng-zorro-antd/spin';

@Component({
  selector: 'app-anggaran-list',
  standalone: true,
  imports: [CommonModule, NzTableModule, NzButtonModule, ReactiveFormsModule, NzInputModule, NzSpinModule, NzMessageModule, NzIconModule, NzModalModule, NzToolTipModule],
  template: `
    <div class="anggaran-list-header">
      <h2>Daftar Anggaran</h2>
      <button nz-button nzType="primary" (click)="showUploadForm = !showUploadForm">
        {{ showUploadForm ? 'Batal' : 'Upload Anggaran' }}
      </button>
    </div>

    <div *ngIf="showUploadForm" class="upload-form-container">
      <form [formGroup]="uploadForm" (ngSubmit)="submitUpload()">
        <div class="form-group">
          <label>Tahun Anggaran</label>
          <input 
            nz-input 
            formControlName="tahunAnggaran" 
            type="number" 
            min="2000" 
            max="2100" 
            placeholder="Masukkan tahun (contoh: 2026)"
            (input)="onTahunInput($event)" />
          <span class="error" *ngIf="uploadForm.get('tahunAnggaran')?.invalid && uploadForm.get('tahunAnggaran')?.touched">
            Tahun wajib diisi
          </span>
          <p style="font-size: 11px; color: #999; margin-top: 4px;">
            Current value: {{ uploadForm.get('tahunAnggaran')?.value || 'empty' }}
          </p>
        </div>

        <div class="form-group">
          <label>File CSV</label>
          <div style="margin-top: 8px;">
            <input 
              #fileInput 
              type="file" 
              accept=".csv" 
              (change)="onFileSelected($event)" 
              style="display: none;" />
            
            <button 
              type="button" 
              (click)="fileInput.click()"
              class="file-select-btn">
              📁 PILIH FILE CSV
            </button>
            
            <div id="fileInfoBox" style="margin-top: 12px; padding: 12px; background: #f5f5f5; border: 1px solid #d9d9d9; border-radius: 4px;">
              <p id="fileInfoText" style="margin: 0; font-weight: 500; font-size: 14px; color: #666;">
                * Belum ada file dipilih
              </p>
            </div>
          </div>
        </div>

        <div class="form-actions" style="margin-top: 20px;">
          <input 
            type="submit" 
            value="UPLOAD DATA"
            [disabled]="uploading || !selectedFile || uploadForm.invalid"
            (click)="debugClick()"
            style="background-color: #52c41a; color: white; padding: 15px 40px; border: none; border-radius: 4px; cursor: pointer; font-size: 16px; font-weight: bold; min-width: 200px;" />
          <p style="margin-top: 8px; font-size: 12px; color: #666;">
            Status: File={{ selectedFile?.name || 'null' }} | FormValid={{ uploadForm.valid }} | Disabled={{ (uploading || !selectedFile || uploadForm.invalid) }}
          </p>
        </div>
      </form>
    </div>

    <nz-table [nzData]="anggaranList" [nzFrontPagination]="false" [nzLoading]="uploading">
      <thead>
        <tr>
          <th>Tahun</th>
          <th>Revisi</th>
          <th>Jumlah Data</th>
          <th nzWidth="100px" nzAlign="center">Aksi</th>
        </tr>
      </thead>
      <tbody>
        <tr *ngFor="let item of anggaranList">
          <td>{{ item.tahunAnggaran }}</td>
          <td>{{ item.revisi }}</td>
          <td>{{ item.jumlah }}</td>
          <td nzAlign="center">
            <button nz-button nzType="text" nzSize="small" nz-tooltip="Lihat Detail" (click)="viewDetail(item.tahunAnggaran, item.revisi)">
              <span nz-icon nzType="eye"></span>
            </button>
          </td>
        </tr>
      </tbody>
    </nz-table>

    <nz-modal
      [(nzVisible)]="detailModalVisible"
      [nzTitle]="'Detail Anggaran - Tahun ' + selectedTahun + ' Revisi ' + selectedRevisi"
      [nzWidth]="'90%'"
      [nzFooter]="null"
      (nzOnCancel)="closeDetailModal()">
      <ng-container *nzModalContent>
        <div style="margin-bottom: 16px;">
          <input 
            nz-input 
            [value]="searchTerm"
            (input)="searchTerm = $any($event.target).value; filterDetails()" 
            placeholder="Cari program, kegiatan, komponen, atau akun..." />
        </div>
        
        <nz-table 
          [nzData]="filteredDetails" 
          [nzLoading]="loadingDetails"
          [nzScroll]="{ x: '1200px', y: '500px' }"
          [nzPageSize]="20">
          <thead>
            <tr>
              <th nzWidth="100px">Kode Program</th>
              <th nzWidth="100px">Kode Kegiatan</th>
              <th nzWidth="100px">Kode Output</th>
              <th nzWidth="100px">Kode Suboutput</th>
              <th nzWidth="100px">Kode Komponen</th>
              <th nzWidth="120px">Kode Subkomponen</th>
              <th nzWidth="100px">Kode Akun</th>
              <th nzWidth="100px">No Item</th>
              <th nzWidth="150px" nzAlign="right">Pagu</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let detail of filteredDetails">
              <td>{{ detail.kdProgram }}</td>
              <td>{{ detail.kdGiat }}</td>
              <td>{{ detail.kdOutput }}</td>
              <td>{{ detail.kdSOutput }}</td>
              <td>{{ detail.kdKmpnen }}</td>
              <td>{{ detail.kdSkmpnen }}</td>
              <td>{{ detail.kdAkun }}</td>
              <td>{{ detail.noItem }}</td>
              <td nzAlign="right">{{ detail.pagu | number: '1.0-0' }}</td>
            </tr>
          </tbody>
        </nz-table>
      </ng-container>
    </nz-modal>
  `,
  styles: [`
    .anggaran-list-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 24px;
    }

    .upload-form-container {
      background: #fafafa;
      border: 1px solid #f0f0f0;
      border-radius: 8px;
      padding: 24px;
      margin-bottom: 24px;
      box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
    }

    .form-group {
      margin-bottom: 16px;
    }

    .form-group label {
      display: block;
      margin-bottom: 8px;
      font-weight: 600;
      color: #262626;
    }

    .file-drop-zone {
      padding: 30px 20px;
      border: 3px dashed #1890ff;
      border-radius: 8px;
      text-align: center;
      cursor: pointer;
      background: #e6f7ff;
      transition: all 0.3s;
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 16px;
    }

    .file-drop-zone:hover {
      border-color: #0050b3;
      background: #bae7ff;
    }

    .file-drop-zone p {
      color: #262626;
      margin: 0;
      font-weight: 600;
    }

    .file-picker-area {
      margin-bottom: 12px;
    }

    .file-selected {
      padding: 12px;
      background: #e6f7ff;
      border-radius: 4px;
      border: 1px solid #91d5ff;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .file-selected p {
      margin: 0;
      color: #0050b3;
    }

    .error {
      color: #ff4d4f;
      font-size: 12px;
      margin-top: 4px;
      display: block;
    }

    .form-actions {
      margin-top: 20px;
      display: flex;
      gap: 8px;
    }

    .file-select-btn {
      background: #1890ff;
      color: white;
      padding: 10px 24px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      font-weight: 500;
      transition: all 0.3s;
      display: inline-block;
    }

    .file-select-btn:hover {
      background: #40a9ff;
    }

    .file-select-btn:active {
      background: #096dd9;
    }

    .file-selected-info {
      margin-top: 12px;
      padding: 12px;
      background: #f6ffed;
      border: 1px solid #b7eb8f;
      border-radius: 4px;
    }

    .btn-danger {
      background: #ff4d4f;
      color: white;
      padding: 6px 16px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 13px;
    }

    .btn-danger:hover {
      background: #ff7875;
    }

    .btn-upload {
      background: #52c41a;
      color: #ffffff !important;
      padding: 12px 32px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 15px;
      font-weight: 600;
      transition: all 0.3s;
      display: inline-block;
      min-width: 150px;
      text-align: center;
    }

    .btn-upload .btn-text {
      color: #ffffff !important;
      display: inline-block;
      font-weight: 600;
      font-size: 15px;
      letter-spacing: 0.5px;
    }

    .btn-upload:hover:not(:disabled):not(.btn-disabled) {
      background: #73d13d;
      transform: translateY(-1px);
      box-shadow: 0 4px 8px rgba(82, 196, 26, 0.3);
    }

    .btn-upload:active:not(:disabled):not(.btn-disabled) {
      background: #389e0d;
      transform: translateY(0);
    }

    .btn-upload:disabled,
    .btn-upload.btn-disabled {
      background: #d9d9d9 !important;
      cursor: not-allowed;
      opacity: 0.6;
      color: #8c8c8c !important;
    }

    .btn-upload:disabled .btn-text,
    .btn-upload.btn-disabled .btn-text {
      color: #8c8c8c !important;
    }
  `]
})
export class AnggaranListComponent {
  anggarans$: Observable<any[]>;
  anggaranList: any[] = [];
  uploadForm: FormGroup;
  uploading = false;
  selectedFile: File | null = null;
  showUploadForm = false;
  
  detailModalVisible = false;
  selectedTahun: number = 0;
  selectedRevisi: number = 0;
  anggaranDetails: any[] = [];
  filteredDetails: any[] = [];
  searchTerm: string = '';
  loadingDetails = false;

  constructor(
    private anggaranService: AnggaranMasterService,
    private fb: FormBuilder,
    private message: NzMessageService
  ) {
    this.uploadForm = this.fb.group({
      tahunAnggaran: ['', [Validators.required]]
    });
    this.anggarans$ = this.anggaranService.getAnggaranSummary().pipe(
      map(response => {
        console.log('Raw data from service:', response);
        // Handle response structure: {success: true, data: Array}
        const data = (response as any)?.data || response;
        if (Array.isArray(data)) {
          this.anggaranList = data;
          console.log('anggaranList set to:', this.anggaranList);
          return data;
        }
        this.anggaranList = [];
        return [];
      })
    );
    // Subscribe immediately
    this.anggarans$.subscribe();
  }

  onFileSelected(event: any) {
    console.log('=== onFileSelected triggered ===');
    
    const file = event.target.files[0];
    if (file) {
      console.log('File selected:', file.name, file.size, 'bytes');
      this.selectedFile = file;
      
      // Update DOM directly
      const fileInfoBox = document.getElementById('fileInfoBox');
      const fileInfoText = document.getElementById('fileInfoText');
      
      if (fileInfoBox && fileInfoText) {
        fileInfoBox.style.background = '#f6ffed';
        fileInfoBox.style.borderColor = '#b7eb8f';
        fileInfoText.style.color = '#52c41a';
        fileInfoText.innerHTML = `✓ File: <strong>${file.name}</strong> (${this.formatFileSize(file.size)})`;
        console.log('File info displayed in UI');
      }
    }
  }

  clearFile() {
    this.selectedFile = null;
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
    
    // Reset DOM
    const fileInfoBox = document.getElementById('fileInfoBox');
    const fileInfoText = document.getElementById('fileInfoText');
    if (fileInfoBox && fileInfoText) {
      fileInfoBox.style.background = '#f5f5f5';
      fileInfoBox.style.borderColor = '#d9d9d9';
      fileInfoText.style.color = '#666';
      fileInfoText.textContent = '* Belum ada file dipilih';
    }
    console.log('File cleared');
  }

  onUploadClick(event: Event) {
    console.log('=== Button clicked ===');
    console.log('Event:', event);
    console.log('selectedFile:', this.selectedFile);
    console.log('form valid:', this.uploadForm.valid);
    console.log('form value:', this.uploadForm.value);
    console.log('uploading:', this.uploading);
  }

  debugClick() {
    console.log('=== UPLOAD BUTTON CLICKED ===');
    console.log('selectedFile:', this.selectedFile);
    console.log('form valid:', this.uploadForm.valid);
    console.log('form value:', this.uploadForm.value);
    console.log('tahun value:', this.uploadForm.get('tahunAnggaran')?.value);
  }

  onTahunInput(event: any) {
    const value = event.target.value;
    console.log('Tahun input changed:', value);
    this.uploadForm.patchValue({ tahunAnggaran: value });
    console.log('Form value after patch:', this.uploadForm.value);
  }

  loadAnggaranData() {
    console.log('Reloading anggaran data...');
    this.anggaranService.getAnggaranSummary().subscribe({
      next: (response) => {
        console.log('Loaded anggaran data:', response);
        // Handle response structure: {success: true, data: Array}
        const data = (response as any)?.data || response;
        if (Array.isArray(data)) {
          this.anggaranList = data;
          console.log('anggaranList updated:', this.anggaranList);
        } else {
          this.anggaranList = [];
        }
      },
      error: (err) => {
        console.error('Error loading anggaran data:', err);
      }
    });
  }

  formatFileSize(bytes: number): string {
    if (bytes < 1024) return bytes + ' B';
    if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(2) + ' KB';
    return (bytes / (1024 * 1024)).toFixed(2) + ' MB';
  }

  submitUpload() {
    console.log('=== submitUpload called ===');
    console.log('Form valid:', this.uploadForm.valid);
    console.log('Form value:', this.uploadForm.value);
    console.log('Selected file:', this.selectedFile);
    console.log('Tahun:', this.uploadForm.get('tahunAnggaran')?.value);
    console.log('Uploading status:', this.uploading);
    console.log('Form errors:', this.uploadForm.errors);
    console.log('Tahun control errors:', this.uploadForm.get('tahunAnggaran')?.errors);
    
    if (this.uploadForm.invalid) {
      this.message.error('Tahun anggaran wajib diisi');
      console.log('Form validation failed - form invalid');
      return;
    }
    
    if (!this.selectedFile) {
      this.message.error('File CSV wajib dipilih');
      console.log('Validation failed - no file selected');
      return;
    }

    const tahunAnggaran = this.uploadForm.get('tahunAnggaran')?.value;
    console.log('Validation passed. Starting upload with tahun:', tahunAnggaran);
    
    this.uploading = true;

    this.anggaranService.uploadAnggaran(this.selectedFile, tahunAnggaran).subscribe({
      next: (response: any) => {
        console.log('Upload success:', response);
        this.message.success(`Upload berhasil - Revisi ${response.revisi} (${response.count} records)`);
        this.uploading = false;
        this.showUploadForm = false;
        this.uploadForm.reset();
        this.selectedFile = null;
        
        // Reset file info DOM
        const fileInfoBox = document.getElementById('fileInfoBox');
        const fileInfoText = document.getElementById('fileInfoText');
        if (fileInfoBox && fileInfoText) {
          fileInfoBox.style.background = '#f5f5f5';
          fileInfoBox.style.borderColor = '#d9d9d9';
          fileInfoText.style.color = '#666';
          fileInfoText.textContent = '* Belum ada file dipilih';
        }
        
        // Refresh data - force reload
        console.log('Reloading anggaran summary...');
        this.loadAnggaranData();
      },
      error: (err: any) => {
        console.log('Upload error:', err);
        console.log('Error details:', err.error);
        this.message.error(err.error?.message || 'Upload gagal');
        this.uploading = false;
      }
    });
  }

  viewDetail(tahun: number, revisi: number): void {
    this.selectedTahun = tahun;
    this.selectedRevisi = revisi;
    this.detailModalVisible = true;
    this.loadingDetails = true;
    this.searchTerm = '';
    
    this.anggaranService.getAnggaranDetail(tahun, revisi).subscribe({
      next: (response: any) => {
        const data = response?.data || response;
        this.anggaranDetails = Array.isArray(data) ? data : [];
        this.filteredDetails = this.anggaranDetails;
        this.loadingDetails = false;
        console.log('Loaded detail data:', this.anggaranDetails.length, 'records');
      },
      error: (err: any) => {
        console.error('Error loading detail:', err);
        this.message.error('Gagal memuat detail anggaran');
        this.loadingDetails = false;
      }
    });
  }

  closeDetailModal(): void {
    this.detailModalVisible = false;
    this.anggaranDetails = [];
    this.filteredDetails = [];
    this.searchTerm = '';
  }

  filterDetails(): void {
    if (!this.searchTerm.trim()) {
      this.filteredDetails = this.anggaranDetails;
      return;
    }
    
    const term = this.searchTerm.toLowerCase();
    this.filteredDetails = this.anggaranDetails.filter(detail => 
      detail.kdProgram?.toLowerCase().includes(term) ||
      detail.kdGiat?.toLowerCase().includes(term) ||
      detail.kdOutput?.toLowerCase().includes(term) ||
      detail.kdSOutput?.toLowerCase().includes(term) ||
      detail.kdKmpnen?.toLowerCase().includes(term) ||
      detail.kdSkmpnen?.toLowerCase().includes(term) ||
      detail.kdAkun?.toLowerCase().includes(term) ||
      detail.noItem?.toLowerCase().includes(term)
    );
  }
}
