using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Strategies.ProjectUpdateStrategy;

public class ManagerProjectUpdateStrategy : IProjectUpdateStrategy
{
    private readonly IProjectRepository _projectRepository;
    private Project? _cachedProject;

    public ManagerProjectUpdateStrategy(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    public bool CanUpdate(ProjectModel projectModel, UserContext userContext)
    {
        return false;
        
        var project = GetProjectAsync(projectModel.Id).GetAwaiter().GetResult();
        
        if (project.ManagerId != userContext.UserId)
            return false;

        return userContext.Role == UserRole.Manager;
    }

    public async Task<bool> UpdateAsync(ProjectModel projectModel, UserContext userContext)
    {
        return false;
        
        var project = await GetProjectAsync(projectModel.Id);
        
        return await _projectRepository.UpdateAsync(project);
    }

    private async Task<Project> GetProjectAsync(Guid id)
    {
        if (_cachedProject != null && _cachedProject.Id == id)
            return _cachedProject;

        _cachedProject = await _projectRepository.GetByIdAsync(id);

        return _cachedProject;
    }
}