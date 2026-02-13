-- Quick fix via direct SQL query
-- Recalculate TotalNilai untuk STPB 0011 dan 0012

-- STPB 0011/DJPPR/2026
UPDATE STPB 
SET TotalNilai = (SELECT SUM(JumlahHarga) FROM StpbDetails WHERE StpbId = '08de6471-ecfe-4ece-83e0-f20def108940')
WHERE Id = '08de6471-ecfe-4ece-83e0-f20def108940';

-- STPB 0012/DJPPR/2026
UPDATE STPB 
SET TotalNilai = (SELECT SUM(JumlahHarga) FROM StpbDetails WHERE StpbId = '08de6472-470b-45ec-8b8f-21a19dd54d78')
WHERE Id = '08de6472-470b-45ec-8b8f-21a19dd54d78';

-- Verify hasil
SELECT 
    s.NomorSTPB,
    s.TotalNilai as TotalSekarang,
    (SELECT SUM(sd.JumlahHarga) FROM StpbDetails sd WHERE sd.StpbId = s.Id) as TotalSeharusnya
FROM STPB s
WHERE s.NomorSTPB IN ('0011/DJPPR/2026', '0012/DJPPR/2026');
