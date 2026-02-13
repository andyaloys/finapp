using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.TaxRate;

namespace FinApp.Core.Interfaces;

public interface ITaxRateService
{
    Task<ApiResponse<List<TaxRateDto>>> GetAllAsync();
    Task<ApiResponse<List<TaxRateDto>>> GetAllActiveAsync();
    Task<ApiResponse<TaxRateDto>> GetByIdAsync(int id);
    Task<ApiResponse<TaxRateDto>> CreateAsync(CreateTaxRateDto dto);
    Task<ApiResponse<TaxRateDto>> UpdateAsync(int id, UpdateTaxRateDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}
