using System;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=10.100.83.166;Port=3366;Database=finapp;Uid=root;Pwd=root123;";
        
        string sql = @"
CREATE TABLE IF NOT EXISTS TaxRates (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TaxCode VARCHAR(20) NOT NULL UNIQUE,
    TaxName VARCHAR(100) NOT NULL,
    Rate DECIMAL(5,2) NOT NULL,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    CreatedAt DATETIME(6) NOT NULL,
    UpdatedAt DATETIME(6) NULL,
    INDEX IX_TaxRates_TaxCode (TaxCode),
    INDEX IX_TaxRates_IsActive (IsActive)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT IGNORE INTO TaxRates (TaxCode, TaxName, Rate, IsActive, CreatedAt) VALUES
('PPN', 'Pajak Pertambahan Nilai', 11.00, 1, NOW()),
('PPH21', 'PPh Pasal 21', 2.50, 1, NOW()),
('PPH22', 'PPh Pasal 22', 1.50, 1, NOW()),
('PPH23', 'PPh Pasal 23', 2.00, 1, NOW());";

        try
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            Console.WriteLine("Connected to database!");
            
            using var command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();
            
            Console.WriteLine("TaxRates table created and data seeded successfully!");
            
            // Verify
            using var verifyCmd = new MySqlCommand("SELECT * FROM TaxRates", connection);
            using var reader = verifyCmd.ExecuteReader();
            
            Console.WriteLine("\nData in TaxRates table:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Id"],-5} {reader["TaxCode"],-10} {reader["TaxName"],-30} {reader["Rate"]}%");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
