namespace DemoCRM.Application.useCases.Student.UpdateStudent
{
    public class UpdateStudentResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public ICollection<int>? CorseIds { get; set; } = new List<int>();
    }
}
