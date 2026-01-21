using AutoMapper;
using FinApp.Core.DTOs.Akun;
using FinApp.Core.DTOs.Common;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class AkunService : IAkunService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AkunService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<AkunDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var (items, totalCount) = await _unitOfWork.Akuns.GetPagedAsync(pageNumber, pageSize, searchTerm);
        return new PagedResult<AkunDto>
        {
            Items = _mapper.Map<IEnumerable<AkunDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<AkunDto> GetByIdAsync(Guid id)
    {
        var akun = await _unitOfWork.Akuns.GetByIdAsync(id);
        if (akun == null)
            throw new NotFoundException($"Akun with ID {id} not found");

        return _mapper.Map<AkunDto>(akun);
    }

    public async Task<IEnumerable<AkunDto>> GetBySubkomponenIdAsync(Guid subkomponenId)
    {
        var akuns = await _unitOfWork.Akuns.GetBySubkomponenIdAsync(subkomponenId);
        return _mapper.Map<IEnumerable<AkunDto>>(akuns);
    }

    public async Task<AkunDto> CreateAsync(CreateAkunDto createDto)
    {
        var akun = _mapper.Map<Akun>(createDto);
        await _unitOfWork.Akuns.AddAsync(akun);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AkunDto>(akun);
    }

    public async Task<AkunDto> UpdateAsync(Guid id, UpdateAkunDto updateDto)
    {
        var akun = await _unitOfWork.Akuns.GetByIdAsync(id);
        if (akun == null)
            throw new NotFoundException($"Akun with ID {id} not found");

        _mapper.Map(updateDto, akun);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AkunDto>(akun);
    }

    public async Task DeleteAsync(Guid id)
    {
        var akun = await _unitOfWork.Akuns.GetByIdAsync(id);
        if (akun == null)
            throw new NotFoundException($"Akun with ID {id} not found");

        await _unitOfWork.Akuns.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
