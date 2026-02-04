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

    public async Task<PagedResult<StpbDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, Guid userId, int tahun)
    {
        // Get user with role and PpkBendahara
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedException("User not found");

        var userWithRole = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);
        if (userWithRole == null)
            throw new UnauthorizedException("User role not found");

        var (items, totalCount) = await _unitOfWork.Stpbs.GetPagedAsync(pageNumber, pageSize, searchTerm);
        
        // Filter by tahun first
        items = items.Where(s => s.Tahun == tahun).ToList();
        
        // Filter by role
        if (!userWithRole.Role.IsAdmin)
        {
            // For PPK role: show all STPB (except Draft) that are directed to PPK jabatan
            if (userWithRole.Role.Name == "PPK")
            {
                var ppkBendaharaIds = await _unitOfWork.PpkBendaharas.GetAllAsync();
                var ppkIds = ppkBendaharaIds
                    .Where(p => p.Jabatan == JabatanType.PPK)
                    .Select(p => p.Id)
                    .ToList();
                
                items = items.Where(s => 
                    s.Status != StpbStatus.Draft && 
                    ppkIds.Contains(s.PpkBendaharaId)).ToList();
            }
            // For Bendahara role: show all STPB (except Draft) that are directed to Bendahara jabatan
            else if (userWithRole.Role.Name == "Bendahara")
            {
                var ppkBendaharaIds = await _unitOfWork.PpkBendaharas.GetAllAsync();
                var bendaharaIds = ppkBendaharaIds
                    .Where(p => p.Jabatan == JabatanType.Bendahara)
                    .Select(p => p.Id)
                    .ToList();
                
                items = items.Where(s => 
                    s.Status != StpbStatus.Draft && 
                    bendaharaIds.Contains(s.PpkBendaharaId)).ToList();
            }
            // For regular User: show only STPB created by user and filter by RBAC suboutput
            else
            {
                var allowedSuboutputs = userWithRole.Role.RoleSuboutputs.Select(rs => rs.KodeSuboutput).ToList();
                items = items.Where(s => 
                    s.CreatedBy == userId && 
                    s.StpbDetails.Any(d => allowedSuboutputs.Contains(d.KodeSuboutput))).ToList();
            }
            
            totalCount = items.Count();
        }
        else
        {
            totalCount = items.Count();
        }

        var dtos = _mapper.Map<IEnumerable<StpbDto>>(items);

        return new PagedResult<StpbDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<StpbDto?> GetByIdAsync(Guid id)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(id);
        
        if (stpb == null)
        {
            throw new NotFoundException($"STPB with ID {id} not found");
        }

        return _mapper.Map<StpbDto>(stpb);
    }

    public async Task<StpbDto> CreateAsync(CreateStpbDto dto, Guid userId)
    {
        // Get user with role to validate access
        var user = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);

        if (user == null)
            throw new UnauthorizedException("User not found");

        // Validate user has access to all details' suboutputs (if not admin)
        if (!user.Role.IsAdmin && dto.Details.Any())
        {
            var allowedSuboutputs = user.Role.RoleSuboutputs.Select(rs => rs.KodeSuboutput).ToList();
            var invalidDetails = dto.Details.Where(d => !allowedSuboutputs.Contains(d.KodeSuboutput)).ToList();
            
            if (invalidDetails.Any())
            {
                throw new UnauthorizedException("Anda tidak memiliki akses untuk membuat STPB dengan detail pada suboutput tertentu");
            }
        }

        // Auto-generate nomor STPB if not provided
        if (string.IsNullOrWhiteSpace(dto.NomorSTPB))
        {
            var year = dto.Tahun;
            var nextNumber = await _unitOfWork.SequenceNumbers.GetNextNumberAsync("STPB", year);
            dto.NomorSTPB = $"{nextNumber:D4}/DJPPR/{year}";
        }
        else
        {
            // Check if manually entered nomor already exists
            var existingStpb = await _unitOfWork.Stpbs.GetByNomorAsync(dto.NomorSTPB);
            if (existingStpb != null)
            {
                throw new ValidationException($"STPB dengan nomor {dto.NomorSTPB} sudah ada");
            }
        }

        var stpb = _mapper.Map<Stpb>(dto);
        stpb.CreatedBy = userId;
        stpb.Status = StpbStatus.Draft;

        // Map details
        stpb.StpbDetails = dto.Details.Select(d =>
        {
            var detail = _mapper.Map<StpbDetail>(d);
            detail.JumlahHarga = d.Volume * d.HargaSatuan;
            return detail;
        }).ToList();

        // Calculate total
        stpb.TotalNilai = stpb.StpbDetails.Sum(d => d.JumlahHarga);

        await _unitOfWork.Stpbs.AddAsync(stpb);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StpbDto>(stpb);
    }

    public async Task<StpbDto> UpdateAsync(Guid id, UpdateStpbDto dto)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(id);
        
        if (stpb == null)
        {
            throw new NotFoundException($"STPB with ID {id} not found");
        }

        // Check if status allows editing
        if (stpb.Status != StpbStatus.Draft && stpb.Status != StpbStatus.Dikembalikan)
        {
            throw new ValidationException("STPB hanya dapat diubah dalam status Draft atau Dikembalikan");
        }

        _mapper.Map(dto, stpb);
        stpb.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Stpbs.UpdateAsync(stpb);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StpbDto>(stpb);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(id);
        
        if (stpb == null)
        {
            throw new NotFoundException($"STPB with ID {id} not found");
        }

        // Get user with role
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedException("User not found");

        var userWithRole = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);
        if (userWithRole == null)
            throw new UnauthorizedException("User role not found");

        // Admin can delete anything
        if (!userWithRole.Role.IsAdmin)
        {
            // Non-admin can only delete their own STPB
            if (stpb.CreatedBy != userId)
                throw new UnauthorizedException("Anda hanya dapat menghapus STPB yang Anda buat");

            // Check if status allows deletion
            if (stpb.Status != StpbStatus.Draft && stpb.Status != StpbStatus.Dikembalikan)
            {
                throw new ValidationException("STPB hanya dapat dihapus dalam status Draft atau Dikembalikan");
            }
        }

        await _unitOfWork.Stpbs.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<StpbDto>> GetByUserIdAsync(Guid userId)
    {
        var stpbs = await _unitOfWork.Stpbs.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<StpbDto>>(stpbs);
    }

    // Workflow methods
    public async Task<StpbDto> KirimAsync(Guid id, Guid userId)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(id);
        
        if (stpb == null)
            throw new NotFoundException($"STPB with ID {id} not found");

        // Validate creator
        if (stpb.CreatedBy != userId)
            throw new UnauthorizedException("Hanya pembuat STPB yang dapat mengirim");

        // Validate status
        if (stpb.Status != StpbStatus.Draft && stpb.Status != StpbStatus.Dikembalikan)
            throw new ValidationException("STPB hanya dapat dikirim dari status Draft atau Dikembalikan");

        // Validate has details
        if (!stpb.StpbDetails.Any())
            throw new ValidationException("STPB harus memiliki minimal 1 detail transaksi");

        stpb.Status = StpbStatus.Kirim;
        stpb.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Stpbs.UpdateAsync(stpb);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StpbDto>(stpb);
    }

    public async Task<StpbDto> ApproveAsync(Guid id, Guid userId)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(id);
        
        if (stpb == null)
            throw new NotFoundException($"STPB with ID {id} not found");

        // Validate status
        if (stpb.Status != StpbStatus.Kirim)
            throw new ValidationException("STPB hanya dapat di-approve dari status Kirim");

        // Validate user authority
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedException("User not found");

        var userWithRole = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);
        if (userWithRole == null)
            throw new UnauthorizedException("User role not found");

        // Only Admin, PPK, or Bendahara can approve
        if (!userWithRole.Role.IsAdmin && 
            userWithRole.Role.Name != "PPK" && 
            userWithRole.Role.Name != "Bendahara")
        {
            throw new UnauthorizedException("Hanya Admin, PPK, atau Bendahara yang dapat meng-approve STPB");
        }

        // PPK/Bendahara can only approve STPB directed to them
        if (userWithRole.Role.Name == "PPK" || userWithRole.Role.Name == "Bendahara")
        {
            if (user.PpkBendaharaId == null)
                throw new UnauthorizedException("User tidak terhubung dengan PPK/Bendahara");

            if (stpb.PpkBendaharaId != user.PpkBendaharaId)
                throw new UnauthorizedException("Anda hanya dapat meng-approve STPB yang ditujukan kepada Anda");
        }

        stpb.Status = StpbStatus.Approve;
        stpb.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Stpbs.UpdateAsync(stpb);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StpbDto>(stpb);
    }

    public async Task<StpbDto> KembalikanAsync(Guid id, Guid userId, string alasan)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(id);
        
        if (stpb == null)
            throw new NotFoundException($"STPB with ID {id} not found");

        // Validate status
        if (stpb.Status != StpbStatus.Kirim)
            throw new ValidationException("STPB hanya dapat dikembalikan dari status Kirim");

        // Validate user authority
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedException("User not found");

        var userWithRole = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);
        if (userWithRole == null)
            throw new UnauthorizedException("User role not found");

        // Only Admin, PPK, or Bendahara can return
        if (!userWithRole.Role.IsAdmin && 
            userWithRole.Role.Name != "PPK" && 
            userWithRole.Role.Name != "Bendahara")
        {
            throw new UnauthorizedException("Hanya Admin, PPK, atau Bendahara yang dapat mengembalikan STPB");
        }

        // PPK/Bendahara can only return STPB directed to them
        if (userWithRole.Role.Name == "PPK" || userWithRole.Role.Name == "Bendahara")
        {
            if (user.PpkBendaharaId == null)
                throw new UnauthorizedException("User tidak terhubung dengan PPK/Bendahara");

            if (stpb.PpkBendaharaId != user.PpkBendaharaId)
                throw new UnauthorizedException("Anda hanya dapat mengembalikan STPB yang ditujukan kepada Anda");
        }

        stpb.Status = StpbStatus.Dikembalikan;
        stpb.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Stpbs.UpdateAsync(stpb);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StpbDto>(stpb);
    }

    // Detail management methods
    public async Task<StpbDetailDto> AddDetailAsync(Guid stpbId, CreateStpbDetailDto dto, Guid userId)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(stpbId);
        
        if (stpb == null)
            throw new NotFoundException($"STPB with ID {stpbId} not found");

        // Validate creator
        if (stpb.CreatedBy != userId)
            throw new UnauthorizedException("Hanya pembuat STPB yang dapat menambah detail");

        // Validate status
        if (stpb.Status != StpbStatus.Draft && stpb.Status != StpbStatus.Dikembalikan)
            throw new ValidationException("Detail hanya dapat ditambahkan pada status Draft atau Dikembalikan");

        // Validate RBAC
        var user = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);
        if (user != null && !user.Role.IsAdmin)
        {
            var allowedSuboutputs = user.Role.RoleSuboutputs.Select(rs => rs.KodeSuboutput).ToList();
            if (!allowedSuboutputs.Contains(dto.KodeSuboutput))
                throw new UnauthorizedException("Anda tidak memiliki akses untuk suboutput ini");
        }

        // Validate Pagu
        var anggaran = await _unitOfWork.AnggaranMasters.GetAnggaranByKeysAsync(
            stpb.Tahun,
            0, // Default revisi to 0 if not tracked in STPB
            dto.KodeProgram,
            dto.KodeKegiatan,
            dto.KodeOutput,
            dto.KodeSuboutput,
            dto.KodeKomponen,
            dto.KodeSubkomponen,
            dto.KodeAkun,
            dto.NoItem!
        );

        if (anggaran == null)
            throw new ValidationException("Data anggaran tidak ditemukan");

        var realisasi = await _unitOfWork.StpbDetails.GetRealisasiByItemAsync(
            stpb.Tahun,
            0, // Default revisi to 0 if not tracked in STPB
            dto.KodeProgram,
            dto.KodeKegiatan,
            dto.KodeOutput,
            dto.KodeSuboutput,
            dto.KodeKomponen,
            dto.KodeSubkomponen,
            dto.KodeAkun,
            dto.NoItem!
        );

        var sisaPagu = anggaran.Pagu - realisasi;
        var nilaiInput = dto.Volume * dto.HargaSatuan;

        if (nilaiInput > sisaPagu)
            throw new ValidationException($"Nilai input (Rp {nilaiInput:N0}) melebihi sisa pagu (Rp {sisaPagu:N0})");

        var detail = _mapper.Map<StpbDetail>(dto);
        detail.StpbId = stpbId;
        detail.Tahun = stpb.Tahun;
        detail.Revisi = anggaran.Revisi;
        detail.JumlahHarga = dto.Volume * dto.HargaSatuan;
        detail.NilaiBersih = detail.JumlahHarga - dto.PPN - (dto.PPH21 + dto.PPH22 + dto.PPH23);

        await _unitOfWork.StpbDetails.AddAsync(detail);

        // Recalculate total (use NilaiBersih for total)
        stpb.TotalNilai = stpb.StpbDetails.Sum(d => d.NilaiBersih) + detail.NilaiBersih;
        stpb.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Stpbs.UpdateAsync(stpb);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StpbDetailDto>(detail);
    }

    public async Task<StpbDetailDto> UpdateDetailAsync(Guid stpbId, Guid detailId, CreateStpbDetailDto dto, Guid userId)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(stpbId);
        
        if (stpb == null)
            throw new NotFoundException($"STPB with ID {stpbId} not found");

        // Validate creator
        if (stpb.CreatedBy != userId)
            throw new UnauthorizedException("Hanya pembuat STPB yang dapat mengubah detail");

        // Validate status
        if (stpb.Status != StpbStatus.Draft && stpb.Status != StpbStatus.Dikembalikan)
            throw new ValidationException("Detail hanya dapat diubah pada status Draft atau Dikembalikan");

        // Validate RBAC
        var user = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);
        if (user != null && !user.Role.IsAdmin)
        {
            var allowedSuboutputs = user.Role.RoleSuboutputs.Select(rs => rs.KodeSuboutput).ToList();
            if (!allowedSuboutputs.Contains(dto.KodeSuboutput))
                throw new UnauthorizedException("Anda tidak memiliki akses untuk suboutput ini");
        }

        var detail = await _unitOfWork.StpbDetails.GetByIdAsync(detailId);
        if (detail == null || detail.StpbId != stpbId)
            throw new NotFoundException($"Detail with ID {detailId} not found in this STPB");

        _mapper.Map(dto, detail);
        detail.Tahun = stpb.Tahun;
        // Keep existing Revisi or update if needed
        detail.JumlahHarga = dto.Volume * dto.HargaSatuan;
        detail.NilaiBersih = detail.JumlahHarga - dto.PPN - (dto.PPH21 + dto.PPH22 + dto.PPH23);
        detail.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.StpbDetails.UpdateAsync(detail);

        // Recalculate total (use NilaiBersih)
        var allDetails = await _unitOfWork.StpbDetails.GetByStpbIdAsync(stpbId);
        stpb.TotalNilai = allDetails.Sum(d => d.NilaiBersih);
        stpb.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Stpbs.UpdateAsync(stpb);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StpbDetailDto>(detail);
    }

    public async Task<bool> DeleteDetailAsync(Guid stpbId, Guid detailId, Guid userId)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(stpbId);
        
        if (stpb == null)
            throw new NotFoundException($"STPB with ID {stpbId} not found");

        // Validate creator
        if (stpb.CreatedBy != userId)
            throw new UnauthorizedException("Hanya pembuat STPB yang dapat menghapus detail");

        // Validate status
        if (stpb.Status != StpbStatus.Draft && stpb.Status != StpbStatus.Dikembalikan)
            throw new ValidationException("Detail hanya dapat dihapus pada status Draft atau Dikembalikan");

        var detail = await _unitOfWork.StpbDetails.GetByIdAsync(detailId);
        if (detail == null || detail.StpbId != stpbId)
            throw new NotFoundException($"Detail with ID {detailId} not found in this STPB");

        await _unitOfWork.StpbDetails.DeleteAsync(detailId);

        // Recalculate total
        var allDetails = await _unitOfWork.StpbDetails.GetByStpbIdAsync(stpbId);
        stpb.TotalNilai = allDetails.Where(d => d.Id != detailId).Sum(d => d.JumlahHarga);
        stpb.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Stpbs.UpdateAsync(stpb);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

