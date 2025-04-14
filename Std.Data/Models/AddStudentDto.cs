namespace StudentCRUD.Models
{
    public class AddStudentDto
    {
        public required string Name { get; set; }
        public int Marks { get; set; }
        public string? Course { get; set; }
    }
}
