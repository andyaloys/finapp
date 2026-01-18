using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Stpb;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class StpbService : IStpbService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StpbService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<StpbDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null)
    {
        var (items, totalCount) = await _unitOfWork.Stpbs.GetPagedAsync(pageNumber, pageSize, searchTerm);
        
        var dtos = _mapper.Map<IEnumerable<StpbDto>>(items);

        return new PagedResult<StpbDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<StpbDto?> GetByIdAsync(int id)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(id);
        
        if (stpb == null)
        {
            throw new NotFoundException($"STPB with ID {id} not found");
        }

        return _mapper.Map<StpbDto>(stpb);
    }

    public async Task<StpbDto> CreateAsync(CreateStpbDto dto, int userId)
    {
        var existingStpb = await _unitOfWork.Stpbs.GetByNomorAsync(dto.NomorSTPB);
        if (existingStpb != null)
        {
            throw new ValidationException($"STPB dengan nomor {dto.NomorSTPB} sudah ada");
        }

        var stpb = _mapper.Map<Stpb>(dto);
        stpb.CreatedBy = userId;

        await _unitOfWork.Stpbs.AddAsync(stpb);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StpbDto>(stpb);
    }

    public async Task<StpbDto> UpdateAsync(int id, UpdateStpbDto dto)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(id);
        
        if (stpb == null)
        {
            throw new NotFoundException($"STPB with ID {id} not found");
        }

        // Check if nomor exists for other STPB
        var existingStpb = await _unitOfWork.Stpbs.GetByNomorAsync(dto.NomorSTPB);
        if (existingStpb != null && existingStpb.Id != id)
        {
            throw new ValidationException($"STPB dengan nomor {dto.NomorSTPB} sudah ada");
        }

        _mapper.Map(dto, stpb);
        stpb.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Stpbs.UpdateAsync(stpb);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StpbDto>(stpb);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var exists = await _unitOfWork.Stpbs.ExistsAsync(id);
        
        if (!exists)
        {
            throw new NotFoundException($"STPB with ID {id} not found");
        }

        await _unitOfWork.Stpbs.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<StpbDto>> GetByUserIdAsync(int userId)
    {
        var stpbs = await _unitOfWork.Stpbs.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<StpbDto>>(stpbs);
    }
}
