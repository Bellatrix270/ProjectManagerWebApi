using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectDbContext _context;

    public ProjectRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync(ISpecification<Project>? specification = null)
    {
        if (specification != null)
        {
            return await GetEmployeesInProject
            (
                await SpecificationEvaluator<Project>.GetQuery(_context.Set<Project>().AsQueryable(), specification)
                .ToListAsync()
            );
        }
        
        return await GetEmployeesInProject
        (
            await _context.Projects.ToListAsync()
        );
    }

    public async Task<Project?> GetByIdAsync(int id, ISpecification<Project> specification = null)
        => await GetEmployeeInProject(await _context.Projects.Include(x => x.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id));

    public async Task<int> AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        return project.Id;
    }

    public async Task<bool> UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Project>> GetBySpecificationAsync(ISpecification<Project> specification)
    {
        var query = SpecificationEvaluator<Project>.GetQuery(_context.Set<Project>().AsQueryable(),
            specification);
        
        return await GetEmployeesInProject(await query.ToListAsync());
    }

    public async Task<bool> AddEmployeeToProjectAsync(int projectId, Guid employeeId)
    {
        await _context.ProjectEmployee.AddAsync(new ProjectEmployee { ProjectId = projectId, EmployeeId = employeeId });
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveEmployeeFromProjectAsync(int projectId, Guid employeeId)
    {
        var projectEmployee = await _context.ProjectEmployee.FindAsync(projectId, employeeId);
        await _context.ProjectEmployee.AddAsync(projectEmployee!);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Project>> GetByFilters(DateTime? startDateFrom, DateTime? startDateTo, int? priority)
    {
        var query = _context.Projects.AsQueryable();

        if (startDateFrom != null)
            query = query.Where(p => p.StartDate >= startDateFrom);

        if (startDateTo != null)
            query = query.Where(p => p.StartDate <= startDateTo);

        if (priority != null)
            query = query.Where(p => p.Priority == priority);

        return await query.ToListAsync();
    }

    private async Task<Project?> GetEmployeeInProject(Project? project)
    {
        if (project == null)
            return null;
        
        //TODO: Я в ручную добавлял внешний ключ на проекты, можно написать sql запрос в ручную для оптимизации.
        var projectEmployees = await _context.ProjectEmployee
            .Where(pe => pe.ProjectId == project.Id)
            .Select(pe => pe.EmployeeId)
            .ToListAsync();

        project.Employees = projectEmployees;

        return project;
    }

    private async Task<List<Project>> GetEmployeesInProject(List<Project> projects)
    {
        foreach (var project in projects)
        {
            await GetEmployeeInProject(project);
        }
        
        return projects;
    }
}