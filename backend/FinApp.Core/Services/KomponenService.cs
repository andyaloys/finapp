using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Komponen;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class KomponenService : IKomponenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public KomponenService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<KomponenDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var (items, totalCount) = await _unitOfWork.Komponens.GetPagedAsync(pageNumber, pageSize, searchTerm);
        return new PagedResult<KomponenDto>
        {
            Items = _mapper.Map<IEnumerable<KomponenDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<KomponenDto> GetByIdAsync(Guid id)
    {
        var komponen = await _unitOfWork.Komponens.GetByIdAsync(id);
        if (komponen == null)
            throw new NotFoundException($"Komponen with ID {id} not found");

        return _mapper.Map<KomponenDto>(komponen);
    }

    public async Task<IEnumerable<KomponenDto>> GetBySuboutputIdAsync(Guid suboutputId)
    {
        var komponens = await _unitOfWork.Komponens.GetBySuboutputIdAsync(suboutputId);
        return _mapper.Map<IEnumerable<KomponenDto>>(komponens);
    }

    public async Task<KomponenDto> CreateAsync(CreateKomponenDto createDto)
    {
        var komponen = _mapper.Map<Komponen>(createDto);
        await _unitOfWork.Komponens.AddAsync(komponen);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<KomponenDto>(komponen);
    }

    public async Task<KomponenDto> UpdateAsync(Guid id, UpdateKomponenDto updateDto)
    {
        var komponen = await _unitOfWork.Komponens.GetByIdAsync(id);
        if (komponen == null)
            throw new NotFoundException($"Komponen with ID {id} not found");

        _mapper.Map(updateDto, komponen);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<KomponenDto>(komponen);
    }

    public async Task DeleteAsync(Guid id)
    {
        var komponen = await _unitOfWork.Komponens.GetByIdAsync(id);
        if (komponen == null)
            throw new NotFoundException($"Komponen with ID {id} not found");

        await _unitOfWork.Komponens.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
