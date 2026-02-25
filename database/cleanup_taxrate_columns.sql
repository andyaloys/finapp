-- ============================================
-- CLEANUP - HAPUS KOLOM TAXRATEID YANG ADA
-- ============================================

USE finapp_db;

-- Hapus foreign key constraint dulu (jika ada)
SET @fk_exists = 0;
SELECT COUNT(*) INTO @fk_exists FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND CONSTRAINT_NAME = 'FK_StpbDetails_TaxRates_PpnTaxRateId';
SET @query = IF(@fk_exists > 0, 'ALTER TABLE StpbDetails DROP FOREIGN KEY FK_StpbDetails_TaxRates_PpnTaxRateId', 'SELECT "FK PpnTaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @fk_exists = 0;
SELECT COUNT(*) INTO @fk_exists FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND CONSTRAINT_NAME = 'FK_StpbDetails_TaxRates_Pph21TaxRateId';
SET @query = IF(@fk_exists > 0, 'ALTER TABLE StpbDetails DROP FOREIGN KEY FK_StpbDetails_TaxRates_Pph21TaxRateId', 'SELECT "FK Pph21TaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @fk_exists = 0;
SELECT COUNT(*) INTO @fk_exists FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND CONSTRAINT_NAME = 'FK_StpbDetails_TaxRates_Pph22TaxRateId';
SET @query = IF(@fk_exists > 0, 'ALTER TABLE StpbDetails DROP FOREIGN KEY FK_StpbDetails_TaxRates_Pph22TaxRateId', 'SELECT "FK Pph22TaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @fk_exists = 0;
SELECT COUNT(*) INTO @fk_exists FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND CONSTRAINT_NAME = 'FK_StpbDetails_TaxRates_Pph23TaxRateId';
SET @query = IF(@fk_exists > 0, 'ALTER TABLE StpbDetails DROP FOREIGN KEY FK_StpbDetails_TaxRates_Pph23TaxRateId', 'SELECT "FK Pph23TaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

-- Hapus index (jika ada)
SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND INDEX_NAME = 'IX_StpbDetails_PpnTaxRateId';
SET @query = IF(@idx_exists > 0, 'DROP INDEX IX_StpbDetails_PpnTaxRateId ON StpbDetails', 'SELECT "Index PpnTaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND INDEX_NAME = 'IX_StpbDetails_Pph21TaxRateId';
SET @query = IF(@idx_exists > 0, 'DROP INDEX IX_StpbDetails_Pph21TaxRateId ON StpbDetails', 'SELECT "Index Pph21TaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND INDEX_NAME = 'IX_StpbDetails_Pph22TaxRateId';
SET @query = IF(@idx_exists > 0, 'DROP INDEX IX_StpbDetails_Pph22TaxRateId ON StpbDetails', 'SELECT "Index Pph22TaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND INDEX_NAME = 'IX_StpbDetails_Pph23TaxRateId';
SET @query = IF(@idx_exists > 0, 'DROP INDEX IX_StpbDetails_Pph23TaxRateId ON StpbDetails', 'SELECT "Index Pph23TaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

-- Hapus kolom (jika ada)
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND COLUMN_NAME = 'PpnTaxRateId';
SET @query = IF(@col_exists > 0, 'ALTER TABLE StpbDetails DROP COLUMN PpnTaxRateId', 'SELECT "Column PpnTaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND COLUMN_NAME = 'Pph21TaxRateId';
SET @query = IF(@col_exists > 0, 'ALTER TABLE StpbDetails DROP COLUMN Pph21TaxRateId', 'SELECT "Column Pph21TaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND COLUMN_NAME = 'Pph22TaxRateId';
SET @query = IF(@col_exists > 0, 'ALTER TABLE StpbDetails DROP COLUMN Pph22TaxRateId', 'SELECT "Column Pph22TaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp_db' AND TABLE_NAME = 'StpbDetails' AND COLUMN_NAME = 'Pph23TaxRateId';
SET @query = IF(@col_exists > 0, 'ALTER TABLE StpbDetails DROP COLUMN Pph23TaxRateId', 'SELECT "Column Pph23TaxRateId tidak ada" as Info');
PREPARE stmt FROM @query; EXECUTE stmt; DEALLOCATE PREPARE stmt;

SELECT 'Cleanup selesai! Semua kolom TaxRateId telah dihapus.' as Status;
