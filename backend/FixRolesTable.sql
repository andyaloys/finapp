USE finapp;

-- Add UpdatedAt column to Roles if not exists
SET @exist := (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_SCHEMA = 'finapp' 
               AND TABLE_NAME = 'Roles' 
               AND COLUMN_NAME = 'UpdatedAt');

SET @sqlstmt := IF(@exist = 0, 
    'ALTER TABLE Roles ADD COLUMN UpdatedAt datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)',
    'SELECT "Column UpdatedAt already exists" AS Result');

PREPARE stmt FROM @sqlstmt;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Update existing rows
UPDATE Roles SET UpdatedAt = CreatedAt WHERE UpdatedAt IS NULL OR UpdatedAt = '0001-01-01 00:00:00';

-- Verify
DESCRIBE Roles;
SELECT * FROM Roles;
