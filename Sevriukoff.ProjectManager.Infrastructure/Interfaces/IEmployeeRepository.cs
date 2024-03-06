using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Infrastructure.Interfaces;

public interface IEmployeeRepository : IRepository<Employee, Guid>
{
    Task<Employee?> GetByEmailAsync(string email);
}