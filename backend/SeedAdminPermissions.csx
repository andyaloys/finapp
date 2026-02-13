// Quick script to seed admin permissions
using MySqlConnector;

var connectionString = "Server=localhost;Port=3306;Database=finapp;User=root;Password=root;";
using var connection = new MySqlConnection(connectionString);
await connection.OpenAsync();

var insertSql = @"
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
);";

using var command = new MySqlCommand(insertSql, connection);
var rowsAffected = await command.ExecuteNonQueryAsync();

Console.WriteLine($"Seeded {rowsAffected} admin permissions successfully!");
