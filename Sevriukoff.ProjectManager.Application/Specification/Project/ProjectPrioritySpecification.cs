namespace Sevriukoff.ProjectManager.Application.Specification.Project;

public class ProjectPrioritySpecification : Specification<Infrastructure.Entities.Project>
{
    public ProjectPrioritySpecification(int? priority)
    {
        if (priority.HasValue)
            SetFilterCondition(p => p.Priority == priority);
    }
}