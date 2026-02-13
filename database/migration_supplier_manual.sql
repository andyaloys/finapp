-- ====================================================================
-- COPY AND PASTE SCRIPT INI KE MySQL CLIENT/WORKBENCH
-- Database: finapp
-- Server: 10.100.83.166:3366
-- ====================================================================

USE finapp;

-- 1. Create Penerimas table
CREATE TABLE IF NOT EXISTS Penerimas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nama VARCHAR(200) NOT NULL,
    Npwp VARCHAR(20) NULL,
    Alamat VARCHAR(500) NULL,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    CreatedAt DATETIME(6) NOT NULL,
    UpdatedAt DATETIME(6) NULL,
    INDEX IX_Penerimas_Nama (Nama),
    INDEX IX_Penerimas_IsActive (IsActive)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 2. Add PenerimaId column to StpbDetails (if not exists)
SET @column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'finapp' 
    AND TABLE_NAME = 'StpbDetails' 
    AND COLUMN_NAME = 'PenerimaId'
);

SET @sql = IF(@column_exists = 0, 
    'ALTER TABLE StpbDetails ADD COLUMN PenerimaId INT NULL AFTER Keterangan',
    'SELECT "Column PenerimaId already exists" AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 3. Add Foreign Key constraint (if not exists)
SET @fk_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
    WHERE TABLE_SCHEMA = 'finapp' 
    AND TABLE_NAME = 'StpbDetails' 
    AND CONSTRAINT_NAME = 'FK_StpbDetails_Penerimas_PenerimaId'
);

SET @sql = IF(@fk_exists = 0,
    'ALTER TABLE StpbDetails ADD CONSTRAINT FK_StpbDetails_Penerimas_PenerimaId FOREIGN KEY (PenerimaId) REFERENCES Penerimas(Id) ON DELETE SET NULL',
    'SELECT "Foreign Key already exists" AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 3b. Remove old Penerima column from StpbDetails (if exists)
SET @old_column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'finapp' 
    AND TABLE_NAME = 'StpbDetails' 
    AND COLUMN_NAME = 'Penerima'
);

SET @sql = IF(@old_column_exists > 0, 
    'ALTER TABLE StpbDetails DROP COLUMN Penerima',
    'SELECT "Old Penerima column already removed or never existed" AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 4. Create index on PenerimaId (if not exists)
SET @idx_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.STATISTICS 
    WHERE TABLE_SCHEMA = 'finapp' 
    AND TABLE_NAME = 'StpbDetails' 
    AND INDEX_NAME = 'IX_StpbDetails_PenerimaId'
);

SET @sql = IF(@idx_exists = 0,
    'CREATE INDEX IX_StpbDetails_PenerimaId ON StpbDetails(PenerimaId)',
    'SELECT "Index IX_StpbDetails_PenerimaId already exists" AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 5. Insert menu Supplier (if not exists)
INSERT INTO Menus (Id, ParentKey, `Key`, Label, Icon, `Order`, IsActive, CreatedAt, UpdatedAt)
SELECT 
    UUID(),
    'master-data',
    'master-supplier',
    'Supplier',
    'team',
    2,
    1,
    UTC_TIMESTAMP(),
    UTC_TIMESTAMP()
WHERE NOT EXISTS (
    SELECT 1 FROM Menus WHERE `Key` = 'master-supplier'
);

-- 6. Insert permission for Admin role (if not exists)
INSERT INTO RoleMenuPermissions (Id, RoleId, MenuKey, CanView, CanCreate, CanUpdate, CanDelete, CreatedAt, UpdatedAt)
SELECT 
    UUID(),
    '00000000-0000-0000-0000-000000000010',
    'master-supplier',
    1,
    1,
    1,
    1,
    UTC_TIMESTAMP(),
    UTC_TIMESTAMP()
WHERE NOT EXISTS (
    SELECT 1 FROM RoleMenuPermissions 
    WHERE RoleId = '00000000-0000-0000-0000-000000000010' 
    AND MenuKey = 'master-supplier'
);

-- Verification queries
SELECT 'Table created successfully!' AS Status, COUNT(*) AS TableExists 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'finapp' AND TABLE_NAME = 'Penerimas';

SELECT 'Column added successfully!' AS Status, COUNT(*) AS ColumnExists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'finapp' AND TABLE_NAME = 'StpbDetails' AND COLUMN_NAME = 'PenerimaId';

SELECT 'Menu created successfully!' AS Status, COUNT(*) AS MenuExists 
FROM Menus WHERE `Key` = 'master-supplier';

SELECT 'Permission created successfully!' AS Status, COUNT(*) AS PermissionExists 
FROM RoleMenuPermissions WHERE MenuKey = 'master-supplier';

SELECT '✅ MIGRATION COMPLETE!' AS Status;
