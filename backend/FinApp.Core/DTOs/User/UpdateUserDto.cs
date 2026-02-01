namespace FinApp.Core.DTOs.User
{
    public class UpdateUserDto
    {
        public string? Password { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
