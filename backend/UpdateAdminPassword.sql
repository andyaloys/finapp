-- Update admin password to Admin123!
UPDATE Users 
SET PasswordHash = '$2a$11$VyFHd84rlCboUP.RPn25qeR7gw9i39bjj65fARIUvG6JkSjv.E2mW',
    UpdatedAt = CURRENT_TIMESTAMP(6)
WHERE Username = 'admin';

-- Verify
SELECT Username, Email, LEFT(PasswordHash, 30) AS HashPrefix FROM Users WHERE Username = 'admin';
