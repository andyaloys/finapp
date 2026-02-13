-- Script untuk menambahkan menu Supplier dan migration database

-- 1. Tambah menu Supplier di master data
INSERT INTO Menus (Id, ParentKey, `Key`, Label, Icon, `Order`, IsActive, CreatedAt, UpdatedAt)
VALUES (
    UUID(),
    'master-data',
    'master-supplier',
    'Supplier',
    'team',
    2,
    1,
    UTC_TIMESTAMP(),
    UTC_TIMESTAMP()
);

-- 2. Berikan permission ke role Admin untuk menu supplier
INSERT INTO RoleMenuPermissions (Id, RoleId, MenuKey, CanView, CanCreate, CanUpdate, CanDelete, CreatedAt, UpdatedAt)
SELECT 
    UUID(),
    '00000000-0000-0000-0000-000000000010',  -- Admin Role ID
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

-- 3. Create Penerima table
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

-- 4. Add PenerimaId column to StpbDetails
ALTER TABLE StpbDetails 
ADD COLUMN PenerimaId INT NULL AFTER Keterangan,
ADD CONSTRAINT FK_StpbDetails_Penerimas_PenerimaId 
    FOREIGN KEY (PenerimaId) REFERENCES Penerimas(Id) ON DELETE SET NULL;

-- 5. Create index for better performance
CREATE INDEX IX_StpbDetails_PenerimaId ON StpbDetails(PenerimaId);

-- Note: Data existing di StpbDetails tidak perlu migrasi karena penerima lama masih ada di field Penerima (string)
-- Untuk data baru akan menggunakan PenerimaId (int FK)
