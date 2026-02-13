-- Grant ALL menu permissions to Admin role
-- Role Admin ID: 00000000-0000-0000-0000-000000000010

-- Delete existing permissions first
DELETE FROM RoleMenuPermissions WHERE RoleId = '00000000-0000-0000-0000-000000000010';

-- Grant access to all active menus
INSERT INTO RoleMenuPermissions (RoleId, MenuKey, IsVisible, CreatedAt)
SELECT 
    '00000000-0000-0000-0000-000000000010' AS RoleId,
    m.`Key` AS MenuKey,
    1 AS IsVisible,
    NOW() AS CreatedAt
FROM Menus m
WHERE m.IsActive = 1
AND NOT EXISTS (
    SELECT 1 FROM RoleMenuPermissions rmp 
    WHERE rmp.RoleId = '00000000-0000-0000-0000-000000000010' 
    AND rmp.MenuKey = m.`Key`
);

-- Verify
SELECT COUNT(*) AS TotalMenus FROM Menus WHERE IsActive = 1;
SELECT COUNT(*) AS TotalPermissions FROM RoleMenuPermissions WHERE RoleId = '00000000-0000-0000-0000-000000000010';

-- Show granted menus
SELECT rmp.MenuKey, m.Label, m.Route 
FROM RoleMenuPermissions rmp
JOIN Menus m ON m.`Key` = rmp.MenuKey
WHERE rmp.RoleId = '00000000-0000-0000-0000-000000000010'
ORDER BY m.ParentKey, m.`Order`;
