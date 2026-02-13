-- Simple TaxRates table creation and data seeding
-- No menu/permission changes, just the core table

USE finapp;

-- 1. Create TaxRates table
CREATE TABLE IF NOT EXISTS TaxRates (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TaxCode VARCHAR(20) NOT NULL UNIQUE,
    TaxName VARCHAR(100) NOT NULL,
    Rate DECIMAL(5,2) NOT NULL,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    CreatedAt DATETIME(6) NOT NULL,
    UpdatedAt DATETIME(6) NULL,
    INDEX IX_TaxRates_TaxCode (TaxCode),
    INDEX IX_TaxRates_IsActive (IsActive)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 2. Insert default tax rates (skip if exists)
INSERT IGNORE INTO TaxRates (TaxCode, TaxName, Rate, IsActive, CreatedAt) VALUES
('PPN', 'Pajak Pertambahan Nilai', 11.00, 1, NOW()),
('PPH21', 'PPh Pasal 21', 2.50, 1, NOW()),
('PPH22', 'PPh Pasal 22', 1.50, 1, NOW()),
('PPH23', 'PPh Pasal 23', 2.00, 1, NOW());

-- 3. Verify
SELECT * FROM TaxRates;
