-- ============================================
-- ADD TAXRATE FOREIGN KEYS TO STPBDETAILS
-- ============================================

USE finapp_db;

-- STEP 0: CLEANUP - Hapus kolom/index/FK yang ada (jika ada)
-- Hapus FK dulu
SET @fk_exists = 0;
SELECT COUNT(*) INTO @fk_exists FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND CONSTRAINT_NAME = 'FK_StpbDetails_TaxRates_PpnTaxRateId';
SET @query = IF(@fk_exists > 0, 'ALTER TABLE StpbDetails DROP FOREIGN KEY FK_StpbDetails_TaxRates_PpnTaxRateId', 'SELECT "FK tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @fk_exists = 0;
SELECT COUNT(*) INTO @fk_exists FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND CONSTRAINT_NAME = 'FK_StpbDetails_TaxRates_Pph21TaxRateId';
SET @query = IF(@fk_exists > 0, 'ALTER TABLE StpbDetails DROP FOREIGN KEY FK_StpbDetails_TaxRates_Pph21TaxRateId', 'SELECT "FK tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @fk_exists = 0;
SELECT COUNT(*) INTO @fk_exists FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND CONSTRAINT_NAME = 'FK_StpbDetails_TaxRates_Pph22TaxRateId';
SET @query = IF(@fk_exists > 0, 'ALTER TABLE StpbDetails DROP FOREIGN KEY FK_StpbDetails_TaxRates_Pph22TaxRateId', 'SELECT "FK tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @fk_exists = 0;
SELECT COUNT(*) INTO @fk_exists FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND CONSTRAINT_NAME = 'FK_StpbDetails_TaxRates_Pph23TaxRateId';
SET @query = IF(@fk_exists > 0, 'ALTER TABLE StpbDetails DROP FOREIGN KEY FK_StpbDetails_TaxRates_Pph23TaxRateId', 'SELECT "FK tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

-- Hapus Index
SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND INDEX_NAME = 'IX_StpbDetails_PpnTaxRateId';
SET @query = IF(@idx_exists > 0, 'DROP INDEX IX_StpbDetails_PpnTaxRateId ON StpbDetails', 'SELECT "Index tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND INDEX_NAME = 'IX_StpbDetails_Pph21TaxRateId';
SET @query = IF(@idx_exists > 0, 'DROP INDEX IX_StpbDetails_Pph21TaxRateId ON StpbDetails', 'SELECT "Index tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND INDEX_NAME = 'IX_StpbDetails_Pph22TaxRateId';
SET @query = IF(@idx_exists > 0, 'DROP INDEX IX_StpbDetails_Pph22TaxRateId ON StpbDetails', 'SELECT "Index tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND INDEX_NAME = 'IX_StpbDetails_Pph23TaxRateId';
SET @query = IF(@idx_exists > 0, 'DROP INDEX IX_StpbDetails_Pph23TaxRateId ON StpbDetails', 'SELECT "Index tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

-- Hapus Kolom
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND COLUMN_NAME = 'PpnTaxRateId';
SET @query = IF(@col_exists > 0, 'ALTER TABLE StpbDetails DROP COLUMN PpnTaxRateId', 'SELECT "Column tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND COLUMN_NAME = 'Pph21TaxRateId';
SET @query = IF(@col_exists > 0, 'ALTER TABLE StpbDetails DROP COLUMN Pph21TaxRateId', 'SELECT "Column tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND COLUMN_NAME = 'Pph22TaxRateId';
SET @query = IF(@col_exists > 0, 'ALTER TABLE StpbDetails DROP COLUMN Pph22TaxRateId', 'SELECT "Column tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND COLUMN_NAME = 'Pph23TaxRateId';
SET @query = IF(@col_exists > 0, 'ALTER TABLE StpbDetails DROP COLUMN Pph23TaxRateId', 'SELECT "Column tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

-- 1. Tambah 4 kolom foreign key dengan collation yang benar
ALTER TABLE StpbDetails
ADD COLUMN PpnTaxRateId CHAR(36) COLLATE ascii_general_ci NULL AFTER PenerimaId,
ADD COLUMN Pph21TaxRateId CHAR(36) COLLATE ascii_general_ci NULL,
ADD COLUMN Pph22TaxRateId CHAR(36) COLLATE ascii_general_ci NULL,
ADD COLUMN Pph23TaxRateId CHAR(36) COLLATE ascii_general_ci NULL;

-- 2. Tambah index untuk performance
CREATE INDEX IX_StpbDetails_PpnTaxRateId ON StpbDetails(PpnTaxRateId);
CREATE INDEX IX_StpbDetails_Pph21TaxRateId ON StpbDetails(Pph21TaxRateId);
CREATE INDEX IX_StpbDetails_Pph22TaxRateId ON StpbDetails(Pph22TaxRateId);
CREATE INDEX IX_StpbDetails_Pph23TaxRateId ON StpbDetails(Pph23TaxRateId);

-- 3. Set data lama ke default TaxRate (jika nilai pajak > 0)
-- PPN
UPDATE StpbDetails
SET PpnTaxRateId = (SELECT Id FROM TaxRates WHERE TaxType = 'PPN' AND IsDefault = 1 LIMIT 1)
WHERE PPN > 0 AND PpnTaxRateId IS NULL;

-- PPH21
UPDATE StpbDetails
SET Pph21TaxRateId = (SELECT Id FROM TaxRates WHERE TaxType = 'PPH21' AND IsDefault = 1 LIMIT 1)
WHERE PPH21 > 0 AND Pph21TaxRateId IS NULL;

-- PPH22
UPDATE StpbDetails
SET Pph22TaxRateId = (SELECT Id FROM TaxRates WHERE TaxType = 'PPH22' AND IsDefault = 1 LIMIT 1)
WHERE PPH22 > 0 AND Pph22TaxRateId IS NULL;

-- PPH23
UPDATE StpbDetails
SET Pph23TaxRateId = (SELECT Id FROM TaxRates WHERE TaxType = 'PPH23' AND IsDefault = 1 LIMIT 1)
WHERE PPH23 > 0 AND Pph23TaxRateId IS NULL;

-- 4. Tambah foreign key constraint
ALTER TABLE StpbDetails
ADD CONSTRAINT FK_StpbDetails_TaxRates_PpnTaxRateId 
    FOREIGN KEY (PpnTaxRateId) REFERENCES TaxRates(Id) ON DELETE SET NULL,
ADD CONSTRAINT FK_StpbDetails_TaxRates_Pph21TaxRateId 
    FOREIGN KEY (Pph21TaxRateId) REFERENCES TaxRates(Id) ON DELETE SET NULL,
ADD CONSTRAINT FK_StpbDetails_TaxRates_Pph22TaxRateId 
    FOREIGN KEY (Pph22TaxRateId) REFERENCES TaxRates(Id) ON DELETE SET NULL,
ADD CONSTRAINT FK_StpbDetails_TaxRates_Pph23TaxRateId 
    FOREIGN KEY (Pph23TaxRateId) REFERENCES TaxRates(Id) ON DELETE SET NULL;

-- 5. Verify
SELECT 'Kolom TaxRate foreign keys berhasil ditambahkan!' as Status;

SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    IS_NULLABLE,
    COLUMN_KEY
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp_db' 
    AND TABLE_NAME = 'StpbDetails' 
    AND COLUMN_NAME LIKE '%TaxRateId'
ORDER BY ORDINAL_POSITION;

SELECT 
    CONSTRAINT_NAME,
    COLUMN_NAME,
    REFERENCED_TABLE_NAME,
    REFERENCED_COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_SCHEMA = 'finapp_db'
    AND TABLE_NAME = 'StpbDetails'
    AND CONSTRAINT_NAME LIKE 'FK%TaxRate%';
