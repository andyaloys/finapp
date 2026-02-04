-- Script untuk update format nomor SPTB yang sudah ada
-- Format lama: STPB-1/2026
-- Format baru: 0001/DJPPR/2026

UPDATE Stpbs 
SET NomorSTPB = CONCAT(
    LPAD(SUBSTRING_INDEX(SUBSTRING_INDEX(NomorSTPB, '-', -1), '/', 1), 4, '0'),
    '/DJPPR/',
    SUBSTRING_INDEX(NomorSTPB, '/', -1)
)
WHERE NomorSTPB LIKE 'STPB-%';

-- Verifikasi hasil update
SELECT Id, NomorSTPB, Tahun, TanggalSTPB, Status
FROM Stpbs
ORDER BY CreatedAt DESC;
