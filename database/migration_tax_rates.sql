-- ====================================================================
-- MIGRATION: TARIF PAJAK (TAX RATES)
-- Database: finapp
-- Server: 10.100.83.166:3366
-- ====================================================================

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

-- 2. Insert default tax rates
INSERT INTO TaxRates (TaxCode, TaxName, Rate, IsActive, CreatedAt) VALUES
('PPN', 'Pajak Pertambahan Nilai', 11.00, 1, NOW()),
('PPH21', 'PPh Pasal 21', 2.50, 1, NOW()),
('PPH22', 'PPh Pasal 22', 1.50, 1, NOW()),
('PPH23', 'PPh Pasal 23', 2.00, 1, NOW())
ON DUPLICATE KEY UPDATE 
    TaxName = VALUES(TaxName),
    Rate = VALUES(Rate),
    IsActive = VALUES(IsActive);

-- 3. Insert menu 'master-taxrate' into Menus table (if not exists)
INSERT INTO Menus (`Key`, Label, Icon, Route, ParentKey, `Order`, IsActive, CreatedAt)
SELECT 
    'master-taxrate',
    'Tarif Pajak',
    'percentage',
    '/tax-rate',
    'master-data',
    3,
    1,
    NOW()
WHERE NOT EXISTS (
    SELECT 1 FROM Menus WHERE `Key` = 'master-taxrate'
);

-- 4. Grant permissions to Admin role (if not exists)
INSERT INTO RoleMenuPermissions (RoleId, MenuKey, IsVisible, CreatedAt)
SELECT 
    '00000000-0000-0000-0000-000000000010',
    'master-taxrate',
    1,
    NOW()
WHERE NOT EXISTS (
    SELECT 1 FROM RoleMenuPermissions 
    WHERE RoleId = '00000000-0000-0000-0000-000000000010' 
    AND MenuKey = 'master-taxrate'
);

-- ====================================================================
-- VERIFICATION QUERIES
-- ====================================================================

-- Check TaxRates table
SELECT 'TaxRates table created:' AS Info, 
       IF(COUNT(*) > 0, 'YES', 'NO') AS Status
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'finapp' AND TABLE_NAME = 'TaxRates';

-- Check TaxRates data
SELECT * FROM TaxRates;

-- Check menu
SELECT * FROM Menus WHERE `Key` = 'master-taxrate';

-- Check role permissions
SELECT * FROM RoleMenuPermissions WHERE MenuKey = 'master-taxrate';
