export interface TaxRateDto {
  id: string;
  taxType: string;
  category: string;
  rate: number;
  description?: string;
  referenceCode?: string;
  isDefault: boolean;
  isActive: boolean;
  displayOrder: number;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateTaxRateDto {
  taxType: string;
  category: string;
  rate: number;
  description?: string;
  referenceCode?: string;
  isDefault: boolean;
  isActive: boolean;
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
