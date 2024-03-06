using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Strategies.TaskUpdateStrategy;

public class ManagerTaskUpdateStrategy : ITaskUpdateStrategy
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IProjectRepository _projectRepository;
    private ProjectTask? _cachedTask;

    public ManagerTaskUpdateStrategy(IProjectTaskRepository projectTaskRepository, IProjectRepository projectRepository)
    {
        _projectTaskRepository = projectTaskRepository;
        _projectRepository = projectRepository;
    }

    public bool CanUpdate(ProjectTaskModel taskModel, UserContext userContext)
    {
        var task = GetTask(taskModel.Id);
        var project =  _projectRepository.GetByIdAsync(task.ProjectId).GetAwaiter().GetResult();

        if (project.ManagerId != userContext.UserId)
            return false;
        
        return userContext.Role == UserRole.Manager;
    }

    public async Task<bool> UpdateAsync(ProjectTaskModel taskModel, UserContext userContext)
    {
        var task = GetTask(taskModel.Id);
        
        task.Status = taskModel.Status;
        task.AssignedToId = taskModel.AssignedToId;

        return await _projectTaskRepository.UpdateAsync(task);
    }
    
    private ProjectTask GetTask(Guid taskId)
    {
        if (_cachedTask != null && _cachedTask.Id == taskId)
            return _cachedTask;
        
        return _projectTaskRepository.GetByIdAsync(taskId).GetAwaiter().GetResult();
    }
}