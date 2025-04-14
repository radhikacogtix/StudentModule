using StudentCRUD.Models;
using StudentCRUD.Models.Entities;

namespace Std.Data.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> AddAsync(AddStudentDto dto);
        Task<Student> UpdateAsync(Guid id, UpdateStudentDto dto);
        Task<bool> DeleteAsync(Guid id);

        //Task<Student> PatchAsync(Guid id);
        Task<Student> PatchAsync(Guid id, UpdateStudentDto dto);
        Task UpdateAsync(UpdateStudentDto dto);
    }
}
