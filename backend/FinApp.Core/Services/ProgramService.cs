using AutoMapper;
using FinApp.Core.DTOs.Program;
using FinApp.Core.Exceptions;
using FinApp.Core.Interfaces;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProgramService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProgramDto>> GetAllAsync()
        {
            var programs = await _unitOfWork.Programs.GetAllAsync();
            return _mapper.Map<IEnumerable<ProgramDto>>(programs);
        }

        public async Task<ProgramDto> GetByIdAsync(Guid id)
        {
            var program = await _unitOfWork.Programs.GetByIdAsync(id);
            if (program == null)
                throw new NotFoundException("Program tidak ditemukan");

            return _mapper.Map<ProgramDto>(program);
        }

        public async Task<ProgramDto> CreateAsync(CreateProgramDto dto)
        {
            var program = _mapper.Map<Domain.Entities.Program>(dto);
            program.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Programs.AddAsync(program);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProgramDto>(program);
        }

        public async Task<ProgramDto> UpdateAsync(Guid id, UpdateProgramDto dto)
        {
            var program = await _unitOfWork.Programs.GetByIdAsync(id);
            if (program == null)
                throw new NotFoundException("Program tidak ditemukan");

            _mapper.Map(dto, program);
            program.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Programs.UpdateAsync(program);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProgramDto>(program);
        }

        public async Task DeleteAsync(Guid id)
        {
            var program = await _unitOfWork.Programs.GetByIdAsync(id);
            if (program == null)
                throw new NotFoundException("Program tidak ditemukan");

            await _unitOfWork.Programs.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
