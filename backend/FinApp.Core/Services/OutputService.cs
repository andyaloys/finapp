using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Output;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class OutputService : IOutputService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OutputService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<OutputDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var (items, totalCount) = await _unitOfWork.Outputs.GetPagedAsync(pageNumber, pageSize, searchTerm);
        return new PagedResult<OutputDto>
        {
            Items = _mapper.Map<IEnumerable<OutputDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<OutputDto> GetByIdAsync(Guid id)
    {
        var output = await _unitOfWork.Outputs.GetByIdAsync(id);
        if (output == null)
            throw new NotFoundException($"Output with ID {id} not found");

        return _mapper.Map<OutputDto>(output);
    }

    public async Task<IEnumerable<OutputDto>> GetByKegiatanIdAsync(Guid kegiatanId)
    {
        var outputs = await _unitOfWork.Outputs.GetByKegiatanIdAsync(kegiatanId);
        return _mapper.Map<IEnumerable<OutputDto>>(outputs);
    }

    public async Task<OutputDto> CreateAsync(CreateOutputDto createDto)
    {
        var output = _mapper.Map<Output>(createDto);
        await _unitOfWork.Outputs.AddAsync(output);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OutputDto>(output);
    }

    public async Task<OutputDto> UpdateAsync(Guid id, UpdateOutputDto updateDto)
    {
        var output = await _unitOfWork.Outputs.GetByIdAsync(id);
        if (output == null)
            throw new NotFoundException($"Output with ID {id} not found");

        _mapper.Map(updateDto, output);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OutputDto>(output);
    }

    public async Task DeleteAsync(Guid id)
    {
        var output = await _unitOfWork.Outputs.GetByIdAsync(id);
        if (output == null)
            throw new NotFoundException($"Output with ID {id} not found");

        await _unitOfWork.Outputs.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
