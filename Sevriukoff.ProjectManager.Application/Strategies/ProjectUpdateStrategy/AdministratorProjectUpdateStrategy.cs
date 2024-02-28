using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Strategies.ProjectUpdateStrategy;

public class AdministratorProjectUpdateStrategy : IProjectUpdateStrategy
{
    private readonly IProjectRepository _projectRepository;

    public AdministratorProjectUpdateStrategy(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    public bool CanUpdate(ProjectModel projectModel, UserContext userContext)
    {
        return userContext.Role == UserRole.Administrator;
    }

    public async Task<bool> UpdateAsync(ProjectModel projectModel, UserContext userContext)
    {
        var project = MapperWrapper.Map<Project>(projectModel);

        return await _projectRepository.UpdateAsync(project);
    }
}