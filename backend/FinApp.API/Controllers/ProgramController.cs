using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinApp.Core.Interfaces;
using FinApp.Core.DTOs.Program;

namespace FinApp.API.Controllers
{
    [Authorize]
    public class ProgramController : BaseApiController
    {
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var programs = await _programService.GetAllAsync();
            var programList = programs.ToList();
            return Ok(new { 
                success = true, 
                data = new { 
                    items = programList,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    totalCount = programList.Count
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var program = await _programService.GetByIdAsync(id);
            return Ok(new { success = true, data = program });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProgramDto dto)
        {
            var program = await _programService.CreateAsync(dto);
            return Ok(new { success = true, data = program });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProgramDto dto)
        {
            var program = await _programService.UpdateAsync(id, dto);
            return Ok(new { success = true, data = program });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _programService.DeleteAsync(id);
            return Ok(new { success = true, message = "Program berhasil dihapus" });
        }
    }
}
