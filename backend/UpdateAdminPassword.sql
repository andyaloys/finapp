-- Update admin password to Admin123!
UPDATE Users 
SET PasswordHash = '$2a$11$6mtygzX7D/O53nh87B5W3O3ro/wBXjAF64kFyYrthx5vpWsg9vfmO',
    UpdatedAt = CURRENT_TIMESTAMP(6)
WHERE Username = 'admin';
