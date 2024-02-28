using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Base;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectModel>> GetAllAsync(params string[] includes);
    Task<ProjectModel?> GetByIdAsync(int id);
    Task<int> AddAsync(ProjectModel projectModel);
    Task<bool> UpdateAsync(ProjectModel projectModel);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ProjectModel>> GetFilteredAndSortedAsync(DateTime? startDateFrom, DateTime? startDateTo,
        int? priority, string? sortBy, params string[] includes);
    
    Task<bool> AddEmployeeToProjectAsync(int projectId, int employeeId);
    Task<bool> RemoveEmployeeFromProjectAsync(int projectId, int employeeId);

    Task<bool> AddTaskToProjectAsync(int projectId, int taskId);
    Task<bool> RemoveTaskFromProjectAsync(int projectId, int taskId);
    
    //Task<ISpecification<ProjectModel>> GetFilteredAsync(DateTime? startDateFrom, DateTime? startDateTo, int? priority);
    //Task<ISpecification<ProjectModel>> GetSortedAsync(string? sortBy, bool? ascending = true);
    //Task<IEnumerable<ProjectModel>> GetBySpecification(params ISpecification<ProjectModel>[] specifications);
}