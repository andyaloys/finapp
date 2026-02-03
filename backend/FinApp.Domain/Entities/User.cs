namespace FinApp.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public Guid? PpkBendaharaId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Role Role { get; set; } = null!;
    public PpkBendahara? PpkBendahara { get; set; }
    public ICollection<Stpb> StpbList { get; set; } = new List<Stpb>();
}
