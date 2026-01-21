using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Item;

namespace FinApp.Core.Interfaces;

public interface IItemService
{
    Task<PagedResult<ItemDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<ItemDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ItemDto>> GetByAkunIdAsync(Guid akunId);
    Task<ItemDto> CreateAsync(CreateItemDto createDto);
    Task<ItemDto> UpdateAsync(Guid id, UpdateItemDto updateDto);
    Task DeleteAsync(Guid id);
}
