using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Application.Specification.ProjectTask;

public class TaskStatusSpecification : Specification<Infrastructure.Entities.ProjectTask>
{
    public TaskStatusSpecification(ProjectTaskStatus? status)
    {
        if (status.HasValue)
            SetFilterCondition(t => t.Status == status);
    }
}