namespace Sevriukoff.ProjectManager.Application.Specification.Project;

public class PrioritySpecification : Specification<Infrastructure.Entities.Project>
{
    public PrioritySpecification(int? priority)
    {
        if (priority.HasValue)
            SetFilterCondition(p => p.Priority == priority);
    }
}