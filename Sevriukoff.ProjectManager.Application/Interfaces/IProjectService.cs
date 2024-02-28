using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Base;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectModel>> GetAllAsync(params string[] includes);
    Task<ProjectModel?> GetByIdAsync(int id);
    Task<int> AddAsync(ProjectModel projectModel);
    Task<bool> UpdateAsync(ProjectModel projectModel, UserContext userContext);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ProjectModel>> GetFilteredAndSortedAsync(DateTime? startDateFrom, DateTime? startDateTo,
        int? priority, string? sortBy, UserContext userContext, params string[] includes);
    
    Task<bool> AddEmployeeToProjectAsync(int projectId, Guid employeeId, UserContext userContext);
    Task<bool> RemoveEmployeeFromProjectAsync(int projectId, Guid employeeId, UserContext userContext);

    Task<bool> AddTaskToProjectAsync(int projectId, int taskId, UserContext userContext);
    Task<bool> RemoveTaskFromProjectAsync(int projectId, int taskId, UserContext userContext);
    
    //Task<ISpecification<ProjectModel>> GetFilteredAsync(DateTime? startDateFrom, DateTime? startDateTo, int? priority);
    //Task<ISpecification<ProjectModel>> GetSortedAsync(string? sortBy, bool? ascending = true);
    //Task<IEnumerable<ProjectModel>> GetBySpecification(params ISpecification<ProjectModel>[] specifications);
}