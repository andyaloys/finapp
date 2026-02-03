export interface MonitoringAnggaran {
  kodeProgram: string;
  namaProgram: string;
  kodeKegiatan: string;
  namaKegiatan: string;
  kodeOutput: string;
  namaOutput: string;
  kodeSuboutput: string;
  namaSuboutput: string;
  kodeKomponen: string;
  namaKomponen: string;
  kodeSubkomponen: string;
  namaSubkomponen: string;
  kodeAkun: string;
  namaAkun: string;
  noItem: string;
  namaItem: string;
  paguAnggaran: number;
  realisasi: number;
  sisaAnggaran: number;
  persenRealisasi: number;
  tahunAnggaran: number;
  revisi: number;
  coa: string;
}

export interface StpbDetailMonitoring {
  noStpb: string;
  tanggalStpb: string;
  keterangan: string;
  penerima: string | null;
  nilaiKotor: number;
  pajak: number;
  nilaiBersih: number;
  status: string;
  stpbId: string;
}
