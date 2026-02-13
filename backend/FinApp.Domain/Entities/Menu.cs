namespace FinApp.Domain.Entities;

public class Menu : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? ParentKey { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation
    public ICollection<RoleMenuPermission> RoleMenuPermissions { get; set; } = new List<RoleMenuPermission>();
}
