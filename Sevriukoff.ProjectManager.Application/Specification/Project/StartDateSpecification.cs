using System.Linq.Expressions;
using Sevriukoff.ProjectManager.Infrastructure.Base;

namespace Sevriukoff.ProjectManager.Application.Specification.Project;

public class StartDateSpecification : Specification<Infrastructure.Entities.Project>
{
    private readonly DateTime _startDateFrom;
    private readonly DateTime _startDateTo;

    public StartDateSpecification(DateTime startDateFrom, DateTime startDateTo)
    {
        _startDateFrom = startDateFrom;
        _startDateTo = startDateTo;
    }

    public override Expression<Func<Infrastructure.Entities.Project, bool>> ToExpression()
    {
        return p => p.StartDate >= _startDateFrom && p.StartDate <= _startDateTo;
    }
}