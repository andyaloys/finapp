-- ============================================
-- SCRIPT UPDATE STRUKTUR TABLE TAXRATES
-- Dari: TaxCode, TaxName -> TaxType, Category
-- ============================================

-- 1. BACKUP DATA LAMA
CREATE TABLE IF NOT EXISTS TaxRates_Backup AS SELECT * FROM TaxRates;

-- 2. TAMBAH KOLOM BARU DULU (jika belum ada)
-- Cek dan tambah Description
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'TaxRates' AND COLUMN_NAME = 'Description';
SET @query = IF(@col_exists = 0, 
    'ALTER TABLE TaxRates ADD COLUMN Description VARCHAR(500) NULL AFTER Rate', 
    'SELECT "Column Description already exists" as Info');
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Cek dan tambah ReferenceCode
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'TaxRates' AND COLUMN_NAME = 'ReferenceCode';
SET @query = IF(@col_exists = 0, 
    'ALTER TABLE TaxRates ADD COLUMN ReferenceCode VARCHAR(50) NULL', 
    'SELECT "Column ReferenceCode already exists" as Info');
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Cek dan tambah IsDefault
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'TaxRates' AND COLUMN_NAME = 'IsDefault';
SET @query = IF(@col_exists = 0, 
    'ALTER TABLE TaxRates ADD COLUMN IsDefault TINYINT(1) NOT NULL DEFAULT 0', 
    'SELECT "Column IsDefault already exists" as Info');
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Cek dan tambah DisplayOrder
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'TaxRates' AND COLUMN_NAME = 'DisplayOrder';
SET @query = IF(@col_exists = 0, 
    'ALTER TABLE TaxRates ADD COLUMN DisplayOrder INT NOT NULL DEFAULT 1', 
    'SELECT "Column DisplayOrder already exists" as Info');
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 3. MIGRASI DATA: Copy TaxCode -> TaxType, TaxName -> Category
-- Cek dan tambah TaxType
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'TaxRates' AND COLUMN_NAME = 'TaxType';
SET @query = IF(@col_exists = 0, 
    'ALTER TABLE TaxRates ADD COLUMN TaxType VARCHAR(255) NULL AFTER Id', 
    'SELECT "Column TaxType already exists" as Info');
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Cek dan tambah Category
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'TaxRates' AND COLUMN_NAME = 'Category';
SET @query = IF(@col_exists = 0, 
    'ALTER TABLE TaxRates ADD COLUMN Category VARCHAR(100) NULL AFTER TaxType', 
    'SELECT "Column Category already exists" as Info');
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Copy data dari kolom lama ke kolom baru (jika TaxCode masih ada)
UPDATE TaxRates SET TaxType = TaxCode WHERE TaxType IS NULL;
UPDATE TaxRates SET Category = TaxName WHERE Category IS NULL;

-- Set IsDefault = 1 untuk rate pertama setiap jenis pajak (berdasarkan Id terkecil)
UPDATE TaxRates t1
SET t1.IsDefault = 1
WHERE t1.Id IN (
    SELECT MinId FROM (
        SELECT MIN(Id) as MinId
        FROM TaxRates
        GROUP BY TaxCode
    ) as temp
);

-- Set DisplayOrder berdasarkan urutan Id per TaxCode
UPDATE TaxRates t1
INNER JOIN (
    SELECT 
        t3.Id,
        (SELECT COUNT(*) FROM TaxRates t4 WHERE t4.TaxCode = t3.TaxCode AND t4.Id <= t3.Id) as RowNum
    FROM TaxRates t3
) t2 ON t1.Id = t2.Id
SET t1.DisplayOrder = t2.RowNum;

-- 4. DROP KOLOM LAMA (jika masih ada)
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'TaxRates' AND COLUMN_NAME = 'TaxCode';
SET @query = IF(@col_exists > 0, 
    'ALTER TABLE TaxRates DROP COLUMN TaxCode', 
    'SELECT "Column TaxCode already dropped" as Info');
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'TaxRates' AND COLUMN_NAME = 'TaxName';
SET @query = IF(@col_exists > 0, 
    'ALTER TABLE TaxRates DROP COLUMN TaxName', 
    'SELECT "Column TaxName already dropped" as Info');
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 5. UBAH TaxType & Category jadi NOT NULL
ALTER TABLE TaxRates 
MODIFY COLUMN TaxType VARCHAR(255) NOT NULL,
MODIFY COLUMN Category VARCHAR(100) NOT NULL;

-- 6. VERIFY STRUKTUR BARU
SELECT 'Struktur table TaxRates berhasil diupdate!' as Status;
SELECT * FROM TaxRates ORDER BY TaxType, DisplayOrder;

-- ============================================
-- SEED DATA DEFAULT TAX RATES (Opsional)
-- Hapus komentar di bawah jika ingin replace dengan data default
-- ============================================

/*
-- Hapus data lama
DELETE FROM TaxRates;

-- PPN (2 rates)
INSERT INTO TaxRates (Id, TaxType, Category, Rate, Description, ReferenceCode, IsDefault, IsActive, DisplayOrder, CreatedAt)
VALUES 
(UUID(), 'PPN', 'PPN 11%', 11.00, 'Pajak Pertambahan Nilai 11% (Default)', 'PP-NO-23-2024', 1, 1, 1, NOW()),
(UUID(), 'PPN', 'PPN 12%', 12.00, 'Pajak Pertambahan Nilai 12% (Berlaku 2025)', 'PP-NO-23-2024-V2', 0, 1, 2, NOW());

-- PPH21 (5 rates)
INSERT INTO TaxRates (Id, TaxType, Category, Rate, Description, ReferenceCode, IsDefault, IsActive, DisplayOrder, CreatedAt)
VALUES
(UUID(), 'PPH21', 'Tarif 0%', 0.00, 'PPh 21 Penghasilan s.d. Rp 60 juta/tahun', 'UU-36-2008-P17', 0, 1, 1, NOW()),
(UUID(), 'PPH21', 'Tarif 5%', 5.00, 'PPh 21 Penghasilan Rp 60-250 juta/tahun (Default)', 'UU-36-2008-P17', 1, 1, 2, NOW()),
(UUID(), 'PPH21', 'Tarif 15%', 15.00, 'PPh 21 Penghasilan Rp 250-500 juta/tahun', 'UU-36-2008-P17', 0, 1, 3, NOW()),
(UUID(), 'PPH21', 'Tarif 25%', 25.00, 'PPh 21 Penghasilan Rp 500 juta-5 M/tahun', 'UU-36-2008-P17', 0, 1, 4, NOW()),
(UUID(), 'PPH21', 'Tarif 30%', 30.00, 'PPh 21 Penghasilan di atas Rp 5 M/tahun', 'UU-36-2008-P17', 0, 1, 5, NOW());

-- PPH22 (4 rates)
INSERT INTO TaxRates (Id, TaxType, Category, Rate, Description, ReferenceCode, IsDefault, IsActive, DisplayOrder, CreatedAt)
VALUES
(UUID(), 'PPH22', 'Tarif 1.5%', 1.50, 'PPh 22 Pembelian Barang (Default)', 'PMK-34-2017', 1, 1, 1, NOW()),
(UUID(), 'PPH22', 'Tarif 0.25%', 0.25, 'PPh 22 Impor yang menggunakan API', 'PMK-34-2017-P2', 0, 1, 2, NOW()),
(UUID(), 'PPH22', 'Tarif 0.5%', 0.50, 'PPh 22 Penjualan hasil produksi', 'PMK-34-2017-P3', 0, 1, 3, NOW()),
(UUID(), 'PPH22', 'Tarif 7.5%', 7.50, 'PPh 22 Impor yang tidak dikuasai', 'PMK-34-2017-P4', 0, 1, 4, NOW());

-- PPH23 (4 rates)
INSERT INTO TaxRates (Id, TaxType, Category, Rate, Description, ReferenceCode, IsDefault, IsActive, DisplayOrder, CreatedAt)
VALUES
(UUID(), 'PPH23', 'Tarif 2%', 2.00, 'PPh 23 Sewa dan Jasa (Default)', 'UU-36-2008-P23', 1, 1, 1, NOW()),
(UUID(), 'PPH23', 'Tarif 15%', 15.00, 'PPh 23 Dividen, bunga, royalti', 'UU-36-2008-P23-A2', 0, 1, 2, NOW()),
(UUID(), 'PPH23', 'Tarif 10%', 10.00, 'PPh 23 Hadiah dan penghargaan', 'PMK-141-2015', 0, 1, 3, NOW()),
(UUID(), 'PPH23', 'Tarif 0.5%', 0.50, 'PPh 23 Jasa konstruksi', 'PP-9-2022', 0, 1, 4, NOW());

SELECT 'Data default tax rates berhasil di-seed!' as Status;
*/
