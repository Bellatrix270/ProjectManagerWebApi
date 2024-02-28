using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Application.Specification;
using Sevriukoff.ProjectManager.Application.Specification.Project;
using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Repositories.Interfaces;
using PrioritySpecification = Sevriukoff.ProjectManager.Application.Specification.Project.PrioritySpecification;

namespace Sevriukoff.ProjectManager.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IProjectTaskRepository _taskRepository;

    public ProjectService(IProjectRepository projectRepository,
        IEmployeeRepository employeeRepository,
        IProjectTaskRepository taskRepository)
    {
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
        _taskRepository = taskRepository;
    }
    
    public async Task<IEnumerable<ProjectModel>> GetAllAsync(params string[] includes)
    {
        var projects = await _projectRepository.GetAllAsync();

        return projects.Select(MapperWrapper.Map<ProjectModel>);
    }

    public async Task<ProjectModel?> GetByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        return MapperWrapper.Map<Project, ProjectModel>(project);
    }

    public async Task<int> AddAsync(ProjectModel projectModel)
    {
        var project = MapperWrapper.Map<Project>(projectModel);
        var id = await _projectRepository.AddAsync(project);

        return id;
    }

    public async Task<bool> UpdateAsync(ProjectModel projectModel)
    {
        var project = MapperWrapper.Map<Project>(projectModel);
        return await _projectRepository.UpdateAsync(project);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _projectRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProjectModel>> GetFilteredAndSortedAsync(DateTime? startDateFrom, DateTime? startDateTo,
        int? priority, string? sortBy, UserContext userContext, params string[] includes)
    {
        var startDateSpec = new ProjectStartDatePeriodSpecification(startDateFrom, startDateTo);
        var prioritySpec = new ProjectPrioritySpecification(priority);
        var managerSpec = new ProjectByManagerIdSpecification(userContext.UserId);
        var sortingSpec = new SortingSpecification<Project>(sortBy);
        var includeSpec = new IncludingSpecification<Project>(includes.Where(x => x != nameof(Project.Employees)).ToArray());

        var combinedSpec = startDateSpec.And(prioritySpec).And(sortingSpec).And(includeSpec);

        if (userContext.Role != UserRole.Administrator)
            combinedSpec = combinedSpec.And(managerSpec);

        var filtered = (await _projectRepository.GetBySpecificationAsync(combinedSpec)).ToList();

        var projects = filtered.Select(MapperWrapper.Map<ProjectModel>).ToList();
        
        if (includes.Contains(nameof(ProjectModel.Employees)))
        {
            for (int i = 0; i < filtered.Count; i++)
            {
                projects[i].Employees = new List<EmployeeModel>();
                for (int j = 0; j < filtered[i].Employees.Count; j++)
                {
                    var emp = MapperWrapper.Map<EmployeeModel>(
                        await _employeeRepository.GetByIdAsync(filtered[i].Employees[j]));
                    projects[i].Employees!.Add(emp);
                }
            }
        }

        return filtered.Select(MapperWrapper.Map<ProjectModel>);
    }
    
    public async Task<bool> AddEmployeeToProjectAsync(int projectId, Guid employeeId, UserContext userContext)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            return false;
        
        CheckUserContext(userContext, UserRole.Manager, project.ManagerId);
        
        if (project.Employees.Any(e => e == employeeId))
            return false;

        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
            return false;

        return await _projectRepository.AddEmployeeToProjectAsync(projectId, employeeId);
    }

    public async Task<bool> RemoveEmployeeFromProjectAsync(int projectId, Guid employeeId, UserContext userContext)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            return false;
        
        CheckUserContext(userContext, UserRole.Manager, project.ManagerId);

        var employee = project.Employees.Find(x => x == employeeId);

        if (employee == null)
            return false;

        return await _projectRepository.RemoveEmployeeFromProjectAsync(projectId, employeeId);
    }

    /// <summary>
    /// Добавление задачи к проекту. Если на выполенение задачи назначен сотрудник то он будет добавлен в проект.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="taskId"></param>
    /// <param name="userContext"></param>
    /// <returns></returns>
    public async Task<bool> AddTaskToProjectAsync(int projectId, int taskId, UserContext userContext)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            return false;
        
        CheckUserContext(userContext, UserRole.Manager, project.ManagerId);

        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            return false;
        
        if (project.Tasks.Any(t => t.Id == taskId))
            return false;
        
        project.Tasks.Add(task);

        if (task.AssignedToId.HasValue && !project.Employees.Exists(x => x == task.AssignedToId))
            await _projectRepository.AddEmployeeToProjectAsync(projectId, task.AssignedToId.Value);

        return await _projectRepository.UpdateAsync(project);
    }

    public async Task<bool> RemoveTaskFromProjectAsync(int projectId, int taskId, UserContext userContext)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            return false;

        CheckUserContext(userContext, UserRole.Manager, project.ManagerId);

        var task = project.Tasks.FirstOrDefault(e => e.Id == taskId);
        if (task == null)
            return false;

        project.Tasks.Remove(task);

        if (task.AssignedToId.HasValue && !project.Employees.Exists(x => x == task.AssignedToId))
            await _projectRepository.RemoveEmployeeFromProjectAsync(projectId, task.AssignedToId.Value);

        return await _projectRepository.UpdateAsync(project);
    }
    
    private void CheckUserContext(UserContext userContext, UserRole minRoleRequired, Guid projectManagerId)
    {
        if (userContext.Role > minRoleRequired)
        {
            return;
        }
        
        if (userContext.Role < minRoleRequired || userContext.UserId != projectManagerId)
        {
            throw new AccessDeniedException("Access Denied");
        }
    }
}