using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Repositories;

public class ProjectRepository : BaseRepository<Project, Guid, ProjectDbContext>, IProjectRepository
{
    public ProjectRepository(ProjectDbContext context) : base(context) { }

    public override async Task<IEnumerable<Project>> GetAllAsync(ISpecification<Project>? specification = null)
        => await GetEmployeesInProjects((await base.GetAllAsync(specification)).ToList());

    public override async Task<Project?> GetByIdAsync(Guid id)
        => await GetEmployeeInProject(await base.GetByIdAsync(id));

    public async Task<bool> AddEmployeeToProjectAsync(Guid projectId, Guid employeeId)
    {
        await Context.ProjectEmployee.AddAsync(new ProjectEmployee { ProjectId = projectId, EmployeeId = employeeId });
        return await Context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId)
    {
        var projectEmployee = await Context.ProjectEmployee.FindAsync(projectId, employeeId);
        await Context.ProjectEmployee.AddAsync(projectEmployee!);
        return await Context.SaveChangesAsync() > 0;
    }

    [Obsolete("Use specification instead")]
    public async Task<IEnumerable<Project>> GetByFilters(DateTime? startDateFrom, DateTime? startDateTo, int? priority)
    {
        var query = Context.Projects.AsQueryable();

        if (startDateFrom != null)
            query = query.Where(p => p.StartDate >= startDateFrom);

        if (startDateTo != null)
            query = query.Where(p => p.StartDate <= startDateTo);

        if (priority != null)
            query = query.Where(p => p.Priority == priority);

        return await query.ToListAsync();
    }

    #region privateMethods
    
    private async Task<Project?> GetEmployeeInProject(Project? project)
    {
        if (project == null)
            return null;
        
        //TODO: Я в ручную добавлял внешний ключ на проекты, можно написать sql запрос в ручную для оптимизации.
        var projectEmployees = await Context.ProjectEmployee
            .Where(pe => pe.ProjectId == project.Id)
            .Select(pe => pe.EmployeeId)
            .ToListAsync();

        project.Employees = projectEmployees;

        return project;
    }

    private async Task<List<Project>> GetEmployeesInProjects(List<Project> projects)
    {
        foreach (var project in projects)
        {
            await GetEmployeeInProject(project);
        }
        
        return projects;
    }
    
    #endregion
}