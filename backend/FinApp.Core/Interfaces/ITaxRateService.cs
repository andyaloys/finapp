using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.TaxRate;
using FinApp.Domain.Entities;

namespace FinApp.Core.Interfaces;

public interface ITaxRateService
{
    Task<ApiResponse<List<TaxRateDto>>> GetAllAsync();
    Task<ApiResponse<List<TaxRateDto>>> GetAllActiveAsync();
    Task<ApiResponse<TaxRateDto>> GetByIdAsync(Guid id);
    Task<ApiResponse<TaxRateDto>> CreateAsync(CreateTaxRateDto dto);
    Task<ApiResponse<TaxRateDto>> UpdateAsync(Guid id, UpdateTaxRateDto dto);
    Task<ApiResponse<bool>> DeleteAsync(Guid id);
    Task<ApiResponse<List<TaxRateDto>>> GetByTaxTypeAsync(TaxType taxType);
}
