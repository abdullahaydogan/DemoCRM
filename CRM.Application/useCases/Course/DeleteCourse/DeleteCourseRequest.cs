using MediatR;

namespace DemoCRM.Application.useCases.Course.DeleteCourse
{
    public class DeleteCourseRequest : IRequest<DeleteCourseResponse>
    {
        public int Id { get; set; }
    }
}
