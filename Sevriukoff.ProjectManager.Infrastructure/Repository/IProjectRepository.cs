using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Infrastructure.Repository;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<int> AddAsync(Project project);
    Task<bool> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
    IEnumerable<Project> GetBySpecification(Specification<Project> specification);
}