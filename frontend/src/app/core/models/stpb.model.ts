export interface Stpb {
  id: string;
  tanggal: Date;
  kodeProgram: string;
  kodeKegiatan: string;
  kodeOutput: string;
  kodeSuboutput: string;
  kodeKomponen: string;
  kodeSubkomponen: string;
  kodeAkun: string;
  itemId?: string;
  uraian: string;
  nominal: number;
  ppn: number;
  pph21: number;
  pph22: number;
  pph23: number;
  nilaiBersih: number;
  nomorSTPB: string;
  isLocked: boolean;
  createdAt: Date;
  updatedAt: Date;
  programId?: string;
  kegiatanId?: string;
  outputId?: string;
  suboutputId?: string;
  komponenId?: string;
  subkomponenId?: string;
  akunId?: string;
}

export interface CreateStpb {
  tanggal: Date;
  programId: string;
  kegiatanId: string;
  outputId: string;
  suboutputId: string;
  komponenId: string;
  subkomponenId: string;
  akunId: string;
  itemId?: string;
  uraian: string;
  nominal: number;
  ppn: number;
  pph21: number;
  pph22: number;
  pph23: number;
  nomorSTPB?: string;
}

export interface UpdateStpb {
  tanggal: Date;
  programId: string;
  kegiatanId: string;
  outputId: string;
  suboutputId: string;
  komponenId: string;
  subkomponenId: string;
  akunId: string;
  itemId?: string;
  uraian: string;
  nominal: number;
  ppn: number;
  pph21: number;
  pph22: number;
  pph23: number;
  nomorSTPB?: string;
}
