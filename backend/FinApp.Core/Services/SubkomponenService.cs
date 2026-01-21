using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Subkomponen;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class SubkomponenService : ISubkomponenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubkomponenService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<SubkomponenDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var (items, totalCount) = await _unitOfWork.Subkomponens.GetPagedAsync(pageNumber, pageSize, searchTerm);
        return new PagedResult<SubkomponenDto>
        {
            Items = _mapper.Map<IEnumerable<SubkomponenDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<SubkomponenDto> GetByIdAsync(Guid id)
    {
        var subkomponen = await _unitOfWork.Subkomponens.GetByIdAsync(id);
        if (subkomponen == null)
            throw new NotFoundException($"Subkomponen with ID {id} not found");

        return _mapper.Map<SubkomponenDto>(subkomponen);
    }

    public async Task<IEnumerable<SubkomponenDto>> GetByKomponenIdAsync(Guid komponenId)
    {
        var subkomponens = await _unitOfWork.Subkomponens.GetByKomponenIdAsync(komponenId);
        return _mapper.Map<IEnumerable<SubkomponenDto>>(subkomponens);
    }

    public async Task<SubkomponenDto> CreateAsync(CreateSubkomponenDto createDto)
    {
        var subkomponen = _mapper.Map<Subkomponen>(createDto);
        await _unitOfWork.Subkomponens.AddAsync(subkomponen);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SubkomponenDto>(subkomponen);
    }

    public async Task<SubkomponenDto> UpdateAsync(Guid id, UpdateSubkomponenDto updateDto)
    {
        var subkomponen = await _unitOfWork.Subkomponens.GetByIdAsync(id);
        if (subkomponen == null)
            throw new NotFoundException($"Subkomponen with ID {id} not found");

        _mapper.Map(updateDto, subkomponen);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SubkomponenDto>(subkomponen);
    }

    public async Task DeleteAsync(Guid id)
    {
        var subkomponen = await _unitOfWork.Subkomponens.GetByIdAsync(id);
        if (subkomponen == null)
            throw new NotFoundException($"Subkomponen with ID {id} not found");

        await _unitOfWork.Subkomponens.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
