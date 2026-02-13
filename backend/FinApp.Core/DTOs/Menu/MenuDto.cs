namespace FinApp.Core.DTOs.Menu;

public class MenuDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? ParentKey { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
