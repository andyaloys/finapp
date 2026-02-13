using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.TaxRate;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FluentValidation;
using ValidationException = FinApp.Core.Exceptions.ValidationException;

namespace FinApp.Core.Services;

public class TaxRateService : ITaxRateService
{
    private readonly ITaxRateRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTaxRateDto> _createValidator;
    private readonly IValidator<UpdateTaxRateDto> _updateValidator;

    public TaxRateService(
        ITaxRateRepository repository,
        IMapper mapper,
        IValidator<CreateTaxRateDto> createValidator,
        IValidator<UpdateTaxRateDto> updateValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ApiResponse<List<TaxRateDto>>> GetAllAsync()
    {
        var taxRates = await _repository.GetAllAsync();
        var dtos = _mapper.Map<List<TaxRateDto>>(taxRates);
        return ApiResponse<List<TaxRateDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<List<TaxRateDto>>> GetAllActiveAsync()
    {
        var taxRates = await _repository.GetAllActiveAsync();
        var dtos = _mapper.Map<List<TaxRateDto>>(taxRates);
        return ApiResponse<List<TaxRateDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<TaxRateDto>> GetByIdAsync(int id)
    {
        var taxRate = await _repository.GetByIdAsync(id);
        
        if (taxRate == null)
        {
            throw new NotFoundException($"Tarif pajak dengan ID {id} tidak ditemukan");
        }

        var dto = _mapper.Map<TaxRateDto>(taxRate);
        return ApiResponse<TaxRateDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<TaxRateDto>> CreateAsync(CreateTaxRateDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors.First().ErrorMessage);
        }

        // Check if tax code already exists
        if (await _repository.ExistsByCodeAsync(dto.TaxCode))
        {
            throw new ValidationException($"Kode pajak '{dto.TaxCode}' sudah digunakan");
        }

        var taxRate = _mapper.Map<TaxRate>(dto);
        taxRate.CreatedAt = DateTime.UtcNow;
        taxRate.IsActive = true;

        await _repository.AddAsync(taxRate);
        await _repository.SaveChangesAsync();

        var resultDto = _mapper.Map<TaxRateDto>(taxRate);
        return ApiResponse<TaxRateDto>.SuccessResponse(resultDto, "Tarif pajak berhasil ditambahkan");
    }

    public async Task<ApiResponse<TaxRateDto>> UpdateAsync(int id, UpdateTaxRateDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors.First().ErrorMessage);
        }

        var taxRate = await _repository.GetByIdAsync(id);
        
        if (taxRate == null)
        {
            throw new NotFoundException($"Tarif pajak dengan ID {id} tidak ditemukan");
        }

        _mapper.Map(dto, taxRate);
        taxRate.UpdatedAt = DateTime.UtcNow;

        _repository.Update(taxRate);
        await _repository.SaveChangesAsync();

        var resultDto = _mapper.Map<TaxRateDto>(taxRate);
        return ApiResponse<TaxRateDto>.SuccessResponse(resultDto, "Tarif pajak berhasil diupdate");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var taxRate = await _repository.GetByIdAsync(id);
        
        if (taxRate == null)
        {
            throw new NotFoundException($"Tarif pajak dengan ID {id} tidak ditemukan");
        }

        _repository.Delete(taxRate);
        await _repository.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Tarif pajak berhasil dihapus");
    }
}
