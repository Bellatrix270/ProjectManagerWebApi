using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Application.Strategies.ProjectUpdateStrategy;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Factories;

public class ProjectUpdateStrategyFactory
{
    private readonly IProjectRepository _projectRepository;

    public ProjectUpdateStrategyFactory(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public IProjectUpdateStrategy CreateStrategy(UserContext userContext)
    {
        return userContext.Role switch
        {
            UserRole.Administrator => new AdministratorProjectUpdateStrategy(_projectRepository),
            UserRole.Manager => new ManagerProjectUpdateStrategy(_projectRepository),
            UserRole.Employee => new EmployeeProjectUpdateStrategy(),
            _ => throw new InvalidOperationException("Неизвестная роль пользователя.")
        };
    }
}