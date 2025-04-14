namespace StudentCRUD.Models.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public  required string Name { get; set; }
        public int Marks { get; set; }
        public string? Course { get; set; }

    }
}
