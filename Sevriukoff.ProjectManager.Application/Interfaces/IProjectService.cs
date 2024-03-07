using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectModel>> GetAllAsync(params string[] includes);
    Task<ProjectModel?> GetByIdAsync(Guid id);
    Task<(Guid? Id, IEnumerable<ValidationError> Errors)> AddAsync(ProjectModel projectModel);
    Task<(bool success, IEnumerable<ValidationError> Errors)> UpdateAsync(ProjectModel projectModel, UserContext userContext);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<ProjectModel>> GetFilteredAndSortedAsync(DateTime? startDateFrom, DateTime? startDateTo,
        int? priority, string? sortBy, UserContext userContext, params string[] includes);
    
    Task<bool> AddEmployeeToProjectAsync(Guid projectId, Guid employeeId, UserContext userContext);
    Task<bool> RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId, UserContext userContext);

    Task<bool> AddTaskToProjectAsync(Guid projectId, Guid taskId, UserContext userContext);
    Task<bool> RemoveTaskFromProjectAsync(Guid projectId, Guid taskId, UserContext userContext);

    #region Obsolute
    
    //Task<ISpecification<ProjectModel>> GetFilteredAsync(DateTime? startDateFrom, DateTime? startDateTo, int? priority);
    //Task<ISpecification<ProjectModel>> GetSortedAsync(string? sortBy, bool? ascending = true);
    //Task<IEnumerable<ProjectModel>> GetBySpecification(params ISpecification<ProjectModel>[] specifications);
    
    #endregion
}