-- Seed all menu permissions for Admin role
-- Admin role ID: 00000000-0000-0000-0000-000000000010

-- Get Menu IDs
SET @menu1 = (SELECT Id FROM Menus WHERE `Key` = 'transaksi');
SET @menu2 = (SELECT Id FROM Menus WHERE `Key` = 'transaksi-stpb');
SET @menu3 = (SELECT Id FROM Menus WHERE `Key` = 'anggaran');
SET @menu4 = (SELECT Id FROM Menus WHERE `Key` = 'anggaran-list');
SET @menu5 = (SELECT Id FROM Menus WHERE `Key` = 'monitoring');
SET @menu6 = (SELECT Id FROM Menus WHERE `Key` = 'master-data');
SET @menu7 = (SELECT Id FROM Menus WHERE `Key` = 'master-ppkbendahara');
SET @menu8 = (SELECT Id FROM Menus WHERE `Key` = 'administration');
SET @menu9 = (SELECT Id FROM Menus WHERE `Key` = 'admin-users');
SET @menu10 = (SELECT Id FROM Menus WHERE `Key` = 'admin-roles');

-- Insert permissions for Admin role (all menus)
INSERT INTO RoleMenuPermissions (Id, RoleId, MenuKey, IsVisible, MenuId, CreatedAt, UpdatedAt)
VALUES 
(UUID(), '00000000-0000-0000-0000-000000000010', 'transaksi', 1, @menu1, NOW(), NOW()),
(UUID(), '00000000-0000-0000-0000-000000000010', 'transaksi-stpb', 1, @menu2, NOW(), NOW()),
(UUID(), '00000000-0000-0000-0000-000000000010', 'anggaran', 1, @menu3, NOW(), NOW()),
(UUID(), '00000000-0000-0000-0000-000000000010', 'anggaran-list', 1, @menu4, NOW(), NOW()),
(UUID(), '00000000-0000-0000-0000-000000000010', 'monitoring', 1, @menu5, NOW(), NOW()),
(UUID(), '00000000-0000-0000-0000-000000000010', 'master-data', 1, @menu6, NOW(), NOW()),
(UUID(), '00000000-0000-0000-0000-000000000010', 'master-ppkbendahara', 1, @menu7, NOW(), NOW()),
(UUID(), '00000000-0000-0000-0000-000000000010', 'administration', 1, @menu8, NOW(), NOW()),
(UUID(), '00000000-0000-0000-0000-000000000010', 'admin-users', 1, @menu9, NOW(), NOW()),
(UUID(), '00000000-0000-0000-0000-000000000010', 'admin-roles', 1, @menu10, NOW(), NOW());

SELECT 'Admin permissions seeded successfully!' AS Result;
