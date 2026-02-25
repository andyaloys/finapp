import { StpbStatus } from './stpb-status.enum';
import { StpbDetailDto, CreateStpbDetailDto } from './stpb-detail.model';
import { PpkBendaharaDto } from './ppk-bendahara.model';

export interface Stpb {
  id: string;
  tahun: number;
  tanggalSTPB: Date;
  nomorSTPB: string;
  keterangan?: string;
  alasanDikembalikan?: string;
  totalNilai: number;
  status: StpbStatus;
  statusDisplay: string;
  ppkBendaharaId?: string;
  ppkBendahara?: PpkBendaharaDto;
  details: StpbDetailDto[];
  createdAt: Date;
  updatedAt: Date;
  createdBy: string;
  creatorName?: string;
  updatedBy?: string;
}

export interface CreateStpb {
  tahun: number;
  tanggalSTPB: Date;
  keterangan?: string;
  ppkBendaharaId?: string;
}

export interface UpdateStpb {
  tahun: number;
  tanggalSTPB: Date;
  keterangan?: string;
  ppkBendaharaId?: string;
}
