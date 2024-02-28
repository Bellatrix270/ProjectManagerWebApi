using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.Application.Strategies.ProjectUpdateStrategy;

public interface IProjectUpdateStrategy
{
    bool CanUpdate(ProjectModel projectModel, UserContext userContext);
    Task<bool> UpdateAsync(ProjectModel projectModel, UserContext userContext);
}