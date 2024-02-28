namespace Sevriukoff.ProjectManager.Application.Specification.ProjectTask;

public class TaskByCreatorAndExecutorSpecification : Specification<Infrastructure.Entities.ProjectTask>
{
    public TaskByCreatorAndExecutorSpecification(Guid? createdById, Guid? assignedById)
    {
        if (createdById.HasValue)
            SetFilterCondition(p => p.CreatedById == createdById);
        else if (assignedById.HasValue) 
            SetFilterCondition(p => p.AssignedToId == assignedById);
        else if (createdById.HasValue && assignedById.HasValue)
            SetFilterCondition(p => p.CreatedById == createdById && p.AssignedToId == assignedById);
    }
}