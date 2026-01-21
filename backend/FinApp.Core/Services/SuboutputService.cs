using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Suboutput;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class SuboutputService : ISuboutputService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SuboutputService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<SuboutputDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var (items, totalCount) = await _unitOfWork.Suboutputs.GetPagedAsync(pageNumber, pageSize, searchTerm);
        return new PagedResult<SuboutputDto>
        {
            Items = _mapper.Map<IEnumerable<SuboutputDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<SuboutputDto> GetByIdAsync(Guid id)
    {
        var suboutput = await _unitOfWork.Suboutputs.GetByIdAsync(id);
        if (suboutput == null)
            throw new NotFoundException($"Suboutput with ID {id} not found");

        return _mapper.Map<SuboutputDto>(suboutput);
    }

    public async Task<IEnumerable<SuboutputDto>> GetByOutputIdAsync(Guid outputId)
    {
        var suboutputs = await _unitOfWork.Suboutputs.GetByOutputIdAsync(outputId);
        return _mapper.Map<IEnumerable<SuboutputDto>>(suboutputs);
    }

    public async Task<SuboutputDto> CreateAsync(CreateSuboutputDto createDto)
    {
        var suboutput = _mapper.Map<Suboutput>(createDto);
        await _unitOfWork.Suboutputs.AddAsync(suboutput);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SuboutputDto>(suboutput);
    }

    public async Task<SuboutputDto> UpdateAsync(Guid id, UpdateSuboutputDto updateDto)
    {
        var suboutput = await _unitOfWork.Suboutputs.GetByIdAsync(id);
        if (suboutput == null)
            throw new NotFoundException($"Suboutput with ID {id} not found");

        _mapper.Map(updateDto, suboutput);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SuboutputDto>(suboutput);
    }

    public async Task DeleteAsync(Guid id)
    {
        var suboutput = await _unitOfWork.Suboutputs.GetByIdAsync(id);
        if (suboutput == null)
            throw new NotFoundException($"Suboutput with ID {id} not found");

        await _unitOfWork.Suboutputs.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
