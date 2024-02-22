using System.Linq.Expressions;
using Sevriukoff.ProjectManager.Infrastructure.Base;

namespace Sevriukoff.ProjectManager.Application.Specification.Project;

public class PrioritySpecification : Specification<Infrastructure.Entities.Project>
{
    private readonly int? _priority;

    public PrioritySpecification(int? priority)
    {
        _priority = priority;
    }

    public override Expression<Func<Infrastructure.Entities.Project, bool>> ToExpression()
    {
        return p => !_priority.HasValue || p.Priority == _priority;
    }
}