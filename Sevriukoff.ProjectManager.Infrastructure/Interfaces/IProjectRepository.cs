using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Infrastructure.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync(ISpecification<Project>? specification = null);
    Task<Project?> GetByIdAsync(int id, ISpecification<Project> specification = null);
    Task<int> AddAsync(Project project);
    Task<bool> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Project>> GetBySpecificationAsync(ISpecification<Project> specification);
    Task<bool> AddEmployeeToProjectAsync(int projectId, Guid employeeId);
    Task<bool> RemoveEmployeeFromProjectAsync(int projectId, Guid employeeId);
}