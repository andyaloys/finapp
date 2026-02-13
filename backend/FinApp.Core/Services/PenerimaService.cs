using AutoMapper;
using FinApp.Core.DTOs.Penerima;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class PenerimaService : IPenerimaService
{
    private readonly IPenerimaRepository _penerimaRepository;
    private readonly IMapper _mapper;

    public PenerimaService(IPenerimaRepository penerimaRepository, IMapper mapper)
    {
        _penerimaRepository = penerimaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PenerimaDto>> GetAllAsync()
    {
        var penerimas = await _penerimaRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PenerimaDto>>(penerimas);
    }

    public async Task<IEnumerable<PenerimaDto>> GetAllActiveAsync()
    {
        var penerimas = await _penerimaRepository.GetAllActiveAsync();
        return _mapper.Map<IEnumerable<PenerimaDto>>(penerimas);
    }

    public async Task<PenerimaDto?> GetByIdAsync(int id)
    {
        var penerima = await _penerimaRepository.GetByIdAsync(id);
        return penerima == null ? null : _mapper.Map<PenerimaDto>(penerima);
    }

    public async Task<PenerimaDto> CreateAsync(CreatePenerimaDto dto)
    {
        // Check if nama already exists
        if (await _penerimaRepository.ExistsByNamaAsync(dto.Nama))
        {
            throw new ValidationException($"Penerima dengan nama '{dto.Nama}' sudah ada");
        }

        var penerima = _mapper.Map<Penerima>(dto);
        penerima.CreatedAt = DateTime.UtcNow;
        penerima.IsActive = true;

        await _penerimaRepository.AddAsync(penerima);
        await _penerimaRepository.SaveChangesAsync();

        return _mapper.Map<PenerimaDto>(penerima);
    }

    public async Task<PenerimaDto> UpdateAsync(int id, UpdatePenerimaDto dto)
    {
        var penerima = await _penerimaRepository.GetByIdAsync(id);
        if (penerima == null)
        {
            throw new NotFoundException($"Penerima dengan ID {id} tidak ditemukan");
        }

        // Check if nama already exists (excluding current record)
        if (await _penerimaRepository.ExistsByNamaAsync(dto.Nama, id))
        {
            throw new ValidationException($"Penerima dengan nama '{dto.Nama}' sudah ada");
        }

        _mapper.Map(dto, penerima);
        penerima.UpdatedAt = DateTime.UtcNow;

        _penerimaRepository.Update(penerima);
        await _penerimaRepository.SaveChangesAsync();

        return _mapper.Map<PenerimaDto>(penerima);
    }

    public async Task DeleteAsync(int id)
    {
        var penerima = await _penerimaRepository.GetByIdAsync(id);
        if (penerima == null)
        {
            throw new NotFoundException($"Penerima dengan ID {id} tidak ditemukan");
        }

        _penerimaRepository.Delete(penerima);
        await _penerimaRepository.SaveChangesAsync();
    }
}
