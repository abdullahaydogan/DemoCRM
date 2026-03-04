using DemoCRM.Application.useCases.Course.CreateCourse;
using DemoCRM.Application.useCases.Course.DeleteCourse;
using MediatR;

namespace WebApi.Graph.Mutations
{
    [MutationType]
    public class CourseMutations
    {
        public async Task<CreateCourseResponse> CreateCourseAsync( [Service] IMediator mediator, CreateCourseRequest request)
        {
            return await mediator.Send(request);    
        }
        public Task<DeleteCourseResponse> DeleteCourseAsync([Service] IMediator mediator, DeleteCourseRequest request)
        {
            return mediator.Send(request);
        }
    }
}
