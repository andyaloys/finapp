using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Item;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class ItemService : IItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<ItemDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var (items, totalCount) = await _unitOfWork.Items.GetPagedAsync(pageNumber, pageSize, searchTerm);
        return new PagedResult<ItemDto>
        {
            Items = _mapper.Map<IEnumerable<ItemDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<ItemDto> GetByIdAsync(Guid id)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        if (item == null)
            throw new NotFoundException($"Item with ID {id} not found");

        return _mapper.Map<ItemDto>(item);
    }

    public async Task<IEnumerable<ItemDto>> GetByAkunIdAsync(Guid akunId)
    {
        var items = await _unitOfWork.Items.GetByAkunIdAsync(akunId);
        return _mapper.Map<IEnumerable<ItemDto>>(items);
    }

    public async Task<ItemDto> CreateAsync(CreateItemDto createDto)
    {
        var item = _mapper.Map<Item>(createDto);
        await _unitOfWork.Items.AddAsync(item);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ItemDto>(item);
    }

    public async Task<ItemDto> UpdateAsync(Guid id, UpdateItemDto updateDto)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        if (item == null)
            throw new NotFoundException($"Item with ID {id} not found");

        _mapper.Map(updateDto, item);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ItemDto>(item);
    }

    public async Task DeleteAsync(Guid id)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        if (item == null)
            throw new NotFoundException($"Item with ID {id} not found");

        await _unitOfWork.Items.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
