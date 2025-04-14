namespace StudentCRUD.Models
{
    public class UpdateStudentDto
    {
        public required string Name { get; set; }
        public int Marks { get; set; }
        public string? Course { get; set; }
    }
}
