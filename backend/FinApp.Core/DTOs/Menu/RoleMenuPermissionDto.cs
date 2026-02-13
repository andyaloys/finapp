namespace FinApp.Core.DTOs.Menu;

public class RoleMenuPermissionDto
{
    public Guid RoleId { get; set; }
    public List<string> MenuKeys { get; set; } = new();
}
