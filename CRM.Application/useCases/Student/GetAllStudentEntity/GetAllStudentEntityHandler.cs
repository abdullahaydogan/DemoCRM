using DemoCRM.Persistance.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCRM.Application.useCases.Student.GetAllStudentEntity
{
    public class GetAllStudentEntityHandler : IRequestHandler<GetAllStudentEntityRequest, IQueryable<DemoCRM.Core.Entity.Student>>
    {
        private readonly CrmContext _crmContext;

        public GetAllStudentEntityHandler(CrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        public Task<IQueryable<Core.Entity.Student>> Handle(GetAllStudentEntityRequest request, CancellationToken cancellationToken)
        {
            var query = _crmContext.Students.Where(s => s.IsActive == true).Include(s => s.Courses).AsNoTracking();
            return Task.FromResult(query);
        }
    }
}
