import { Component, OnInit, OnChanges, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzMessageService } from 'ng-zorro-antd/message';

import { AnggaranMasterService } from '../../../core/services/anggaran-master.service';
import { StpbService } from '../../../core/services/stpb.service';
import { PenerimaService } from '../../../core/services/penerima.service';
import { TaxRateService } from '../../../services/taxrate.service';
import { TaxRateDto } from '../../../models/taxrate.model';
import { CreateStpbDetailDto, StpbDetailDto } from '../../../core/models/stpb-detail.model';
import { Penerima } from '../../../core/models/penerima.model';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';

@Component({
  selector: 'app-stpb-detail-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzInputNumberModule,
    NzButtonModule,
    NzDividerModule,
    NzAlertModule,
    NzDatePickerModule,
    NzCheckboxModule
  ],
  templateUrl: './stpb-detail-modal.component.html',
  styleUrls: ['./stpb-detail-modal.component.scss']
})
export class StpbDetailModalComponent implements OnInit, OnChanges {
  @Input() visible = false;
  @Input() stpbId: string | null = null;
  @Input() detail: StpbDetailDto | null = null;
  @Input() existingDetails: any[] = [];
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() onSuccess = new EventEmitter<void>();

  detailForm: FormGroup;
  isLoading = false;
  isEditMode = false;

  // Cascade data
  programs: any[] = [];
  kegiatans: any[] = [];
  outputs: any[] = [];
  suboutputs: any[] = [];
  komponens: any[] = [];
  subkomponens: any[] = [];
  akuns: any[] = [];
  items: any[] = [];
  suppliers: Penerima[] = [];
  
  // Tax rates - all available rates per type
  ppnRates: TaxRateDto[] = [];
  pph21Rates: TaxRateDto[] = [];
  pph22Rates: TaxRateDto[] = [];
  pph23Rates: TaxRateDto[] = [];
  
  // Selected tax rate IDs
  selectedTaxRateIds = { ppn: null as string | null, pph21: null as string | null, pph22: null as string | null, pph23: null as string | null };
  
  // Tax enabled flags (checkbox state)
  taxEnabled = { ppn: false, pph21: false, pph22: false, pph23: false };
  
  // Calculated tax values
  calculatedTaxes = { ppn: 0, pph21: 0, pph22: 0, pph23: 0 };
  manualEditFlags = { ppn: false, pph21: false, pph22: false, pph23: false };
  
  // Pagu info
  paguInfo: any = null;
  loadingPagu = false;
  
  tahun: number = new Date().getFullYear();
  revisi: number = 0;
  anggaranData: any[] = [];

  constructor(
    private fb: FormBuilder,
    private anggaranMasterService: AnggaranMasterService,
    private stpbService: StpbService,
    private penerimaService: PenerimaService,
    private taxRateService: TaxRateService,
    private message: NzMessageService
  ) {
    this.detailForm = this.fb.group({
      tanggalTransaksi: [new Date(), [Validators.required]],
      kodeProgram: [null, [Validators.required]],
      kodeKegiatan: [null, [Validators.required]],
      kodeOutput: [null, [Validators.required]],
      kodeSuboutput: [null, [Validators.required]],
      kodeKomponen: [null, [Validators.required]],
      kodeSubkomponen: [null, [Validators.required]],
      kodeAkun: [null, [Validators.required]],
      kodeItem: [null, [Validators.required]],
      uraian: ['', [Validators.required]],
      nilaiTransaksi: [0, [Validators.required, Validators.min(1)]],
      penerimaId: [null],
      ppn: [0, [Validators.min(0)]],
      pph21: [0, [Validators.min(0)]],
      pph22: [0, [Validators.min(0)]],
      pph23: [0, [Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    // Determine tahun and revisi
    this.tahun = new Date().getFullYear();
    this.revisi = 0;
    
    this.loadAnggaranData();
    this.loadSuppliers();
    this.loadTaxRates();
    this.setupCascading();
    this.setupTaxCalculation();
    
    if (this.detail) {
      this.isEditMode = true;
      // populateForm akan dipanggil dari loadPrograms() setelah data loaded
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Saat modal dibuka (visible berubah dari false ke true)
    if (changes['visible'] && changes['visible'].currentValue === true) {
      console.log('Modal opened, existingDetails count:', this.existingDetails?.length || 0);
      console.log('Is edit mode:', !!this.detail);
      console.log('Existing details:', this.existingDetails);
      console.log('Detail to edit:', this.detail);
      
      // Set edit mode berdasarkan apakah ada detail yang di-pass
      this.isEditMode = !!this.detail;
      
      // Reset form saat modal dibuka (jika bukan edit mode)
      if (!this.detail) {
        this.detailForm.reset({
          tanggalTransaksi: new Date(),
          nilaiTransaksi: 0,
          ppn: 0,
          pph21: 0,
          pph22: 0,
          pph23: 0
        });
        
        // Jika ada detail sebelumnya, prefill setelah data loaded
        if (this.existingDetails && this.existingDetails.length > 0) {
          console.log('Scheduling prefill from existing detail');
          // Tunggu sedikit untuk memastikan programs sudah loaded
          setTimeout(() => {
            if (this.programs.length > 0) {
              this.prefillFromExisting();
            } else {
              // Jika programs belum loaded, tunggu lebih lama
              setTimeout(() => this.prefillFromExisting(), 500);
            }
          }, 300);
        }
      } else {
        // Edit mode - populate form jika anggaran data sudah loaded
        console.log('Edit mode detected in ngOnChanges');
        if (this.programs.length > 0) {
          console.log('Programs already loaded, populating now');
          setTimeout(() => this.populateForm(), 100);
        } else {
          console.log('Programs not loaded yet, waiting...');
        }
      }
    }
  }

  loadAnggaranData(): void {
    console.log('Loading anggaran data with tahun:', this.tahun, 'revisi:', this.revisi);
    this.anggaranMasterService.getAnggaranDetail(this.tahun, this.revisi).subscribe({
      next: (response: any) => {
        console.log('Anggaran response:', response);
        if (response.success && response.data) {
          this.anggaranData = response.data;
          console.log('Anggaran data loaded:', this.anggaranData.length, 'items');
          this.loadPrograms();
        } else {
          console.warn('No anggaran data found');
          this.message.warning('Data anggaran tidak ditemukan untuk tahun ' + this.tahun);
        }
      },
      error: (error: any) => {
        console.error('Error loading anggaran data:', error);
        this.message.error('Gagal memuat data anggaran');
      }
    });
  }

  loadSuppliers(): void {
    this.penerimaService.getAllActive().subscribe({
      next: (response) => {
        if (response.success) {
          this.suppliers = response.data;
        }
      },
      error: () => {
        this.message.error('Gagal memuat data supplier');
      }
    });
  }

  loadTaxRates(): void {
    // Load all active tax rates for each type
    this.taxRateService.getByTaxType('PPN').subscribe({
      next: (rates) => {
        this.ppnRates = rates.sort((a, b) => a.displayOrder - b.displayOrder);
      },
      error: () => this.message.error('Gagal memuat tarif PPN')
    });

    this.taxRateService.getByTaxType('PPH21').subscribe({
      next: (rates) => {
        this.pph21Rates = rates.sort((a, b) => a.displayOrder - b.displayOrder);
      },
      error: () => this.message.error('Gagal memuat tarif PPH 21')
    });

    this.taxRateService.getByTaxType('PPH22').subscribe({
      next: (rates) => {
        this.pph22Rates = rates.sort((a, b) => a.displayOrder - b.displayOrder);
      },
      error: () => this.message.error('Gagal memuat tarif PPH 22')
    });

    this.taxRateService.getByTaxType('PPH23').subscribe({
      next: (rates) => {
        this.pph23Rates = rates.sort((a, b) => a.displayOrder - b.displayOrder);
      },
      error: () => this.message.error('Gagal memuat tarif PPH 23')
    });
  }

  setupTaxCalculation(): void {
    // Watch for volume/harga changes
    this.detailForm.get('nilaiTransaksi')?.valueChanges.subscribe(() => {
      this.calculateTaxes();
    });
  }

  calculateTaxes(): void {
    const nilaiTransaksi = this.detailForm.value.nilaiTransaksi || 0;
    
    // Calculate each tax if enabled and not manually edited
    if (!this.manualEditFlags.ppn && this.taxEnabled.ppn && this.selectedTaxRateIds.ppn) {
      const selectedRate = this.ppnRates.find(r => r.id === this.selectedTaxRateIds.ppn);
      if (selectedRate) {
        // PPN = (((100/111) × Nilai Transaksi) × (11/12)) × Rate
        this.calculatedTaxes.ppn = Math.round(((100/111) * nilaiTransaksi * (11/12)) * (selectedRate.rate / 100));
      }
    } else if (!this.taxEnabled.ppn) {
      this.calculatedTaxes.ppn = 0;
    }
      
    if (!this.manualEditFlags.pph21 && this.taxEnabled.pph21 && this.selectedTaxRateIds.pph21) {
      const selectedRate = this.pph21Rates.find(r => r.id === this.selectedTaxRateIds.pph21);
      if (selectedRate) {
        this.calculatedTaxes.pph21 = Math.round(nilaiTransaksi * (selectedRate.rate / 100));
      }
    } else if (!this.taxEnabled.pph21) {
      this.calculatedTaxes.pph21 = 0;
    }
      
    if (!this.manualEditFlags.pph22 && this.taxEnabled.pph22 && this.selectedTaxRateIds.pph22) {
      const selectedRate = this.pph22Rates.find(r => r.id === this.selectedTaxRateIds.pph22);
      if (selectedRate) {
        // PPH 22 = (100/111) * Nilai Transaksi * Rate
        this.calculatedTaxes.pph22 = Math.round((100/111) * nilaiTransaksi * (selectedRate.rate / 100));
      }
    } else if (!this.taxEnabled.pph22) {
      this.calculatedTaxes.pph22 = 0;
    }
      
    if (!this.manualEditFlags.pph23 && this.taxEnabled.pph23 && this.selectedTaxRateIds.pph23) {
      const selectedRate = this.pph23Rates.find(r => r.id === this.selectedTaxRateIds.pph23);
      if (selectedRate) {
        // PPH 23 = (100/111) * Nilai Transaksi * Rate
        this.calculatedTaxes.pph23 = Math.round((100/111) * nilaiTransaksi * (selectedRate.rate / 100));
      }
    } else if (!this.taxEnabled.pph23) {
      this.calculatedTaxes.pph23 = 0;
    }

    // Update form values (for submission)
    this.detailForm.patchValue({
      ppn: this.calculatedTaxes.ppn,
      pph21: this.calculatedTaxes.pph21,
      pph22: this.calculatedTaxes.pph22,
      pph23: this.calculatedTaxes.pph23
    }, { emitEvent: false });
  }

  onTaxRateChange(taxType: 'ppn' | 'pph21' | 'pph22' | 'pph23'): void {
    // Reset manual edit flag when tax rate changes
    this.manualEditFlags[taxType] = false;
    // Recalculate taxes
    this.calculateTaxes();
  }

  onTaxCheckboxChange(taxType: 'ppn' | 'pph21' | 'pph22' | 'pph23', checked: boolean): void {
    this.taxEnabled[taxType] = checked;
    
    if (!checked) {
      // If unchecked, clear selection and value
      this.selectedTaxRateIds[taxType] = null;
      this.calculatedTaxes[taxType] = 0;
      this.manualEditFlags[taxType] = false;
    } else {
      // If checked, auto-select default rate if available
      const rates = taxType === 'ppn' ? this.ppnRates :
                    taxType === 'pph21' ? this.pph21Rates :
                    taxType === 'pph22' ? this.pph22Rates :
                    this.pph23Rates;
      
      const defaultRate = rates.find(r => r.isDefault);
      if (defaultRate) {
        this.selectedTaxRateIds[taxType] = defaultRate.id;
      }
      this.manualEditFlags[taxType] = false;
    }
    
    this.calculateTaxes();
  }

  onTaxManualEdit(taxType: 'ppn' | 'pph21' | 'pph22' | 'pph23', value: number): void {
    // Mark as manually edited
    this.manualEditFlags[taxType] = true;
    this.calculatedTaxes[taxType] = value || 0;
    
    // Update form value
    this.detailForm.patchValue({
      [taxType]: value || 0
    }, { emitEvent: false });
  }

  getTotalPajak(): number {
    return this.calculatedTaxes.ppn 
         + this.calculatedTaxes.pph21 
         + this.calculatedTaxes.pph22 
         + this.calculatedTaxes.pph23;
  }

  getNilaiBersih(): number {
    const nilaiTransaksi = this.detailForm.value.nilaiTransaksi || 0;
    return nilaiTransaksi - this.getTotalPajak();
  }

  loadPrograms(): void {
    const uniquePrograms = new Map();
    this.anggaranData.forEach((item: any) => {
      const kd = item.kdProgram || item.kodeProgram;
      if (kd && !uniquePrograms.has(kd)) {
        uniquePrograms.set(kd, {
          kodeProgram: kd,
          namaProgram: kd  // API doesn't return name, use code
        });
      }
    });
    this.programs = Array.from(uniquePrograms.values());
    console.log('Programs loaded:', this.programs.length, 'programs');
    
    // Handle edit mode - populate form after data loaded
    if (this.isEditMode && this.detail) {
      console.log('Edit mode: populating form with detail data');
      setTimeout(() => this.populateForm(), 100);
    }
  }

  loadKegiatans(kodeProgram: string): void {
    const uniqueKegiatans = new Map();
    this.anggaranData
      .filter((item: any) => (item.kdProgram || item.kodeProgram) === kodeProgram)
      .forEach((item: any) => {
        const kd = item.kdGiat || item.kodeKegiatan;
        if (kd && !uniqueKegiatans.has(kd)) {
          uniqueKegiatans.set(kd, {
            kodeKegiatan: kd,
            namaKegiatan: kd
          });
        }
      });
    this.kegiatans = Array.from(uniqueKegiatans.values());
  }

  loadOutputs(kodeKegiatan: string): void {
    const kodeProgram = this.detailForm.get('kodeProgram')?.value;
    const uniqueOutputs = new Map();
    this.anggaranData
      .filter((item: any) => 
        (item.kdProgram || item.kodeProgram) === kodeProgram && 
        (item.kdGiat || item.kodeKegiatan) === kodeKegiatan
      )
      .forEach((item: any) => {
        const kd = item.kdOutput || item.kodeOutput;
        if (kd && !uniqueOutputs.has(kd)) {
          uniqueOutputs.set(kd, {
            kodeOutput: kd,
            namaOutput: kd
          });
        }
      });
    this.outputs = Array.from(uniqueOutputs.values());
  }

  loadSuboutputs(kodeOutput: string): void {
    const kodeProgram = this.detailForm.get('kodeProgram')?.value;
    const kodeKegiatan = this.detailForm.get('kodeKegiatan')?.value;
    const uniqueSuboutputs = new Map();
    this.anggaranData
      .filter((item: any) => 
        (item.kdProgram || item.kodeProgram) === kodeProgram && 
        (item.kdGiat || item.kodeKegiatan) === kodeKegiatan && 
        (item.kdOutput || item.kodeOutput) === kodeOutput
      )
      .forEach((item: any) => {
        const kd = item.kdSOutput || item.kodeSuboutput;
        if (kd && !uniqueSuboutputs.has(kd)) {
          uniqueSuboutputs.set(kd, {
            kodeSuboutput: kd,
            namaSuboutput: kd
          });
        }
      });
    this.suboutputs = Array.from(uniqueSuboutputs.values());
  }

  loadKomponens(kodeSuboutput: string): void {
    const kodeProgram = this.detailForm.get('kodeProgram')?.value;
    const kodeKegiatan = this.detailForm.get('kodeKegiatan')?.value;
    const kodeOutput = this.detailForm.get('kodeOutput')?.value;
    const uniqueKomponens = new Map();
    this.anggaranData
      .filter((item: any) => 
        (item.kdProgram || item.kodeProgram) === kodeProgram && 
        (item.kdGiat || item.kodeKegiatan) === kodeKegiatan && 
        (item.kdOutput || item.kodeOutput) === kodeOutput &&
        (item.kdSOutput || item.kodeSuboutput) === kodeSuboutput
      )
      .forEach((item: any) => {
        const kd = item.kdKmpnen || item.kodeKomponen;
        if (kd && !uniqueKomponens.has(kd)) {
          uniqueKomponens.set(kd, {
            kodeKomponen: kd,
            namaKomponen: kd
          });
        }
      });
    this.komponens = Array.from(uniqueKomponens.values());
  }

  loadSubkomponens(kodeKomponen: string): void {
    const kodeProgram = this.detailForm.get('kodeProgram')?.value;
    const kodeKegiatan = this.detailForm.get('kodeKegiatan')?.value;
    const kodeOutput = this.detailForm.get('kodeOutput')?.value;
    const kodeSuboutput = this.detailForm.get('kodeSuboutput')?.value;
    const uniqueSubkomponens = new Map();
    this.anggaranData
      .filter((item: any) => 
        (item.kdProgram || item.kodeProgram) === kodeProgram && 
        (item.kdGiat || item.kodeKegiatan) === kodeKegiatan && 
        (item.kdOutput || item.kodeOutput) === kodeOutput &&
        (item.kdSOutput || item.kodeSuboutput) === kodeSuboutput &&
        (item.kdKmpnen || item.kodeKomponen) === kodeKomponen
      )
      .forEach((item: any) => {
        const kd = item.kdSkmpnen || item.kodeSubkomponen;
        if (kd && !uniqueSubkomponens.has(kd)) {
          uniqueSubkomponens.set(kd, {
            kodeSubkomponen: kd,
            namaSubkomponen: kd
          });
        }
      });
    this.subkomponens = Array.from(uniqueSubkomponens.values());
  }

  loadAkuns(kodeSubkomponen: string): void {
    const kodeProgram = this.detailForm.get('kodeProgram')?.value;
    const kodeKegiatan = this.detailForm.get('kodeKegiatan')?.value;
    const kodeOutput = this.detailForm.get('kodeOutput')?.value;
    const kodeSuboutput = this.detailForm.get('kodeSuboutput')?.value;
    const kodeKomponen = this.detailForm.get('kodeKomponen')?.value;
    const uniqueAkuns = new Map();
    this.anggaranData
      .filter((item: any) => 
        (item.kdProgram || item.kodeProgram) === kodeProgram && 
        (item.kdGiat || item.kodeKegiatan) === kodeKegiatan && 
        (item.kdOutput || item.kodeOutput) === kodeOutput &&
        (item.kdSOutput || item.kodeSuboutput) === kodeSuboutput &&
        (item.kdKmpnen || item.kodeKomponen) === kodeKomponen &&
        (item.kdSkmpnen || item.kodeSubkomponen) === kodeSubkomponen
      )
      .forEach((item: any) => {
        const kd = item.kdAkun || item.kodeAkun;
        if (kd && !uniqueAkuns.has(kd)) {
          uniqueAkuns.set(kd, {
            kodeAkun: kd,
            namaAkun: kd
          });
        }
      });
    this.akuns = Array.from(uniqueAkuns.values());
  }

  loadItems(kodeAkun: string): void {
    const kodeProgram = this.detailForm.get('kodeProgram')?.value;
    const kodeKegiatan = this.detailForm.get('kodeKegiatan')?.value;
    const kodeOutput = this.detailForm.get('kodeOutput')?.value;
    const kodeSuboutput = this.detailForm.get('kodeSuboutput')?.value;
    const kodeKomponen = this.detailForm.get('kodeKomponen')?.value;
    const kodeSubkomponen = this.detailForm.get('kodeSubkomponen')?.value;
    
    const filtered = this.anggaranData.filter((item: any) => 
      (item.kdProgram || item.kodeProgram) === kodeProgram && 
      (item.kdGiat || item.kodeKegiatan) === kodeKegiatan && 
      (item.kdOutput || item.kodeOutput) === kodeOutput &&
      (item.kdSOutput || item.kodeSuboutput) === kodeSuboutput &&
      (item.kdKmpnen || item.kodeKomponen) === kodeKomponen &&
      (item.kdSkmpnen || item.kodeSubkomponen) === kodeSubkomponen &&
      (item.kdAkun || item.kodeAkun) === kodeAkun
    );
    
    this.items = filtered.map((item: any) => ({
        kodeItem: item.noItem || item.kodeItem,
        namaItem: item.nmItem || item.namaItem || `Item ${item.noItem}`,
        satuan: item.satuan || 'unit',
        hargaSatuan: item.hargaSat || 0
      }));
  }

  setupCascading(): void {
    // Program → Kegiatan
    this.detailForm.get('kodeProgram')?.valueChanges.subscribe(value => {
      if (value) {
        this.detailForm.patchValue({
          kodeKegiatan: null,
          kodeOutput: null,
          kodeSuboutput: null,
          kodeKomponen: null,
          kodeSubkomponen: null,
          kodeAkun: null,
          kodeItem: null
        });
        this.kegiatans = [];
        this.outputs = [];
        this.suboutputs = [];
        this.komponens = [];
        this.subkomponens = [];
        this.akuns = [];
        this.items = [];
        this.loadKegiatans(value);
      }
    });

    // Kegiatan → Output
    this.detailForm.get('kodeKegiatan')?.valueChanges.subscribe(value => {
      if (value) {
        this.detailForm.patchValue({
          kodeOutput: null,
          kodeSuboutput: null,
          kodeKomponen: null,
          kodeSubkomponen: null,
          kodeAkun: null,
          kodeItem: null
        });
        this.outputs = [];
        this.suboutputs = [];
        this.komponens = [];
        this.subkomponens = [];
        this.akuns = [];
        this.items = [];
        this.loadOutputs(value);
      }
    });

    // Output → Suboutput
    this.detailForm.get('kodeOutput')?.valueChanges.subscribe(value => {
      if (value) {
        this.detailForm.patchValue({
          kodeSuboutput: null,
          kodeKomponen: null,
          kodeSubkomponen: null,
          kodeAkun: null,
          kodeItem: null
        });
        this.suboutputs = [];
        this.komponens = [];
        this.subkomponens = [];
        this.akuns = [];
        this.items = [];
        this.loadSuboutputs(value);
      }
    });

    // Suboutput → Komponen
    this.detailForm.get('kodeSuboutput')?.valueChanges.subscribe(value => {
      if (value) {
        this.detailForm.patchValue({
          kodeKomponen: null,
          kodeSubkomponen: null,
          kodeAkun: null,
          kodeItem: null
        });
        this.komponens = [];
        this.subkomponens = [];
        this.akuns = [];
        this.items = [];
        this.loadKomponens(value);
      }
    });

    // Komponen → Subkomponen
    this.detailForm.get('kodeKomponen')?.valueChanges.subscribe(value => {
      if (value) {
        this.detailForm.patchValue({
          kodeSubkomponen: null,
          kodeAkun: null,
          kodeItem: null
        });
        this.subkomponens = [];
        this.akuns = [];
        this.items = [];
        this.loadSubkomponens(value);
      }
    });

    // Subkomponen → Akun
    this.detailForm.get('kodeSubkomponen')?.valueChanges.subscribe(value => {
      if (value) {
        this.detailForm.patchValue({
          kodeAkun: null,
          kodeItem: null
        });
        this.akuns = [];
        this.items = [];
        this.loadAkuns(value);
      }
    });

    // Akun → Item
    this.detailForm.get('kodeAkun')?.valueChanges.subscribe(value => {
      if (value) {
        this.detailForm.patchValue({
          kodeItem: null
        });
        this.items = [];
        this.loadItems(value);
      }
    });

    // Item → Auto-fill & Load Pagu
    this.detailForm.get('kodeItem')?.valueChanges.subscribe(value => {
      if (value) {
        const selectedItem = this.items.find(item => item.kodeItem === value);
        if (selectedItem && !this.isEditMode) {
          this.detailForm.patchValue({
            uraian: selectedItem.namaItem || '',
            satuan: selectedItem.satuan || '',
            hargaSatuan: selectedItem.hargaSatuan || 0
          });
        }
        // Load pagu info
        this.loadPaguInfo();
      }
    });
    
    // NilaiTransaksi → Check Pagu Real-time
    this.detailForm.get('nilaiTransaksi')?.valueChanges.subscribe(() => this.checkPaguRealtime());
  }

  populateForm(): void {
    if (this.detail) {
      console.log('Populating form with detail:', this.detail);
      console.log('Detail structure check:', {
        kodeProgram: this.detail.kodeProgram,
        kodeKegiatan: this.detail.kodeKegiatan,
        kodeOutput: this.detail.kodeOutput,
        kodeSuboutput: this.detail.kodeSuboutput,
        kodeKomponen: this.detail.kodeKomponen,
        kodeSubkomponen: this.detail.kodeSubkomponen,
        kodeAkun: this.detail.kodeAkun,
        noItem: (this.detail as any).noItem,
        ppn: (this.detail as any).ppn,
        pph21: (this.detail as any).pph21,
        pph22: (this.detail as any).pph22,
        pph23: (this.detail as any).pph23
      });
      
      // Load cascade data based on detail - dengan timing yang lebih panjang
      this.loadKegiatans(this.detail.kodeProgram);
      setTimeout(() => {
        console.log('Kegiatans loaded for edit:', this.kegiatans.length);
        this.loadOutputs(this.detail!.kodeKegiatan);
      }, 150);
      
      setTimeout(() => {
        console.log('Outputs loaded for edit:', this.outputs.length);
        this.loadSuboutputs(this.detail!.kodeOutput);
      }, 300);
      
      setTimeout(() => {
        console.log('Suboutputs loaded for edit:', this.suboutputs.length);
        this.loadKomponens(this.detail!.kodeSuboutput);
      }, 450);
      
      setTimeout(() => {
        console.log('Komponens loaded for edit:', this.komponens.length);
        this.loadSubkomponens(this.detail!.kodeKomponen);
      }, 600);
      
      setTimeout(() => {
        console.log('Subkomponens loaded for edit:', this.subkomponens.length);
        this.loadAkuns(this.detail!.kodeSubkomponen);
      }, 750);
      
      setTimeout(() => {
        console.log('Akuns loaded for edit:', this.akuns.length);
        this.loadItems(this.detail!.kodeAkun);
      }, 900);

      // Populate form setelah semua cascade data loaded - tunggu items loaded juga
      setTimeout(() => {
        console.log('Items loaded for edit:', this.items.length);
        console.log('Setting form values for edit');
        const detailAny = this.detail as any;
        
        this.detailForm.patchValue({
          kodeProgram: this.detail!.kodeProgram,
          kodeKegiatan: this.detail!.kodeKegiatan,
          kodeOutput: this.detail!.kodeOutput,
          kodeSuboutput: this.detail!.kodeSuboutput,
          kodeKomponen: this.detail!.kodeKomponen,
          kodeSubkomponen: this.detail!.kodeSubkomponen,
          kodeAkun: this.detail!.kodeAkun,
          kodeItem: detailAny.noItem || null,
          uraian: detailAny.keterangan || '',
          nilaiTransaksi: detailAny.hargaSatuan || 0,
          penerimaId: detailAny.penerimaId || null,
          ppn: detailAny.ppn ?? 0,
          pph21: detailAny.ppH21 ?? 0,
          pph22: detailAny.ppH22 ?? 0,
          pph23: detailAny.ppH23 ?? 0
        });
        console.log('Form values after patch:', this.detailForm.value);
        console.log('Form populated successfully for edit');
      }, 1200);
    }
  }

  prefillFromExisting(): void {
    // Auto-fill Program, Kegiatan, Output from first detail
    const firstDetail = this.existingDetails[0];
    console.log('Prefilling form with:', firstDetail);
    
    if (firstDetail) {
      // Step 1: Set Program
      this.detailForm.patchValue({
        kodeProgram: firstDetail.kodeProgram
      });
      console.log('Step 1: Program set to', firstDetail.kodeProgram);
      
      // Step 2: Load Kegiatans dan set Kegiatan
      this.loadKegiatans(firstDetail.kodeProgram);
      setTimeout(() => {
        console.log('Step 2: Kegiatans loaded, count:', this.kegiatans.length);
        this.detailForm.patchValue({
          kodeKegiatan: firstDetail.kodeKegiatan
        });
        console.log('Step 2: Kegiatan set to', firstDetail.kodeKegiatan);
        
        // Step 3: Load Outputs dan set Output
        this.loadOutputs(firstDetail.kodeKegiatan);
        setTimeout(() => {
          console.log('Step 3: Outputs loaded, count:', this.outputs.length);
          this.detailForm.patchValue({
            kodeOutput: firstDetail.kodeOutput
          });
          console.log('Step 3: Output set to', firstDetail.kodeOutput);
        }, 200);
      }, 200);
    }
  }

  formatRupiah = (value: number | null): string => {
    if (value === null || value === undefined) return 'Rp 0';
    return 'Rp ' + value.toLocaleString('id-ID');
  }

  parseRupiah = (value: string): string => {
    return value.replace(/Rp\s?|[^\d]/g, '');
  }

  formatNumber = (value: number | null): string => {
    if (value === null || value === undefined) return '0';
    return value.toLocaleString('id-ID');
  }

  parseNumber = (value: string): string => {
    return value.replace(/[^\d]/g, '');
  }

  handleOk(): void {
    if (this.detailForm.valid && this.isPaguValid && this.stpbId) {
      this.isLoading = true;

      const formValue = this.detailForm.value;
      
      // Get selected item details to include names
      const selectedItem = this.items.find(i => i.kodeItem === formValue.kodeItem);
      const selectedProgram = this.programs.find(p => p.kdProgram === formValue.kodeProgram);
      const selectedKegiatan = this.kegiatans.find(k => k.kdGiat === formValue.kodeKegiatan);
      const selectedOutput = this.outputs.find(o => o.kdOutput === formValue.kodeOutput);
      const selectedSuboutput = this.suboutputs.find(s => s.kdSOutput === formValue.kodeSuboutput);
      const selectedKomponen = this.komponens.find(k => k.kdKmpnen === formValue.kodeKomponen);
      const selectedSubkomponen = this.subkomponens.find(s => s.kdSkmpnen === formValue.kodeSubkomponen);
      const selectedAkun = this.akuns.find(a => a.kodeAkun === formValue.kodeAkun);
      
      const dto: CreateStpbDetailDto = {
        tanggalTransaksi: formValue.tanggalTransaksi,
        kodeProgram: formValue.kodeProgram,
        namaProgram: selectedProgram?.nmProgram || '',
        kodeKegiatan: formValue.kodeKegiatan,
        namaKegiatan: selectedKegiatan?.nmGiat || '',
        kodeOutput: formValue.kodeOutput,
        namaOutput: selectedOutput?.nmOutput || '',
        kodeSuboutput: formValue.kodeSuboutput,
        namaSuboutput: selectedSuboutput?.nmSOutput || '',
        kodeKomponen: formValue.kodeKomponen,
        namaKomponen: selectedKomponen?.nmKmpnen || '',
        kodeSubkomponen: formValue.kodeSubkomponen,
        namaSubkomponen: selectedSubkomponen?.nmSkmpnen || '',
        kodeAkun: formValue.kodeAkun,
        namaAkun: selectedAkun?.namaAkun || '',
        kodeItem: formValue.kodeItem,
        noItem: formValue.kodeItem,
        namaItem: selectedItem?.namaItem || '',
        keterangan: formValue.uraian,
        volume: 1,
        satuan: 'unit',
        hargaSatuan: formValue.nilaiTransaksi,
        penerimaId: formValue.penerimaId,
        ppn: formValue.ppn || 0,
        ppH21: formValue.pph21 || 0,
        ppH22: formValue.pph22 || 0,
        ppH23: formValue.pph23 || 0
      };

      const request = this.isEditMode && this.detail
        ? this.stpbService.updateDetail(this.stpbId, this.detail.id, dto)
        : this.stpbService.addDetail(this.stpbId, dto);

      request.subscribe({
        next: () => {
          this.message.success(
            this.isEditMode ? 'Detail berhasil diperbarui' : 'Detail berhasil ditambahkan'
          );
          // Emit success FIRST before closing modal
          this.onSuccess.emit();
          // Small delay to ensure parent receives event before modal closes
          setTimeout(() => this.handleCancel(), 100);
        },
        error: (error: any) => {
          console.error('=== Full Error Object ===', error);
          console.error('error.error:', error.error);
          console.error('error.message:', error.message);
          console.error('error.status:', error.status);
          
          // Try to extract error message from various possible locations
          let errorMessage = 'Terjadi kesalahan';
          
          if (error.error) {
            // Check for errors array first (ValidationException format)
            if (error.error.errors && Array.isArray(error.error.errors) && error.error.errors.length > 0) {
              errorMessage = error.error.errors.join(', ');
              console.log('Using errors array:', errorMessage);
            } else if (typeof error.error === 'string') {
              errorMessage = error.error;
              console.log('Using error.error string:', errorMessage);
            } else if (error.error.message) {
              errorMessage = error.error.message;
              console.log('Using error.error.message:', errorMessage);
            } else if (error.error.title) {
              errorMessage = error.error.title;
              console.log('Using error.error.title:', errorMessage);
            }
          } else if (error.message) {
            errorMessage = error.message;
            console.log('Using error.message:', errorMessage);
          }
          
          console.log('Final error message:', errorMessage);
          this.message.error(errorMessage);
          this.isLoading = false;
        }
      });
    } else {
      Object.values(this.detailForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  getFormValidationErrors() {
    const errors: any = {};
    Object.keys(this.detailForm.controls).forEach(key => {
      const control = this.detailForm.get(key);
      if (control && control.errors) {
        errors[key] = control.errors;
      }
    });
    return errors;
  }

  handleCancel(): void {
    this.visible = false;
    this.visibleChange.emit(false);
    this.detailForm.reset({
      volume: 1,
      hargaSatuan: 0
    });
    this.isEditMode = false;
    this.detail = null;
    this.subkomponens = [];
    this.items = [];
    this.isLoading = false;
  }

  get totalNilai(): number {
    return this.detailForm.get('nilaiTransaksi')?.value || 0;
  }

  get totalPPH(): number {
    const pph21 = this.detailForm.get('pph21')?.value || 0;
    const pph22 = this.detailForm.get('pph22')?.value || 0;
    const pph23 = this.detailForm.get('pph23')?.value || 0;
    return pph21 + pph22 + pph23;
  }

  get nilaiBersih(): number {
    const ppn = this.detailForm.get('ppn')?.value || 0;
    return this.totalNilai - ppn - this.totalPPH;
  }
  
  loadPaguInfo(): void {
    const form = this.detailForm.value;
    
    if (!form.kodeProgram || !form.kodeKegiatan || !form.kodeOutput || 
        !form.kodeSuboutput || !form.kodeKomponen || !form.kodeSubkomponen ||
        !form.kodeAkun || !form.kodeItem) {
      return;
    }
    
    this.loadingPagu = true;
    this.anggaranMasterService.checkPagu(
      this.tahun,
      this.revisi,
      form.kodeProgram,
      form.kodeKegiatan,
      form.kodeOutput,
      form.kodeSuboutput,
      form.kodeKomponen,
      form.kodeSubkomponen,
      form.kodeAkun,
      form.kodeItem
    ).subscribe({
      next: (response: any) => {
        if (response.success) {
          this.paguInfo = response.data;
          this.checkPaguRealtime();
        }
        this.loadingPagu = false;
      },
      error: (error: any) => {
        console.error('Error loading pagu info:', error);
        this.paguInfo = null;
        this.loadingPagu = false;
      }
    });
  }
  
  checkPaguRealtime(): void {
    if (!this.paguInfo) return;
    
    const nilaiKotor = this.totalNilai;
    const sisaPaguSetelahInput = this.paguInfo.sisaPagu - nilaiKotor;
    
    this.paguInfo.isOverPagu = sisaPaguSetelahInput < 0;
    this.paguInfo.nilaiInput = nilaiKotor;
    this.paguInfo.sisaSetelahInput = sisaPaguSetelahInput;
  }
  
  get isPaguValid(): boolean {
    if (!this.paguInfo) return true; // Allow if no pagu data
    return !this.paguInfo.isOverPagu;
  }

  // Formatter and Parser for currency input
  currencyFormatter = (value: number): string => {
    return `Rp ${value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
  };

  currencyParser = (value: string): string => {
    return value.replace(/Rp\s?|(,*)/g, '');
  };
}
