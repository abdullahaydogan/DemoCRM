using DemoCRM.Core.Const;
using DemoCRM.Persistance.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCRM.Application.useCases.Course.DeleteCourse
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseRequest, DeleteCourseResponse>
    {
        private readonly CrmContext _crmContext;

        public DeleteCourseHandler(CrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        public async Task<DeleteCourseResponse> Handle(DeleteCourseRequest request, CancellationToken cancellationToken)
        {
            var existCourse = await _crmContext.Courses.FirstOrDefaultAsync(c => c.Id == request.Id);

            if ( existCourse == null)
            {
                throw new ApplicationException($"{ExCodes.CourseNotFound} - {ExMessages.CourseNotFound}");
            }

            _crmContext.Remove(existCourse);
            await _crmContext.SaveChangesAsync(cancellationToken);
            var response = new DeleteCourseResponse
            {
                Name = existCourse.Name,
            };
            return response;
        }
    }
}
