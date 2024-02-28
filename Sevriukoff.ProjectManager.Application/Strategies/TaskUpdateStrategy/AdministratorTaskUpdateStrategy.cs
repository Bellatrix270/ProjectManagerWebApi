using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Strategies.TaskUpdateStrategy;

public class AdministratorTaskUpdateStrategy : ITaskUpdateStrategy
{
    private readonly IProjectTaskRepository _projectTaskRepository;

    public AdministratorTaskUpdateStrategy(IProjectTaskRepository projectTaskRepository)
    {
        _projectTaskRepository = projectTaskRepository;
    }

    public bool CanUpdate(ProjectTaskModel taskModel, UserContext userContext)
    {
        return userContext.Role == UserRole.Administrator;
    }

    public async Task<bool> UpdateAsync(ProjectTaskModel taskModel, UserContext userContext)
    {
        return await _projectTaskRepository.UpdateAsync(MapperWrapper.Map<ProjectTask>(taskModel));
    }
}