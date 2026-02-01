namespace FinApp.Core.DTOs.Role;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsAdmin { get; set; }
}

public class UpdateRoleDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsAdmin { get; set; }
}

public class RoleSuboutputDto
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string KodeSuboutput { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class AssignSuboutputsDto
{
    public List<string> KodeSuboutputs { get; set; } = new();
}
