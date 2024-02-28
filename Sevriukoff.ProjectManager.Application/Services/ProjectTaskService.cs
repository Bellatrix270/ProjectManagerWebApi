using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Factories;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Application.Specification;
using Sevriukoff.ProjectManager.Application.Specification.ProjectTask;
using Sevriukoff.ProjectManager.Application.Strategies.TaskUpdateStrategy;
using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Services;

public class ProjectTaskService : IProjectTaskService
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly TaskUpdateStrategyFactory _strategyFactory;

    public ProjectTaskService(IProjectTaskRepository projectTaskRepository,
        IEmployeeRepository employeeRepository,
        TaskUpdateStrategyFactory  strategyFactory)
    {
        _projectTaskRepository = projectTaskRepository;
        _employeeRepository = employeeRepository;
        _strategyFactory = strategyFactory;
    }
    
    public async Task<IEnumerable<ProjectTaskModel>> GetAllAsync()
    {
        var projectTasks = await _projectTaskRepository.GetAllAsync();

        return projectTasks.Select(MapperWrapper.Map<ProjectTaskModel>);
    }

    public async Task<ProjectTaskModel?> GetByIdAsync(int id)
    {
        var projectTask = await _projectTaskRepository.GetByIdAsync(id);

        return MapperWrapper.Map<ProjectTask, ProjectTaskModel>(projectTask);
    }

    public async Task<int> AddAsync(ProjectTaskModel projectTaskModel)
    {
        var projectTask = MapperWrapper.Map<ProjectTask>(projectTaskModel);
        var id = await _projectTaskRepository.AddAsync(projectTask);

        return id;
    }
    
    public async Task<bool> UpdateAsync(ProjectTaskModel projectTaskModel, UserContext userContext)
    {
        var strategy = _strategyFactory.CreateStrategy(userContext);

        if (!strategy.CanUpdate(projectTaskModel, userContext))
            throw new AccessDeniedException("У вас нет прав на изменение задачи.");

        return await strategy.UpdateAsync(projectTaskModel, userContext);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _projectTaskRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProjectTaskModel>> GetFilteredAndSortedAsync(ProjectTaskStatus? status, int? priority, 
        Guid? createdById, Guid? assignedToId, string? sortBy, UserContext userContext, params string[] includes)
    {
        if (userContext.Role != UserRole.Administrator)
            createdById = userContext.UserId;
        
        var prioritySpec = new TaskPrioritySpecification(priority);
        var statusSpec = new TaskStatusSpecification(status);
        var creatorAndExecutorSpec = new TaskByCreatorAndExecutorSpecification(createdById, assignedToId);
        var sortingSpec = new SortingSpecification<ProjectTask>(sortBy);

        var combinedSpec = prioritySpec.And(statusSpec)
                                                            .And(sortingSpec)
                                                            .And(creatorAndExecutorSpec);

        var filteredTasks = (await _projectTaskRepository.GetBySpecificationAsync(combinedSpec)).ToList();
        var mappedTask = filteredTasks.Select(MapperWrapper.Map<ProjectTaskModel>).ToList();

        foreach (var task in mappedTask)
        {
            if (includes.Contains(nameof(ProjectTaskModel.CreatedBy)))
            {
                task.CreatedBy = MapperWrapper.Map<EmployeeModel>(await _employeeRepository.GetByIdAsync(task.CreatedById));
            }

            if (includes.Contains(nameof(ProjectTaskModel.AssignedTo)))
            {
                task.AssignedTo = MapperWrapper.Map<EmployeeModel>(await _employeeRepository.GetByIdAsync(task.AssignedToId.Value));
            }
        }

        return mappedTask;
    }

    private List<ProjectTaskModel> IncludeEmployee()
    {
        throw new NotImplementedException();
    }
}