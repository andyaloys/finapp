export interface Stpb {
  id: number;
  nomorSTPB: string;
  tanggal: Date;
  deskripsi?: string;
  nilaiTotal: number;
  status: string;
  createdBy: number;
  creatorName: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateStpb {
  nomorSTPB: string;
  tanggal: Date;
  deskripsi?: string;
  nilaiTotal: number;
  status: string;
}

export interface UpdateStpb {
  nomorSTPB: string;
  tanggal: Date;
  deskripsi?: string;
  nilaiTotal: number;
  status: string;
}
