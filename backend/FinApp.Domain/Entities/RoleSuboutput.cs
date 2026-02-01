namespace FinApp.Domain.Entities;

public class RoleSuboutput : BaseEntity
{
    public Guid RoleId { get; set; }
    public string KodeSuboutput { get; set; } = string.Empty;
    
    // Navigation property
    public Role Role { get; set; } = null!;
}
