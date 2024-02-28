namespace Sevriukoff.ProjectManager.Application.Specification.Project;

public class ProjectByManagerIdSpecification : Specification<Infrastructure.Entities.Project>
{
    public ProjectByManagerIdSpecification(Guid? managerId)
    {
        if (managerId.HasValue)
            SetFilterCondition(p => p.ManagerId == managerId);
    }
}