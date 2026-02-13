-- Check admin user
SELECT 
    Id,
    Username,
    Email,
    IsActive,
    RoleId,
    LEFT(PasswordHash, 60) AS PasswordHash,
    CreatedAt,
    UpdatedAt
FROM Users 
WHERE Username = 'admin';

-- Check if password hash matches
SELECT 
    CASE 
        WHEN PasswordHash = '$2a$11$VyFHd84rlCboUP.RPn25qeR7gw9i39bjj65fARIUvG6JkSjv.E2mW' 
        THEN 'MATCH' 
        ELSE 'NOT MATCH' 
    END AS HashStatus
FROM Users 
WHERE Username = 'admin';
