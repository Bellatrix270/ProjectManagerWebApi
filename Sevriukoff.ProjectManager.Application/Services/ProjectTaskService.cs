using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Repositories.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Services;

public class ProjectTaskService : IProjectTaskService
{
    private readonly IProjectTaskRepository _projectTaskRepository;

    public ProjectTaskService(IProjectTaskRepository projectTaskRepository)
    {
        _projectTaskRepository = projectTaskRepository;
    }
    
    public async Task<IEnumerable<ProjectTaskModel>> GetAllAsync()
    {
        var projectTasks = await _projectTaskRepository.GetAllAsync();

        return projectTasks.Select(MapperWrapper.Map<ProjectTaskModel>);
    }

    public async Task<ProjectTaskModel> GetByIdAsync(int id)
    {
        var projectTask = await _projectTaskRepository.GetByIdAsync(id);

        return MapperWrapper.Map<ProjectTaskModel>(projectTask);
    }

    public async Task<int> AddAsync(ProjectTaskModel projectTaskModel)
    {
        var projectTask = MapperWrapper.Map<ProjectTask>(projectTaskModel);
        var id = await _projectTaskRepository.AddAsync(projectTask);

        return id;
    }

    public async Task<bool> UpdateAsync(ProjectTaskModel projectTaskModel)
    {
        var projectTask = MapperWrapper.Map<ProjectTask>(projectTaskModel);
        return await _projectTaskRepository.UpdateAsync(projectTask);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _projectTaskRepository.DeleteAsync(id);
    }
}