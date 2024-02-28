using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Application.Strategies.TaskUpdateStrategy;

public interface ITaskUpdateStrategy
{
    bool CanUpdate(ProjectTaskModel taskModel, UserContext userContext);
    Task<bool> UpdateAsync(ProjectTaskModel taskModel, UserContext userContext);
}
