
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Std.Data.Services;
using StudentCRUD.Models;

namespace StudentCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    //[Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _studentService.GetAllAsync());

        [HttpPost("json")]
        [Consumes("application/json", "application/xml")]
        public async Task<IActionResult> AddJson([FromBody] AddStudentDto dto)
        {
            var student = await _studentService.AddAsync(dto);
            return Ok(student);
        }


        [HttpPost("form")]
        [Consumes("multipart/form-data", "application/x-www-form-urlencoded")]
        public async Task<IActionResult> AddForm([FromForm] AddStudentDto dto) => Ok(await _studentService.AddAsync(dto));


        [HttpPut("form/{id:guid}")]
        [Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        public async Task<IActionResult> UpdateForm(Guid id, [FromForm] UpdateStudentDto dto)
        {
            var updated = await _studentService.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpPut("json/{id:guid}")]
        [Consumes("application/json", "application/xml")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentDto dto)
        {
            var updated = await _studentService.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }


        [HttpPatch("form/{id:guid}")]
        [Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        public async Task<IActionResult> PatchForm(Guid id, [FromForm] UpdateStudentDto dto)
        {
            var patched = await _studentService.PatchAsync(id, dto);
            return patched == null ? NotFound() : Ok(patched);
        }

        [HttpPatch("json/{id:guid}")]
        [Consumes("application/json", "application/xml")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] UpdateStudentDto dto)
        {
            var patched = await _studentService.PatchAsync(id, dto);
            return patched == null ? NotFound() : Ok(patched);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _studentService.DeleteAsync(id);
            return deleted ? Ok(deleted) : NotFound();
        }
    }
}
