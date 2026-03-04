using DemoCRM.Core.Const;
using DemoCRM.Persistance.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCRM.Application.useCases.Student.UpdateStudent
{
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentRequest, UpdateStudentResponse>
    {
        private readonly CrmContext _crmContext;

        public UpdateStudentHandler(CrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        public async Task<UpdateStudentResponse> Handle(UpdateStudentRequest request, CancellationToken cancellationToken)
        {
            var entity = await _crmContext.Students.Include(s => s.Courses).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                         ?? throw new ApplicationException($"{ExCodes.StudentNotFound} - {ExMessages.StudentNotFound}");

            if (request.Name is not null)
                entity.Name = request.Name;

            if (request.Surname is not null)
                entity.Surname = request.Surname;

            if (request.PhoneNumber is not null)
                entity.PhoneNumber = request.PhoneNumber;

            if (request.Email is not null)
                entity.Email = request.Email;

            if (request.Courses != null)
            {
                var courseIds = await _crmContext.Courses.Where(c => request.Courses.Select(rc => rc.Id).Contains(c.Id)).ToListAsync(cancellationToken);
                foreach (var course in courseIds)
                {
                    entity.Courses.Add(course);
                }

            }


            entity.UpdatedDate = DateTime.UtcNow;
            await _crmContext.SaveChangesAsync(cancellationToken);

            var response = entity.Adapt<UpdateStudentResponse>();
            response.CorseIds = entity.Courses.Select(c => c.Id).ToList();
            return response;


        }
    }
}
