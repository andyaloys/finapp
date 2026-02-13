namespace FinApp.Domain.Entities;

public class RoleMenuPermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public string MenuKey { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    
    // Navigation properties
    public Role Role { get; set; } = null!;
    public Menu Menu { get; set; } = null!;
}
