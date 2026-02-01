-- Add UpdatedAt column to RoleSuboutputs table
ALTER TABLE `RoleSuboutputs` 
ADD COLUMN `UpdatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6);

-- Verify
DESCRIBE `RoleSuboutputs`;
