using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.Application.Strategies.ProjectUpdateStrategy;

public class EmployeeProjectUpdateStrategy : IProjectUpdateStrategy
{
    public bool CanUpdate(ProjectModel projectModel, UserContext userContext)
    {
        return false;
    }

    public async Task<bool> UpdateAsync(ProjectModel projectModel, UserContext userContext)
    {
        return false;
    }
}