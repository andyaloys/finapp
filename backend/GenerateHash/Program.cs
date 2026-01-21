var password = "Admin123!";
var hash = BCrypt.Net.BCrypt.HashPassword(password);
Console.WriteLine("BCrypt hash for 'Admin123!':");
Console.WriteLine(hash);
