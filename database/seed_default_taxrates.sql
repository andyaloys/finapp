-- ============================================
-- SEED DEFAULT TAX RATES
-- ============================================

-- Hapus data lama (opsional - comment jika mau keep existing data)
DELETE FROM TaxRates;

-- PPN (2 rates)
INSERT INTO TaxRates (Id, TaxType, Category, Rate, Description, ReferenceCode, IsDefault, IsActive, DisplayOrder, CreatedAt)
VALUES 
(UUID(), 'PPN', 'PPN 11%', 11.00, 'Pajak Pertambahan Nilai 11% (Default)', 'PP-NO-23-2024', 1, 1, 1, NOW()),
(UUID(), 'PPN', 'PPN 12%', 12.00, 'Pajak Pertambahan Nilai 12% (Berlaku 2025)', 'PP-NO-23-2024-V2', 0, 1, 2, NOW());

-- PPH21 (5 rates)
INSERT INTO TaxRates (Id, TaxType, Category, Rate, Description, ReferenceCode, IsDefault, IsActive, DisplayOrder, CreatedAt)
VALUES
(UUID(), 'PPH21', 'Tarif 0%', 0.00, 'PPh 21 Penghasilan s.d. Rp 60 juta/tahun', 'UU-36-2008-P17', 0, 1, 1, NOW()),
(UUID(), 'PPH21', 'Tarif 5%', 5.00, 'PPh 21 Penghasilan Rp 60-250 juta/tahun (Default)', 'UU-36-2008-P17', 1, 1, 2, NOW()),
(UUID(), 'PPH21', 'Tarif 15%', 15.00, 'PPh 21 Penghasilan Rp 250-500 juta/tahun', 'UU-36-2008-P17', 0, 1, 3, NOW()),
(UUID(), 'PPH21', 'Tarif 25%', 25.00, 'PPh 21 Penghasilan Rp 500 juta-5 M/tahun', 'UU-36-2008-P17', 0, 1, 4, NOW()),
(UUID(), 'PPH21', 'Tarif 30%', 30.00, 'PPh 21 Penghasilan di atas Rp 5 M/tahun', 'UU-36-2008-P17', 0, 1, 5, NOW());

-- PPH22 (4 rates)
INSERT INTO TaxRates (Id, TaxType, Category, Rate, Description, ReferenceCode, IsDefault, IsActive, DisplayOrder, CreatedAt)
VALUES
(UUID(), 'PPH22', 'Tarif 1.5%', 1.50, 'PPh 22 Pembelian Barang (Default)', 'PMK-34-2017', 1, 1, 1, NOW()),
(UUID(), 'PPH22', 'Tarif 0.25%', 0.25, 'PPh 22 Impor yang menggunakan API', 'PMK-34-2017-P2', 0, 1, 2, NOW()),
(UUID(), 'PPH22', 'Tarif 0.5%', 0.50, 'PPh 22 Penjualan hasil produksi', 'PMK-34-2017-P3', 0, 1, 3, NOW()),
(UUID(), 'PPH22', 'Tarif 7.5%', 7.50, 'PPh 22 Impor yang tidak dikuasai', 'PMK-34-2017-P4', 0, 1, 4, NOW());

-- PPH23 (4 rates)
INSERT INTO TaxRates (Id, TaxType, Category, Rate, Description, ReferenceCode, IsDefault, IsActive, DisplayOrder, CreatedAt)
VALUES
(UUID(), 'PPH23', 'Tarif 2%', 2.00, 'PPh 23 Sewa dan Jasa (Default)', 'UU-36-2008-P23', 1, 1, 1, NOW()),
(UUID(), 'PPH23', 'Tarif 15%', 15.00, 'PPh 23 Dividen, bunga, royalti', 'UU-36-2008-P23-A2', 0, 1, 2, NOW()),
(UUID(), 'PPH23', 'Tarif 10%', 10.00, 'PPh 23 Hadiah dan penghargaan', 'PMK-141-2015', 0, 1, 3, NOW()),
(UUID(), 'PPH23', 'Tarif 0.5%', 0.50, 'PPh 23 Jasa konstruksi', 'PP-9-2022', 0, 1, 4, NOW());

-- Verify
SELECT 'Data default tax rates berhasil di-seed!' as Status;
SELECT TaxType, COUNT(*) as Total, SUM(IsDefault) as TotalDefault 
FROM TaxRates 
GROUP BY TaxType 
ORDER BY TaxType;

SELECT * FROM TaxRates ORDER BY TaxType, DisplayOrder;
