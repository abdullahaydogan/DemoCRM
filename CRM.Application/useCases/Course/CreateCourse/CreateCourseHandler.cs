using DemoCRM.Core.Const;
using DemoCRM.Persistance.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCRM.Application.useCases.Course.CreateCourse
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseRequest, CreateCourseResponse>
    {
        private readonly CrmContext _crmContext;

        public CreateCourseHandler(CrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        public async Task<CreateCourseResponse> Handle(CreateCourseRequest request, CancellationToken cancellationToken)
        {
            var existCourse = await _crmContext.Courses.AnyAsync(c => c.Name == request.Name && c.IsActive == true);
            if (existCourse == true)
            {
                throw new ApplicationException($"{ExCodes.CourseAlreadyExists} - {ExMessages.CourseAlreadyExists}");
            }

            var course = new Core.Entity.Course
            {
                Name = request.Name,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                Price = request.Price,
            };
            if ( request.TeacherIds != null && request.TeacherIds.Count() != 0)
            {
                var teachers = await _crmContext.Teachers.Where(t => request.TeacherIds.Contains(t.Id)).ToListAsync(cancellationToken);
                foreach (var teacher in teachers)
                {
                    course.Teachers.Add(teacher);
                }
            }
            await _crmContext.Courses.AddAsync(course, cancellationToken);
            await _crmContext.SaveChangesAsync(cancellationToken);

            var response = course.Adapt<CreateCourseResponse>();
            response.TeacherIds = course.Teachers.Select(t => t.Id).ToList();
            response.StudentIds = course.Students.Select(s => s.Id).ToList();
            return response;

        }
    }
}
