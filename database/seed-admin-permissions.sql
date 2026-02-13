USE finapp;

-- Insert all menu permissions for Admin role
-- Admin role ID: 00000000-0000-0000-0000-000000000010

INSERT IGNORE INTO RoleMenuPermissions (Id, RoleId, MenuKey, IsVisible, CreatedAt, UpdatedAt)
SELECT 
    UUID() as Id,
    '00000000-0000-0000-0000-000000000010' as RoleId,
    m.`Key` as MenuKey,
    1 as IsVisible,
    NOW() as CreatedAt,
    NOW() as UpdatedAt
FROM Menus m
WHERE NOT EXISTS (
    SELECT 1 FROM RoleMenuPermissions 
    WHERE RoleId = '00000000-0000-0000-0000-000000000010' 
    AND MenuKey = m.`Key`
);

SELECT COUNT(*) as 'Admin Permissions Count' 
FROM RoleMenuPermissions 
WHERE RoleId = '00000000-0000-0000-0000-000000000010';
