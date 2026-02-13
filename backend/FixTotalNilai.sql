-- Script untuk memperbaiki TotalNilai yang salah di table STPB
-- Mengubah dari NilaiBersih (yang double-count) ke JumlahHarga yang benar

UPDATE STPB s
SET s.TotalNilai = (
    SELECT SUM(sd.JumlahHarga)
    FROM StpbDetails sd
    WHERE sd.StpbId = s.Id
);

-- Verifikasi hasil
SELECT 
    s.NomorSTPB,
    s.TotalNilai as TotalNilaiSekarang,
    SUM(sd.JumlahHarga) as TotalSeharusnya,
    COUNT(sd.Id) as JumlahDetail
FROM STPB s
LEFT JOIN StpbDetails sd ON s.Id = sd.StpbId
GROUP BY s.Id, s.NomorSTPB, s.TotalNilai;
