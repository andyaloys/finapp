-- Check role admin dan permissions
SELECT * FROM Roles WHERE Name = 'Admin';

-- Check existing permissions
SELECT * FROM RoleMenuPermissions WHERE RoleId = '00000000-0000-0000-0000-000000000010';

-- Check all menus
SELECT `Key`, Label, Route, ParentKey FROM Menus WHERE IsActive = 1 ORDER BY ParentKey, `Order`;
