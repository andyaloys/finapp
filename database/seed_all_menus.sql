-- Seed all menus for FinApp
USE finapp;

-- Clear existing menus first (optional, comment out if you want to keep existing)
-- DELETE FROM RoleMenuPermissions;
-- DELETE FROM Menus;

-- Insert parent menus
INSERT INTO Menus (Id, `Key`, Label, Icon, ParentKey, `Order`, IsActive, CreatedAt, UpdatedAt) VALUES
(UUID(), 'anggaran', 'Anggaran', 'calculator', NULL, 1, 1, NOW(), NOW()),
(UUID(), 'master-data', 'Master Data', 'database', NULL, 2, 1, NOW(), NOW()),
(UUID(), 'transaksi', 'Transaksi', 'file-text', NULL, 3, 1, NOW(), NOW()),
(UUID(), 'monitoring', 'Monitoring', 'bar-chart', NULL, 4, 1, NOW(), NOW()),
(UUID(), 'admin', 'Admin', 'setting', NULL, 5, 1, NOW(), NOW())
ON DUPLICATE KEY UPDATE Label = VALUES(Label), Icon = VALUES(Icon);

-- Insert Anggaran sub-menus
INSERT INTO Menus (Id, `Key`, Label, Icon, ParentKey, `Order`, IsActive, CreatedAt, UpdatedAt) VALUES
(UUID(), 'anggaran-input', 'Input Anggaran', 'edit', 'anggaran', 1, 1, NOW(), NOW()),
(UUID(), 'anggaran-list', 'Daftar Anggaran', 'unordered-list', 'anggaran', 2, 1, NOW(), NOW())
ON DUPLICATE KEY UPDATE Label = VALUES(Label), Icon = VALUES(Icon);

-- Insert Master Data sub-menus
INSERT INTO Menus (Id, `Key`, Label, Icon, ParentKey, `Order`, IsActive, CreatedAt, UpdatedAt) VALUES
(UUID(), 'master-supplier', 'Supplier', 'shop', 'master-data', 1, 1, NOW(), NOW()),
(UUID(), 'master-item', 'Item', 'shopping', 'master-data', 2, 1, NOW(), NOW()),
(UUID(), 'master-taxrate', 'Tarif Pajak', 'percentage', 'master-data', 3, 1, NOW(), NOW())
ON DUPLICATE KEY UPDATE Label = VALUES(Label), Icon = VALUES(Icon);

-- Insert Transaksi sub-menus
INSERT INTO Menus (Id, `Key`, Label, Icon, ParentKey, `Order`, IsActive, CreatedAt, UpdatedAt) VALUES
(UUID(), 'transaksi-stpb', 'STPB', 'file-text', 'transaksi', 1, 1, NOW(), NOW())
ON DUPLICATE KEY UPDATE Label = VALUES(Label), Icon = VALUES(Icon);

-- Insert Monitoring sub-menus
INSERT INTO Menus (Id, `Key`, Label, Icon, ParentKey, `Order`, IsActive, CreatedAt, UpdatedAt) VALUES
(UUID(), 'monitoring-anggaran', 'Monitoring Anggaran', 'bar-chart', 'monitoring', 1, 1, NOW(), NOW())
ON DUPLICATE KEY UPDATE Label = VALUES(Label), Icon = VALUES(Icon);

-- Insert Admin sub-menus
INSERT INTO Menus (Id, `Key`, Label, Icon, ParentKey, `Order`, IsActive, CreatedAt, UpdatedAt) VALUES
(UUID(), 'admin-users', 'User Management', 'user', 'admin', 1, 1, NOW(), NOW()),
(UUID(), 'admin-roles', 'Role Management', 'team', 'admin', 2, 1, NOW(), NOW()),
(UUID(), 'admin-ppk-bendahara', 'PPK & Bendahara', 'user-check', 'admin', 3, 1, NOW(), NOW())
ON DUPLICATE KEY UPDATE Label = VALUES(Label), Icon = VALUES(Icon);

-- Grant all menus to Admin role
DELETE FROM RoleMenuPermissions WHERE RoleId = '00000000-0000-0000-0000-000000000010';

INSERT INTO RoleMenuPermissions (Id, RoleId, MenuId, MenuKey, IsVisible, CreatedAt, UpdatedAt)
SELECT 
    UUID(),
    '00000000-0000-0000-0000-000000000010',
    m.Id,
    m.`Key`,
    1,
    NOW(),
    NOW()
FROM Menus m
WHERE m.IsActive = 1;

-- Verify
SELECT 'Total Menus' AS Info, COUNT(*) AS Count FROM Menus WHERE IsActive = 1
UNION ALL
SELECT 'Admin Permissions', COUNT(*) FROM RoleMenuPermissions WHERE RoleId = '00000000-0000-0000-0000-000000000010';

-- Show menu structure
SELECT 
    COALESCE(m.ParentKey, 'ROOT') AS Parent,
    m.`Key`,
    m.Label,
    m.Icon,
    m.`Order`
FROM Menus m
WHERE m.IsActive = 1
ORDER BY COALESCE(m.ParentKey, '0'), m.`Order`;
