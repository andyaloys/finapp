namespace FinApp.Domain.Entities;

public class SequenceNumber : BaseEntity
{
    public string EntityType { get; set; } = string.Empty;
    public int Year { get; set; }
    public int LastNumber { get; set; }
}
