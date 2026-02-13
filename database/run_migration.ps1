# PowerShell script to run migration via MySQL.Data
Add-Type -Path "C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.11\System.Data.dll"

$connectionString = "Server=10.100.83.166;Port=3366;Database=finapp;Uid=root;Pwd=root123;AllowUserVariables=True"

Write-Host "Connecting to database..." -ForegroundColor Yellow

# Read SQL file
$sqlContent = Get-Content "c:\TI\NET\finapp\database\migration_tax_rates.sql" -Raw

# Split by semicolon and execute each statement
$statements = $sqlContent -split ';' | Where-Object { $_.Trim() -ne '' -and $_ -notmatch '^\s*--' }

try {
    # Load MySQL connector
    Add-Type -Path "C:\Program Files\MySQL\MySQL Connector NET 8.0.32\Assemblies\v4.5.2\MySql.Data.dll" -ErrorAction SilentlyContinue
    
    $connection = New-Object MySql.Data.MySqlClient.MySqlConnection($connectionString)
    $connection.Open()
    Write-Host "Connected successfully!" -ForegroundColor Green
    
    foreach ($statement in $statements) {
        $cleanStmt = $statement.Trim()
        if ($cleanStmt -and $cleanStmt -notmatch '^(--|/\*)') {
            Write-Host "Executing: $($cleanStmt.Substring(0, [Math]::Min(50, $cleanStmt.Length)))..." -ForegroundColor Cyan
            $command = $connection.CreateCommand()
            $command.CommandText = $cleanStmt
            $command.ExecuteNonQuery() | Out-Null
            Write-Host "Success!" -ForegroundColor Green
        }
    }
    
    $connection.Close()
    Write-Host "`nMigration completed successfully!" -ForegroundColor Green
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
    Write-Host "Please run the SQL file manually using a MySQL client." -ForegroundColor Yellow
}
