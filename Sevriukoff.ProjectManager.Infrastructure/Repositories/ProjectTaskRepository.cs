using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Repositories.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Repositories;

public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly ProjectDbContext _context;

    public ProjectTaskRepository(ProjectDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ProjectTask>> GetAllAsync()
        => await _context.ProjectTasks.ToListAsync();

    public async Task<ProjectTask?> GetByIdAsync(int id)
        => await _context.ProjectTasks.FindAsync(id);
    
    public async Task<int> AddAsync(ProjectTask projectTask)
    {
        await _context.ProjectTasks.AddAsync(projectTask);
        await _context.SaveChangesAsync();
        
        return projectTask.Id;
    }

    public async Task<bool> UpdateAsync(ProjectTask projectTask)
    {
        _context.ProjectTasks.Update(projectTask);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var projectTaskToDelete = await _context.ProjectTasks.FindAsync(id);
        if (projectTaskToDelete != null)
        {
            _context.ProjectTasks.Remove(projectTaskToDelete);
            return await _context.SaveChangesAsync() > 0;
        }

        return false;
    }
}