using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Kegiatan;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class KegiatanService : IKegiatanService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public KegiatanService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<KegiatanDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var (items, totalCount) = await _unitOfWork.Kegiatans.GetPagedAsync(pageNumber, pageSize, searchTerm);
        return new PagedResult<KegiatanDto>
        {
            Items = _mapper.Map<IEnumerable<KegiatanDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<KegiatanDto> GetByIdAsync(Guid id)
    {
        var kegiatan = await _unitOfWork.Kegiatans.GetByIdAsync(id);
        if (kegiatan == null)
            throw new NotFoundException($"Kegiatan with ID {id} not found");

        return _mapper.Map<KegiatanDto>(kegiatan);
    }

    public async Task<IEnumerable<KegiatanDto>> GetByProgramIdAsync(Guid programId)
    {
        var kegiatans = await _unitOfWork.Kegiatans.GetByProgramIdAsync(programId);
        return _mapper.Map<IEnumerable<KegiatanDto>>(kegiatans);
    }

    public async Task<KegiatanDto> CreateAsync(CreateKegiatanDto createDto)
    {
        var kegiatan = _mapper.Map<Kegiatan>(createDto);
        await _unitOfWork.Kegiatans.AddAsync(kegiatan);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<KegiatanDto>(kegiatan);
    }

    public async Task<KegiatanDto> UpdateAsync(Guid id, UpdateKegiatanDto updateDto)
    {
        var kegiatan = await _unitOfWork.Kegiatans.GetByIdAsync(id);
        if (kegiatan == null)
            throw new NotFoundException($"Kegiatan with ID {id} not found");

        _mapper.Map(updateDto, kegiatan);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<KegiatanDto>(kegiatan);
    }

    public async Task DeleteAsync(Guid id)
    {
        var kegiatan = await _unitOfWork.Kegiatans.GetByIdAsync(id);
        if (kegiatan == null)
            throw new NotFoundException($"Kegiatan with ID {id} not found");

        await _unitOfWork.Kegiatans.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
