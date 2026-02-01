namespace FinApp.Core.Interfaces;

public interface IStpbPdfService
{
    Task<byte[]> GenerateStpbPdfAsync(Guid stpbId);
}
