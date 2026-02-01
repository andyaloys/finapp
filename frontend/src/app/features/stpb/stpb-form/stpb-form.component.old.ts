import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzDividerModule } from 'ng-zorro-antd/divider';

import { StpbService } from '../../../core/services/stpb.service';
import { AnggaranMasterService } from '../../../core/services/anggaran-master.service';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import {
  ProgramDto,
  KegiatanDto,
  OutputDto,
  SuboutputDto,
  KomponenDto,
  SubkomponenDto,
  AkunDto,
  ItemDto,
  AnggaranProgramDto,
  AnggaranKegiatanDto,
  AnggaranOutputDto,
  AnggaranSuboutputDto,
  AnggaranKomponenDto,
  AnggaranSubkomponenDto,
  AnggaranAkunDto,
  AnggaranItemDto
} from '../../../core/models/referensi.model';

@Component({
  selector: 'app-stpb-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzDatePickerModule,
    NzSelectModule,
    NzInputNumberModule,
    NzCardModule,
    NzIconModule,
    NzDividerModule,
    PageHeaderComponent
  ],
  templateUrl: './stpb-form.component.html',
  styleUrls: ['./stpb-form.component.scss']
})
export class StpbFormComponent implements OnInit {
  stpbForm: FormGroup;
  isLoading = false;
  isEditMode = false;
  stpbId: string | null = null;

  // Dropdown options
  programs: AnggaranProgramDto[] = [];
  kegiatans: AnggaranKegiatanDto[] = [];
  outputs: AnggaranOutputDto[] = [];
  suboutputs: AnggaranSuboutputDto[] = [];
  komponens: AnggaranKomponenDto[] = [];
  subkomponens: AnggaranSubkomponenDto[] = [];
  akuns: AnggaranAkunDto[] = [];
  items: AnggaranItemDto[] = [];
  tahunList: number[] = [];
  revisiList: number[] = [];
  tahunAnggaran: number = new Date().getFullYear();
  revisi: number = 0;

  constructor(
    private fb: FormBuilder,
    private stpbService: StpbService,
    private anggaranMasterService: AnggaranMasterService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.stpbForm = this.fb.group({
      tanggal: [new Date(), [Validators.required]],
      programId: [null, [Validators.required]],
      kegiatanId: [null, [Validators.required]],
      outputId: [null, [Validators.required]],
      suboutputId: [null, [Validators.required]],
      komponenId: [null, [Validators.required]],
      subkomponenId: [null, [Validators.required]],
      akunId: [null, [Validators.required]],
      itemId: [null],
      uraian: ['', [Validators.required]],
      nominal: [0, [Validators.required, Validators.min(1)]],
      ppn: [0, [Validators.min(0)]],
      pph21: [0, [Validators.min(0)]],
      pph22: [0, [Validators.min(0)]],
      pph23: [0, [Validators.min(0)]],
      nomorSTPB: ['']
    });
  }

  ngOnInit(): void {
    this.loadPrograms();
    
    // Setup cascading dropdowns
    this.setupCascading();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.stpbId = id;
      this.loadStpb(this.stpbId);
    }
  }

  setupCascading(): void {
    // When program changes, load kegiatans and reset downstream
    this.stpbForm.get('programId')?.valueChanges.subscribe(programId => {
      this.stpbForm.patchValue({
        kegiatanId: null,
        outputId: null,
        suboutputId: null,
        komponenId: null,
        subkomponenId: null,
        akunId: null,
        itemId: null
      });
      this.kegiatans = [];
      this.outputs = [];
      this.suboutputs = [];
      this.komponens = [];
      this.subkomponens = [];
      this.akuns = [];
      this.items = [];
      
      if (programId) {
        this.loadKegiatans(programId);
      }
    });

    // When kegiatan changes, load outputs
    this.stpbForm.get('kegiatanId')?.valueChanges.subscribe(kegiatanId => {
      this.stpbForm.patchValue({
        outputId: null,
        suboutputId: null,
        komponenId: null,
        subkomponenId: null,
        akunId: null,
        itemId: null
      });
      this.outputs = [];
      this.suboutputs = [];
      this.komponens = [];
      this.subkomponens = [];
      this.akuns = [];
      this.items = [];
      
      if (kegiatanId) {
        this.loadOutputs(kegiatanId);
      }
    });

    // When output changes, load suboutputs
    this.stpbForm.get('outputId')?.valueChanges.subscribe(outputId => {
      this.stpbForm.patchValue({
        suboutputId: null,
        komponenId: null,
        subkomponenId: null,
        akunId: null,
        itemId: null
      });
      this.suboutputs = [];
      this.komponens = [];
      this.subkomponens = [];
      this.akuns = [];
      this.items = [];
      
      if (outputId) {
        this.loadSuboutputs(outputId);
      }
    });

    // When suboutput changes, load komponens
    this.stpbForm.get('suboutputId')?.valueChanges.subscribe(suboutputId => {
      this.stpbForm.patchValue({
        komponenId: null,
        subkomponenId: null,
        akunId: null,
        itemId: null
      });
      this.komponens = [];
      this.subkomponens = [];
      this.akuns = [];
      this.items = [];
      
      if (suboutputId) {
        this.loadKomponens();
      }
    });

    // When komponen changes, load subkomponens
    this.stpbForm.get('komponenId')?.valueChanges.subscribe(komponenId => {
      this.stpbForm.patchValue({
        subkomponenId: null,
        akunId: null,
        itemId: null
      });
      this.subkomponens = [];
      this.akuns = [];
      this.items = [];
      
      if (komponenId) {
        this.loadSubkomponens();
      }
    });

    // When subkomponen changes, load akuns
    this.stpbForm.get('subkomponenId')?.valueChanges.subscribe(subkomponenId => {
      this.stpbForm.patchValue({
        akunId: null,
        itemId: null
      });
      this.akuns = [];
      this.items = [];
      
      if (subkomponenId) {
        this.loadAkuns();
      }
    });

    // When akun changes, load items
    this.stpbForm.get('akunId')?.valueChanges.subscribe(akunId => {
      this.stpbForm.patchValue({
        itemId: null
      });
      this.items = [];
      
      if (akunId) {
        this.loadItems();
      }
    });
  }

  loadPrograms(): void {
    this.anggaranMasterService.getDistinctPrograms(this.tahunAnggaran, this.revisi).subscribe({
      next: (response) => {
        this.programs = response?.data ?? [];
      },
      error: () => {
        this.message.error('Gagal memuat data program');
      }
    });
  }

  loadKegiatans(kdProgram: string): void {
    this.anggaranMasterService.getDistinctKegiatans(this.tahunAnggaran, this.revisi, kdProgram).subscribe({
      next: (response) => {
        this.kegiatans = response?.data ?? [];
      },
      error: () => {
        this.message.error('Gagal memuat data kegiatan');
      }
    });
  }

  loadOutputs(kdGiat: string): void {
    const kdProgram = this.stpbForm.get('programId')?.value;
    this.anggaranMasterService.getDistinctOutputs(this.tahunAnggaran, this.revisi, kdProgram, kdGiat).subscribe({
      next: (response) => {
        this.outputs = response?.data ?? [];
      },
      error: () => {
        this.message.error('Gagal memuat data output');
      }
    });
  }

  loadSuboutputs(kdSOutput: string): void {
    const kdProgram = this.stpbForm.get('programId')?.value;
    const kdGiat = this.stpbForm.get('kegiatanId')?.value;
    const kdOutput = this.stpbForm.get('outputId')?.value;
    this.anggaranMasterService.getDistinctSuboutputs(this.tahunAnggaran, this.revisi, kdProgram, kdGiat, kdOutput).subscribe({
      next: (response) => {
        this.suboutputs = response?.data ?? [];
      },
      error: () => {
        this.message.error('Gagal memuat data suboutput');
      }
    });
  }

  loadKomponens(): void {
    const kdProgram = this.stpbForm.get('programId')?.value;
    const kdGiat = this.stpbForm.get('kegiatanId')?.value;
    const kdOutput = this.stpbForm.get('outputId')?.value;
    const kdSOutput = this.stpbForm.get('suboutputId')?.value;
    console.log('Loading komponens with:', { kdProgram, kdGiat, kdOutput, kdSOutput });
    this.anggaranMasterService.getDistinctKomponens(this.tahunAnggaran, this.revisi, kdProgram, kdGiat, kdOutput, kdSOutput).subscribe({
      next: (response) => {
        console.log('Komponens response:', response);
        this.komponens = response?.data ?? [];
        console.log('Komponens array:', this.komponens);
      },
      error: () => {
        this.message.error('Gagal memuat data komponen');
      }
    });
  }

  loadSubkomponens(): void {
    const kdProgram = this.stpbForm.get('programId')?.value;
    const kdGiat = this.stpbForm.get('kegiatanId')?.value;
    const kdOutput = this.stpbForm.get('outputId')?.value;
    const kdSOutput = this.stpbForm.get('suboutputId')?.value;
    const kdKomponen = this.stpbForm.get('komponenId')?.value;
    console.log('Loading subkomponens with:', { kdProgram, kdGiat, kdOutput, kdSOutput, kdKomponen });
    this.anggaranMasterService.getDistinctSubkomponens(this.tahunAnggaran, this.revisi, kdProgram, kdGiat, kdOutput, kdSOutput, kdKomponen).subscribe({
      next: (response) => {
        console.log('Subkomponens response:', response);
        this.subkomponens = response?.data ?? [];
        console.log('Subkomponens array:', this.subkomponens);
      },
      error: () => {
        this.message.error('Gagal memuat data subkomponen');
      }
    });
  }

  loadAkuns(): void {
    const kdProgram = this.stpbForm.get('programId')?.value;
    const kdGiat = this.stpbForm.get('kegiatanId')?.value;
    const kdOutput = this.stpbForm.get('outputId')?.value;
    const kdSOutput = this.stpbForm.get('suboutputId')?.value;
    const kdKomponen = this.stpbForm.get('komponenId')?.value;
    const kdSubkomponen = this.stpbForm.get('subkomponenId')?.value;
    this.anggaranMasterService.getDistinctAkuns(this.tahunAnggaran, this.revisi, kdProgram, kdGiat, kdOutput, kdSOutput, kdKomponen, kdSubkomponen).subscribe({
      next: (response) => {
        this.akuns = response?.data ?? [];
      },
      error: () => {
        this.message.error('Gagal memuat data akun');
      }
    });
  }

  loadItems(): void {
    const kdProgram = this.stpbForm.get('programId')?.value;
    const kdGiat = this.stpbForm.get('kegiatanId')?.value;
    const kdOutput = this.stpbForm.get('outputId')?.value;
    const kdSOutput = this.stpbForm.get('suboutputId')?.value;
    const kdKomponen = this.stpbForm.get('komponenId')?.value;
    const kdSubkomponen = this.stpbForm.get('subkomponenId')?.value;
    const kdAkun = this.stpbForm.get('akunId')?.value;
    this.anggaranMasterService.getDistinctItems(this.tahunAnggaran, this.revisi, kdProgram, kdGiat, kdOutput, kdSOutput, kdKomponen, kdSubkomponen, kdAkun).subscribe({
      next: (response) => {
        this.items = response?.data ?? [];
      },
      error: () => {
        this.message.error('Gagal memuat data item');
      }
    });
  }

  loadStpb(id: string): void {
    this.isLoading = true;
    this.stpbService.getById(id).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          const data = response.data;
          
          // Load cascading data first, then patch values
          if (data.programId) {
            this.loadKegiatans(data.programId);
          }
          if (data.kegiatanId) {
            this.loadOutputs(data.kegiatanId);
          }
          if (data.outputId) {
            this.loadSuboutputs(data.outputId);
          }
          
          // Patch form values first before loading remaining cascade
          this.stpbForm.patchValue({
            tanggal: new Date(data.tanggal),
            programId: data.programId,
            kegiatanId: data.kegiatanId,
            outputId: data.outputId,
            suboutputId: data.suboutputId,
            komponenId: data.komponenId,
            subkomponenId: data.subkomponenId,
            akunId: data.akunId,
            itemId: data.itemId,
            uraian: data.uraian,
            nominal: data.nominal,
            ppn: data.ppn,
            pph21: data.pph21,
            pph22: data.pph22,
            pph23: data.pph23,
            nomorSTPB: data.nomorSTPB
          });
          
          // After patching, load remaining cascade data
          if (data.suboutputId) {
            this.loadKomponens();
          }
          if (data.komponenId) {
            this.loadSubkomponens();
          }
          if (data.subkomponenId) {
            this.loadAkuns();
          }
          if (data.akunId) {
            this.loadItems();
          }
        }
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.message.error('Gagal memuat data STPB');
        this.router.navigate(['/stpb']);
      }
    });
  }

  get nilaiBersih(): number {
    const nominal = this.stpbForm.get('nominal')?.value || 0;
    const ppn = this.stpbForm.get('ppn')?.value || 0;
    const pph21 = this.stpbForm.get('pph21')?.value || 0;
    const pph22 = this.stpbForm.get('pph22')?.value || 0;
    const pph23 = this.stpbForm.get('pph23')?.value || 0;
    
    return nominal - ppn - pph21 - pph22 - pph23;
  }

  onSubmit(): void {
    if (this.stpbForm.valid) {
      this.isLoading = true;
      const formValue = this.stpbForm.value;
      
      // Find selected item to get noItem and namaItem
      const selectedItem = this.items.find(item => item.noItem === formValue.itemId);
      
      const formData = {
        tanggal: formValue.tanggal,
        programId: formValue.programId,
        kegiatanId: formValue.kegiatanId,
        outputId: formValue.outputId,
        suboutputId: formValue.suboutputId,
        komponenId: formValue.komponenId,
        subkomponenId: formValue.subkomponenId,
        akunId: formValue.akunId,
        itemId: formValue.itemId,
        noItem: selectedItem?.noItem || null,
        namaItem: selectedItem?.nmItem || null,
        uraian: formValue.uraian,
        nominal: formValue.nominal,
        ppn: formValue.ppn || 0,
        pph21: formValue.pph21 || 0,
        pph22: formValue.pph22 || 0,
        pph23: formValue.pph23 || 0,
        nomorSTPB: '',
        nilaiTotal: this.nilaiBersih
      };
      
      console.log('Form data to submit:', formData);
      console.log('Nilai bersih:', this.nilaiBersih);

      const request = this.isEditMode && this.stpbId
        ? this.stpbService.update(this.stpbId, formData)
        : this.stpbService.create(formData);

      request.subscribe({
        next: () => {
          this.message.success(`STPB berhasil ${this.isEditMode ? 'diupdate' : 'dibuat'}`);
          this.router.navigate(['/stpb']);
        },
        error: () => {
          this.isLoading = false;
          this.message.error(`Gagal ${this.isEditMode ? 'mengupdate' : 'membuat'} STPB`);
        }
      });
    } else {
      Object.values(this.stpbForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/stpb']);
  }

  formatterIDR = (value: number): string => `Rp ${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  parserIDR = (value: string): string => value.replace(/Rp\s?|(,*)/g, '');
}
