namespace DemoCRM.Core.Entity
{
    public class Course
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Student>? Students { get; set; } = new List<Student>();
        public ICollection<Teacher>? Teachers { get; set; } = new List<Teacher>();
    }
}
