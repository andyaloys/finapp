-- Add AlasanDikembalikan column to Stpbs table
ALTER TABLE Stpbs 
ADD COLUMN AlasanDikembalikan TEXT NULL;

-- Update existing records to set NULL (default)
UPDATE Stpbs 
SET AlasanDikembalikan = NULL 
WHERE Status != 2;  -- 2 = Dikembalikan

SELECT 'AlasanDikembalikan column added successfully' as Result;
