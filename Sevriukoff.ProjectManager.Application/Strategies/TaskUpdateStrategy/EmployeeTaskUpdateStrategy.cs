using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Strategies.TaskUpdateStrategy;

public class EmployeeTaskUpdateStrategy : ITaskUpdateStrategy
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private ProjectTask? _cachedTask;

    public EmployeeTaskUpdateStrategy(IProjectTaskRepository projectTaskRepository)
    {
        _projectTaskRepository = projectTaskRepository;
    }

    public bool CanUpdate(ProjectTaskModel taskModel, UserContext userContext)
    {
        var task = GetTaskAsync(taskModel.Id).GetAwaiter().GetResult();

        if (task.AssignedToId != userContext.UserId)
            return false;
        
        return userContext.Role == UserRole.Employee;
    }

    public async Task<bool> UpdateAsync(ProjectTaskModel taskModel, UserContext userContext)
    {
        var projectTask = await GetTaskAsync(taskModel.Id);
        
        projectTask.Status = taskModel.Status;

        return await _projectTaskRepository.UpdateAsync(projectTask);
    }
    
    private async Task<ProjectTask> GetTaskAsync(Guid taskId)
    {
        if (_cachedTask != null && _cachedTask.Id == taskId)
            return _cachedTask;

        _cachedTask = await _projectTaskRepository.GetByIdAsync(taskId);
        
        return _cachedTask;
    }
}