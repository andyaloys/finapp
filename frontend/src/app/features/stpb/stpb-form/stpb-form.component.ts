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
import { ProgramService } from '../../../core/services/program.service';
import { KegiatanService } from '../../../core/services/kegiatan.service';
import { OutputService } from '../../../core/services/output.service';
import { SuboutputService } from '../../../core/services/suboutput.service';
import { KomponenService } from '../../../core/services/komponen.service';
import { SubkomponenService } from '../../../core/services/subkomponen.service';
import { AkunService } from '../../../core/services/akun.service';
import { ItemService } from '../../../core/services/item.service';
import { ProgramDto, KegiatanDto, OutputDto, SuboutputDto, KomponenDto, SubkomponenDto, AkunDto, ItemDto } from '../../../core/models/referensi.model';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

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
  programs: ProgramDto[] = [];
  kegiatans: KegiatanDto[] = [];
  outputs: OutputDto[] = [];
  suboutputs: SuboutputDto[] = [];
  komponens: KomponenDto[] = [];
  subkomponens: SubkomponenDto[] = [];
  akuns: AkunDto[] = [];
  items: ItemDto[] = [];

  constructor(
    private fb: FormBuilder,
    private stpbService: StpbService,
    private programService: ProgramService,
    private kegiatanService: KegiatanService,
    private outputService: OutputService,
    private suboutputService: SuboutputService,
    private komponenService: KomponenService,
    private subkomponenService: SubkomponenService,
    private akunService: AkunService,
    private itemService: ItemService,
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
        this.loadKomponens(suboutputId);
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
        this.loadSubkomponens(komponenId);
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
        this.loadAkuns(subkomponenId);
      }
    });

    // When akun changes, load items
    this.stpbForm.get('akunId')?.valueChanges.subscribe(akunId => {
      this.stpbForm.patchValue({
        itemId: null
      });
      this.items = [];
      
      if (akunId) {
        this.loadItems(akunId);
      }
    });
  }

  loadPrograms(): void {
    this.programService.getAll(1, 1000).subscribe({
      next: (response) => {
        if (response.success) {
          this.programs = response.data.items.filter((p: ProgramDto) => p.isActive);
        }
      },
      error: () => {
        this.message.error('Gagal memuat data program');
      }
    });
  }

  loadKegiatans(programId: string): void {
    this.kegiatanService.getByProgramId(programId).subscribe({
      next: (response) => {
        if (response.success) {
          this.kegiatans = response.data.filter(k => k.isActive);
        }
      },
      error: () => {
        this.message.error('Gagal memuat data kegiatan');
      }
    });
  }

  loadOutputs(kegiatanId: string): void {
    this.outputService.getByKegiatanId(kegiatanId).subscribe({
      next: (response) => {
        if (response.success) {
          this.outputs = response.data.filter(o => o.isActive);
        }
      },
      error: () => {
        this.message.error('Gagal memuat data output');
      }
    });
  }

  loadSuboutputs(outputId: string): void {
    this.suboutputService.getByOutputId(outputId).subscribe({
      next: (response) => {
        if (response.success) {
          this.suboutputs = response.data.filter(s => s.isActive);
        }
      },
      error: () => {
        this.message.error('Gagal memuat data suboutput');
      }
    });
  }

  loadKomponens(suboutputId: string): void {
    this.komponenService.getBySuboutputId(suboutputId).subscribe({
      next: (response) => {
        if (response.success) {
          this.komponens = response.data.filter(k => k.isActive);
        }
      },
      error: () => {
        this.message.error('Gagal memuat data komponen');
      }
    });
  }

  loadSubkomponens(komponenId: string): void {
    console.log('Loading subkomponens for komponenId:', komponenId);
    this.subkomponenService.getByKomponenId(komponenId).subscribe({
      next: (response) => {
        console.log('Subkomponen response:', response);
        if (response.success) {
          this.subkomponens = response.data.filter(s => s.isActive);
          console.log('Filtered subkomponens:', this.subkomponens);
        }
      },
      error: (err) => {
        console.error('Error loading subkomponens:', err);
        this.message.error('Gagal memuat data subkomponen');
      }
    });
  }

  loadAkuns(subkomponenId: string): void {
    this.akunService.getBySubkomponenId(subkomponenId).subscribe({
      next: (response) => {
        if (response.success) {
          this.akuns = response.data.filter(a => a.isActive);
        }
      },
      error: () => {
        this.message.error('Gagal memuat data akun');
      }
    });
  }

  loadItems(akunId: string): void {
    this.itemService.getByAkunId(akunId).subscribe({
      next: (response) => {
        if (response.success) {
          this.items = response.data.filter(i => i.isActive);
        }
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
          if (data.suboutputId) {
            this.loadKomponens(data.suboutputId);
          }
          if (data.komponenId) {
            this.loadSubkomponens(data.komponenId);
          }
          if (data.subkomponenId) {
            this.loadAkuns(data.subkomponenId);
          }
          if (data.akunId) {
            this.loadItems(data.akunId);
          }
          
          // Patch form values
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
      const formData = {
        ...this.stpbForm.value,
        nilaiTotal: this.nilaiBersih,
        deskripsi: this.stpbForm.value.uraian
      };

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
