using AutoMapper;
using FinApp.Core.DTOs.PpkBendahara;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class PpkBendaharaService : IPpkBendaharaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PpkBendaharaService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PpkBendaharaDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.PpkBendaharas.GetAllAsync();
        return _mapper.Map<IEnumerable<PpkBendaharaDto>>(entities);
    }

    public async Task<IEnumerable<PpkBendaharaDto>> GetActiveAsync()
    {
        var entities = await _unitOfWork.PpkBendaharas.GetActiveAsync();
        return _mapper.Map<IEnumerable<PpkBendaharaDto>>(entities);
    }

    public async Task<PpkBendaharaDto?> GetByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.PpkBendaharas.GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException($"PPK/Bendahara with ID {id} not found");
        
        return _mapper.Map<PpkBendaharaDto>(entity);
    }

    public async Task<PpkBendaharaDto> CreateAsync(CreatePpkBendaharaDto dto)
    {
        // Check if NIP already exists
        var existing = await _unitOfWork.PpkBendaharas.GetByNIPAsync(dto.NIP);
        if (existing != null)
            throw new ValidationException($"NIP {dto.NIP} sudah terdaftar");

        var entity = _mapper.Map<PpkBendahara>(dto);
        await _unitOfWork.PpkBendaharas.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PpkBendaharaDto>(entity);
    }

    public async Task<PpkBendaharaDto> UpdateAsync(Guid id, UpdatePpkBendaharaDto dto)
    {
        var entity = await _unitOfWork.PpkBendaharas.GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException($"PPK/Bendahara with ID {id} not found");

        // Check if NIP already exists for other entity
        var existing = await _unitOfWork.PpkBendaharas.GetByNIPAsync(dto.NIP);
        if (existing != null && existing.Id != id)
            throw new ValidationException($"NIP {dto.NIP} sudah terdaftar");

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.PpkBendaharas.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PpkBendaharaDto>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exists = await _unitOfWork.PpkBendaharas.ExistsAsync(id);
        if (!exists)
            throw new NotFoundException($"PPK/Bendahara with ID {id} not found");

        await _unitOfWork.PpkBendaharas.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
