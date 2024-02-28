using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IProjectTaskService
{
    Task<IEnumerable<ProjectTaskModel>> GetAllAsync();
    Task<ProjectTaskModel?> GetByIdAsync(int id);
    Task<int> AddAsync(ProjectTaskModel employee);
    Task<bool> UpdateAsync(ProjectTaskModel employee);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ProjectTaskModel>> GetFilteredAndSortedAsync(ProjectTaskStatus? status, int? priority,
        Guid? createdById, Guid? assignedToId, string? sortBy, UserContext userContext, params string[] includes);
}