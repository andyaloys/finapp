// Program
export interface ProgramDto {
  id: string;
  kode: string;
  nama: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateProgramDto {
  kode: string;
  nama: string;
}

export interface UpdateProgramDto {
  kode: string;
  nama: string;
  isActive: boolean;
}

// Kegiatan
export interface KegiatanDto {
  id: string;
  kode: string;
  nama: string;
  programId: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  program?: ProgramDto;
}

export interface CreateKegiatanDto {
  kode: string;
  nama: string;
  programId: string;
}

export interface UpdateKegiatanDto {
  kode: string;
  nama: string;
  programId: string;
  isActive: boolean;
}

// Output
export interface OutputDto {
  id: string;
  kode: string;
  nama: string;
  kegiatanId: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  kegiatan?: KegiatanDto;
}

export interface CreateOutputDto {
  kode: string;
  nama: string;
  kegiatanId: string;
}

export interface UpdateOutputDto {
  kode: string;
  nama: string;
  kegiatanId: string;
  isActive: boolean;
}

// Suboutput
export interface SuboutputDto {
  id: string;
  kode: string;
  nama: string;
  outputId: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  output?: OutputDto;
}

export interface CreateSuboutputDto {
  kode: string;
  nama: string;
  outputId: string;
}

export interface UpdateSuboutputDto {
  kode: string;
  nama: string;
  outputId: string;
  isActive: boolean;
}

// Komponen
export interface KomponenDto {
  id: string;
  kode: string;
  nama: string;
  suboutputId: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  suboutput?: SuboutputDto;
}

export interface CreateKomponenDto {
  kode: string;
  nama: string;
  suboutputId: string;
}

export interface UpdateKomponenDto {
  kode: string;
  nama: string;
  suboutputId: string;
  isActive: boolean;
}

// Subkomponen
export interface SubkomponenDto {
  id: string;
  kode: string;
  nama: string;
  komponenId: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  komponen?: KomponenDto;
}

export interface CreateSubkomponenDto {
  kode: string;
  nama: string;
  komponenId: string;
}

export interface UpdateSubkomponenDto {
  kode: string;
  nama: string;
  komponenId: string;
  isActive: boolean;
}

// Akun
export interface AkunDto {
  id: string;
  kode: string;
  nama: string;
  subkomponenId: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  subkomponen?: SubkomponenDto;
}

export interface CreateAkunDto {
  kode: string;
  nama: string;
  subkomponenId: string;
}

export interface UpdateAkunDto {
  kode: string;
  nama: string;
  subkomponenId: string;
  isActive: boolean;
}

// Item
export interface ItemDto {
  id: string;
  nama: string;
  satuan: string;
  hargaSatuan: number;
  akunId: string;
  kodeAkun: string;
  isActive: boolean;
  createdAt: Date;
  akun?: AkunDto;
}

export interface CreateItemDto {
  nama: string;
  satuan: string;
  hargaSatuan: number;
  akunId: string;
  isActive: boolean;
}

export interface UpdateItemDto {
  nama: string;
  satuan: string;
  hargaSatuan: number;
  akunId: string;
  isActive: boolean;
}

// Paging
export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

// Anggaran Master Distinct DTOs (from CSV data)
export interface AnggaranProgramDto {
  kdProgram: string;
  nmProgram: string;
}

export interface AnggaranKegiatanDto {
  kdGiat: string;
  nmGiat: string;
}

export interface AnggaranOutputDto {
  kdOutput: string;
  nmOutput: string;
}

export interface AnggaranSuboutputDto {
  kdSOutput: string;
  nmSOutput: string;
}

export interface AnggaranKomponenDto {
  kdKmpnen: string;
  nmKmpnen: string;
}

export interface AnggaranSubkomponenDto {
  kdSkmpnen: string;
  nmSkmpnen: string;
}

export interface AnggaranAkunDto {
  kdAkun: string;
  nmAkun: string;
}

export interface AnggaranItemDto {
  noItem: string;
  nmItem: string;
}
