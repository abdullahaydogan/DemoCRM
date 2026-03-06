namespace CRM.Application.useCases.Student.CreateStudent
{
    public class CreateStudentResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public List<int>? CoursesIds { get; set; }
    }
}
