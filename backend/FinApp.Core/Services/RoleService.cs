using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Role;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<RoleDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var (items, totalCount) = await _unitOfWork.Roles.GetPagedAsync(pageNumber, pageSize, searchTerm);
        var dtos = _mapper.Map<IEnumerable<RoleDto>>(items);

        return new PagedResult<RoleDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _unitOfWork.Roles.GetAllAsync();
        return _mapper.Map<List<RoleDto>>(roles);
    }

    public async Task<RoleDto?> GetByIdAsync(Guid id)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        return role == null ? null : _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        // Check if role name already exists
        var existingRole = await _unitOfWork.Roles.GetByNameAsync(dto.Name);
        if (existingRole != null)
        {
            throw new ValidationException($"Role dengan nama '{dto.Name}' sudah ada");
        }

        var role = _mapper.Map<Role>(dto);
        role.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Roles.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> UpdateAsync(Guid id, UpdateRoleDto dto)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        if (role == null)
        {
            throw new NotFoundException($"Role dengan ID {id} tidak ditemukan");
        }

        // Check if new name conflicts with existing role
        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != role.Name)
        {
            var existingRole = await _unitOfWork.Roles.GetByNameAsync(dto.Name);
            if (existingRole != null)
            {
                throw new ValidationException($"Role dengan nama '{dto.Name}' sudah ada");
            }
            role.Name = dto.Name;
        }

        if (dto.Description != null)
        {
            role.Description = dto.Description;
        }

        if (dto.IsAdmin.HasValue)
        {
            role.IsAdmin = dto.IsAdmin.Value;
        }

        role.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Roles.UpdateAsync(role);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoleDto>(role);
    }

    public async Task DeleteAsync(Guid id)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        if (role == null)
        {
            throw new NotFoundException($"Role dengan ID {id} tidak ditemukan");
        }

        if (role.IsAdmin)
        {
            throw new ValidationException("Role Admin tidak dapat dihapus");
        }

        // Check if role is being used by any users
        var users = await _unitOfWork.Users.GetAllAsync();
        if (users.Any(u => u.RoleId == id))
        {
            throw new ValidationException("Role tidak dapat dihapus karena masih digunakan oleh user");
        }

        // Delete role suboutputs first
        await _unitOfWork.RoleSuboutputs.DeleteByRoleIdAsync(id);
        
        await _unitOfWork.Roles.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<RoleSuboutputDto>> GetRoleSuboutputsAsync(Guid roleId)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
        if (role == null)
        {
            throw new NotFoundException($"Role dengan ID {roleId} tidak ditemukan");
        }

        var roleSuboutputs = await _unitOfWork.RoleSuboutputs.GetByRoleIdAsync(roleId);
        return _mapper.Map<List<RoleSuboutputDto>>(roleSuboutputs);
    }

    public async Task AssignSuboutputsAsync(Guid roleId, AssignSuboutputsDto dto)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
        if (role == null)
        {
            throw new NotFoundException($"Role dengan ID {roleId} tidak ditemukan");
        }

        // Delete existing assignments
        await _unitOfWork.RoleSuboutputs.DeleteByRoleIdAsync(roleId);

        // Add new assignments
        foreach (var kodeSuboutput in dto.KodeSuboutputs)
        {
            var roleSuboutput = new RoleSuboutput
            {
                RoleId = roleId,
                KodeSuboutput = kodeSuboutput,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.RoleSuboutputs.AddAsync(roleSuboutput);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
