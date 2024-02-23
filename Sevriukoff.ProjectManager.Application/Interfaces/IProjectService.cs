using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectModel>> GetAllAsync();
    Task<ProjectModel> GetByIdAsync(int id);
    Task<int> AddAsync(ProjectModel projectModel);
    Task<bool> UpdateAsync(ProjectModel projectModel);
    Task<bool> DeleteAsync(int id);
    IEnumerable<ProjectModel> GetFiltered(DateTime startDateFrom, DateTime startDateTo, int priority);

    Task<bool> AddEmployeeToProjectAsync(int projectId, int employeeId);
    Task<bool> RemoveEmployeeFromProjectAsync(int projectId, int employeeId);

    Task<bool> AddTaskToProjectAsync(int projectId, int taskId);
    Task<bool> RemoveTaskFromProjectAsync(int projectId, int taskId);
}