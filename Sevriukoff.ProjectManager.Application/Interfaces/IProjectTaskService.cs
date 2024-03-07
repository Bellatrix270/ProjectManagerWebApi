using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IProjectTaskService
{
    Task<IEnumerable<ProjectTaskModel>> GetAllAsync();
    Task<ProjectTaskModel?> GetByIdAsync(Guid id);
    Task<(Guid? Id, IEnumerable<ValidationError> Errors)> AddAsync(ProjectTaskModel employee);
    Task<(bool success, IEnumerable<ValidationError> Errors)> UpdateAsync(ProjectTaskModel employee, UserContext userContext);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<ProjectTaskModel>> GetFilteredAndSortedAsync(ProjectTaskStatus? status, int? priority,
        Guid? createdById, Guid? assignedToId, string? sortBy, UserContext userContext, params string[] includes);
}