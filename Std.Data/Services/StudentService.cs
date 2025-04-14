using StudentCRUD.Models;
using Microsoft.EntityFrameworkCore;
using StudentCRUD.Models.Entities;

namespace Std.Data.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> AddAsync(AddStudentDto dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Course = dto.Course,
                Marks = dto.Marks
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student> UpdateAsync(Guid id, UpdateStudentDto dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;

            student.Name = dto.Name;
            student.Course = dto.Course;
            student.Marks = dto.Marks;

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Student> PatchAsync(Guid id,UpdateStudentDto dto)
        {
            var student = await _context.Students.FindAsync(id);

            if (student is null) return null;

            if (!string.IsNullOrEmpty(dto.Name))
            {
                student.Name = dto.Name;
            }
            if(!string.IsNullOrEmpty(dto.Course))
            {
                student.Course = dto.Course;
            }
            if (dto.Marks > 0)
            {
                student.Marks = dto.Marks;
            }

            await _context.SaveChangesAsync();
            return student;
        }

        public Task UpdateAsync(UpdateStudentDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
