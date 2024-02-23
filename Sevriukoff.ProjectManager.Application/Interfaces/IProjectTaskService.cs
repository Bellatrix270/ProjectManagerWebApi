using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IProjectTaskService
{
    Task<IEnumerable<ProjectTaskModel>> GetAllAsync();
    Task<ProjectTaskModel> GetByIdAsync(int id);
    Task<int> AddAsync(ProjectTaskModel employee);
    Task<bool> UpdateAsync(ProjectTaskModel employee);
    Task<bool> DeleteAsync(int id);
}