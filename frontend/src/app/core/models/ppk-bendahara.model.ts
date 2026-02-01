export enum JabatanType {
  PPK = 1,
  Bendahara = 2
}

export interface PpkBendahara {
  id: string;
  nama: string;
  nip: string;
  jabatan: JabatanType;
  isActive: boolean;
}

export interface CreatePpkBendaharaDto {
  nama: string;
  nip: string;
  jabatan: JabatanType;
  isActive: boolean;
}

export interface PpkBendaharaDto {
  id: string;
  nama: string;
  nip: string;
  jabatan: JabatanType;
  jabatanName: string;
  isActive: boolean;
}
