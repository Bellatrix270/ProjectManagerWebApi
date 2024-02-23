using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Application.Specification.Project;
using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Repositories.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ProjectService(IProjectRepository projectRepository, IEmployeeRepository employeeRepository)
    {
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
    }
    
    public async Task<IEnumerable<ProjectModel>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAllAsync();

        return projects.Select(MapperWrapper.Map<ProjectModel>);
    }

    public async Task<ProjectModel> GetByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        return MapperWrapper.Map<ProjectModel>(project);
    }

    public async Task<int> AddAsync(ProjectModel projectModel)
    {
        var project = MapperWrapper.Map<Project>(projectModel);
        var id = await _projectRepository.AddAsync(project);

        return id;
    }

    public async Task<bool> UpdateAsync(ProjectModel projectModel)
    {
        var project = MapperWrapper.Map<Project>(projectModel);
        return await _projectRepository.UpdateAsync(project);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _projectRepository.DeleteAsync(id);
    }

    public IEnumerable<ProjectModel> GetFiltered(DateTime startDateFrom, DateTime startDateTo, int priority)
    {
        var startDateSpec = new StartDateSpecification(startDateFrom, startDateTo);
        var prioritySpec = new PrioritySpecification(priority);
        var combinedSpec = new CompositeSpecification<Project>(startDateSpec, prioritySpec);

        var filtered = _projectRepository.GetBySpecification(combinedSpec);

        return filtered.Select(MapperWrapper.Map<ProjectModel>);
    }

    public async Task<bool> AddEmployeeToProjectAsync(int projectId, int employeeId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            return false;

        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
            return false;

        // Check if employee is already assigned to the project
        if (project.Employees.Any(e => e.Id == employeeId))
            return false; // Employee already assigned

        project.Employees.Add(employee);
        return await _projectRepository.UpdateAsync(project);
    }

    public async Task<bool> RemoveEmployeeFromProjectAsync(int projectId, int employeeId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            return false;

        // Check if employee is assigned to the project
        var employeeToRemove = project.Employees.FirstOrDefault(e => e.Id == employeeId);
        if (employeeToRemove == null)
            return false; // Employee not assigned to the project

        project.Employees.Remove(employeeToRemove);
        return await _projectRepository.UpdateAsync(project);
    }
}