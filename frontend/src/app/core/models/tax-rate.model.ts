export interface TaxRate {
  id: number;
  taxCode: string;
  taxName: string;
  rate: number;
  isActive: boolean;
  createdAt: Date;
  updatedAt?: Date;
}

export interface CreateTaxRateDto {
  taxCode: string;
  taxName: string;
  rate: number;
}

export interface UpdateTaxRateDto {
  taxName: string;
  rate: number;
  isActive: boolean;
}

export interface TaxRatesForCalculation {
  ppn: number;
  pph21: number;
  pph22: number;
  pph23: number;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}
