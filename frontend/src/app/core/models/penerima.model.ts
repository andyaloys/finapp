export interface Penerima {
  id: number;
  nama: string;
  npwp?: string;
  alamat?: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt?: Date;
}

export interface CreatePenerimaDto {
  nama: string;
  npwp?: string;
  alamat?: string;
}

export interface UpdatePenerimaDto {
  nama: string;
  npwp?: string;
  alamat?: string;
  isActive: boolean;
}
