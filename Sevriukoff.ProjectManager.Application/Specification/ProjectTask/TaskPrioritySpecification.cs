namespace Sevriukoff.ProjectManager.Application.Specification.ProjectTask;

public class TaskPrioritySpecification : Specification<Infrastructure.Entities.ProjectTask>
{
    public TaskPrioritySpecification(int? priority)
    {
        if (priority.HasValue)
            SetFilterCondition(p => p.Priority == priority);
    }
}