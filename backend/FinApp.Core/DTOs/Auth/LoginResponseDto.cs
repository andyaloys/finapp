using System.Text.Json.Serialization;

namespace FinApp.Core.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    
    [JsonPropertyName("menuPermissions")]
    public List<string> MenuPermissions { get; set; } = new();
    
    public DateTime ExpiresAt { get; set; }
}
