using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Infrastructure.Interfaces;

public interface IProjectRepository : IRepository<Project, Guid>
{
    Task<bool> AddEmployeeToProjectAsync(Guid projectId, Guid employeeId);
    Task<bool> RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId);
}