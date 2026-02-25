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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTaxRateDto> _createValidator;
    private readonly IValidator<UpdateTaxRateDto> _updateValidator;

    public TaxRateService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateTaxRateDto> createValidator,
        IValidator<UpdateTaxRateDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ApiResponse<List<TaxRateDto>>> GetAllAsync()
    {
        var taxRates = await _unitOfWork.TaxRates.GetAllAsync();
        var dtos = _mapper.Map<List<TaxRateDto>>(taxRates);
        return ApiResponse<List<TaxRateDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<List<TaxRateDto>>> GetAllActiveAsync()
    {
        var taxRates = await _unitOfWork.TaxRates.GetActiveTaxRatesAsync();
        var dtos = _mapper.Map<List<TaxRateDto>>(taxRates);
        return ApiResponse<List<TaxRateDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<TaxRateDto>> GetByIdAsync(Guid id)
    {
        var taxRate = await _unitOfWork.TaxRates.GetByIdAsync(id);
        
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

        // Check if IsDefault is true, ensure no other default exists for the same TaxType
        if (dto.IsDefault)
        {
            var existingDefault = await _unitOfWork.TaxRates.GetDefaultByTaxTypeAsync(dto.TaxType);
            if (existingDefault != null)
            {
                existingDefault.IsDefault = false;
            }
        }

        var taxRate = _mapper.Map<TaxRate>(dto);

        await _unitOfWork.TaxRates.AddAsync(taxRate);
        await _unitOfWork.SaveChangesAsync();

        var resultDto = _mapper.Map<TaxRateDto>(taxRate);
        return ApiResponse<TaxRateDto>.SuccessResponse(resultDto, "Tarif pajak berhasil ditambahkan");
    }

    public async Task<ApiResponse<TaxRateDto>> UpdateAsync(Guid id, UpdateTaxRateDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors.First().ErrorMessage);
        }

        var taxRate = await _unitOfWork.TaxRates.GetByIdAsync(id);
        
        if (taxRate == null)
        {
            throw new NotFoundException($"Tarif pajak dengan ID {id} tidak ditemukan");
        }

        // Check if IsDefault is changed to true
        if (dto.IsDefault && !taxRate.IsDefault)
        {
            var existingDefault = await _unitOfWork.TaxRates.GetDefaultByTaxTypeAsync(taxRate.TaxType);
            if (existingDefault != null && existingDefault.Id != id)
            {
                existingDefault.IsDefault = false;
            }
        }

        _mapper.Map(dto, taxRate);

        await _unitOfWork.SaveChangesAsync();

        var resultDto = _mapper.Map<TaxRateDto>(taxRate);
        return ApiResponse<TaxRateDto>.SuccessResponse(resultDto, "Tarif pajak berhasil diupdate");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        var taxRate = await _unitOfWork.TaxRates.GetByIdAsync(id);
        
        if (taxRate == null)
        {
            throw new NotFoundException($"Tarif pajak dengan ID {id} tidak ditemukan");
        }

        await _unitOfWork.TaxRates.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Tarif pajak berhasil dihapus");
    }

    public async Task<ApiResponse<List<TaxRateDto>>> GetByTaxTypeAsync(TaxType taxType)
    {
        var taxRates = await _unitOfWork.TaxRates.GetByTaxTypeAsync(taxType);
        var dtos = _mapper.Map<List<TaxRateDto>>(taxRates);
        return ApiResponse<List<TaxRateDto>>.SuccessResponse(dtos);
    }
}
