using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Std.Data.Services;
using StudentCRUD.Models;
using StudentCRUD.Models.Entities;

namespace StudentCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentDto dto)
        {
            var student = await _studentService.AddAsync(dto);
            return Ok(student);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateStudentDto dto)
        {
            var updated = await _studentService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _studentService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok(deleted);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(Guid id, UpdateStudentDto dto)
        {   
            if (dto is null) return NotFound();

            var student = await _studentService.PatchAsync(id, dto);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // Format - Specific  Post Methods

        [HttpPost("Json")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddJson([FromBody] AddStudentDto dto)
        {
            var student = await _studentService.AddAsync(dto);
            return Ok(student);
        }

        [HttpPost("xml")]
        [Consumes("application/xml")]

        public async Task<IActionResult> AddXml([FromBody] AddStudentDto dto)
        {
            var student = await _studentService.AddAsync(dto);
            return Ok(student);
        }

        [HttpPost("form")]
        [Consumes("application/x-www-form-urlencoded")]

        public async Task<IActionResult> AddForm([FromForm] AddStudentDto dto)
        {
            var student = await _studentService.AddAsync(dto);
            return Ok(student);
        }

        [HttpPost("multipart")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> AddMultipart([FromForm] AddStudentDto dto)
        {
            var student = await _studentService.AddAsync(dto);
            return Ok(student);
        }

        // Format Specific Put Methods

        [HttpPut("json/id:Guid")]
        [Consumes("application/json")]
        public async Task<IActionResult> updateJson(Guid id ,[FromBody] UpdateStudentDto dto)
        {
            var student = await _studentService.UpdateAsync(id, dto);
            if (student is null) return NotFound();
            return Ok(student);
        }

        [HttpPut("xml/id:Guid")]
        [Consumes("application/xml")]
        public async Task<IActionResult> updateXml(Guid id, [FromBody] UpdateStudentDto dto)
        {
            var student = await _studentService.UpdateAsync(id, dto);
            if (student is null) return NotFound();
            return Ok(student);
        }
        [HttpPut("form/id")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> updateForm(Guid id, [FromForm] UpdateStudentDto dto)
        {
            var student = await _studentService.UpdateAsync(id, dto);
            if (student is null) return NotFound();
            return Ok(student);
        }

        [HttpPut("multipart/id")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> updateMultipart(Guid id, [FromForm] UpdateStudentDto dto)
        {
            var student = await _studentService.UpdateAsync(id, dto);
            if (student is null) return NotFound();
            return Ok(student);
        }

        // Format Specific Patch Method
        [HttpPatch("json/id")]
        [Consumes("application/json")]
        public async Task<IActionResult> PatchJson(Guid id, [FromBody] UpdateStudentDto dto)
        {
            if (dto is null) return NotFound();

            var student = await _studentService.PatchAsync(id, dto);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPatch("xml/id")]
        [Consumes("application/xml")]
        public async Task<IActionResult> PatchXml(Guid id,[FromBody] UpdateStudentDto dto)
        {
            if (dto is null) return NotFound();

            var student = await _studentService.PatchAsync(id, dto);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPatch("form/id")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PatchForm(Guid id,[FromForm] UpdateStudentDto dto)
        {
            if (dto is null) return NotFound();

            var student = await _studentService.PatchAsync(id, dto);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPatch("multipart/id")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PatchMultipart(Guid id,[FromForm] UpdateStudentDto dto)
        {
            if (dto is null) return NotFound();

            var student = await _studentService.PatchAsync(id, dto);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }
    }
}
