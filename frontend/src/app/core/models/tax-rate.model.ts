export interface TaxRate {
  id: string;
  taxType: string;
  category: string;
  rate: number;
  description?: string;
  referenceCode?: string;
  isDefault: boolean;
  isActive: boolean;
  displayOrder: number;
  createdAt: Date;
  updatedAt?: Date;
}

export interface CreateTaxRateDto {
  taxType: string;
  category: string;
  rate: number;
  description?: string;
  referenceCode?: string;
  isDefault: boolean;
  displayOrder: number;
}

export interface UpdateTaxRateDto {
  category: string;
  rate: number;
  description?: string;
  referenceCode?: string;
  isDefault: boolean;
  isActive: boolean;
  displayOrder: number;
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
