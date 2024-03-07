using System.ComponentModel;
using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;
using Sevriukoff.ProjectManager.WebApi.ViewModels.Project;
using Sevriukoff.ProjectManager.WebApi.ViewModels.ProjectTask;

#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.Mapping;

public class MapperConfiguration : IMapperConfiguration
{
    public void Configure(MapperWrapperConfiguration wrapperConfiguration)
    {
        TypeDescriptor.AddAttributes(typeof(EmployeeModel),
            new TypeConverterAttribute(typeof(EmployeeModelToViewConverter)));
        
       wrapperConfiguration.Bind<EmployeeModel, EmployeeViewModel>();
       wrapperConfiguration.Bind<EmployeeCreateViewModel, EmployeeModel>();
       wrapperConfiguration.Bind<EmployeeUpdateViewModel, EmployeeModel>();
       
       wrapperConfiguration.Bind<ProjectModel, ProjectViewModel>();
       wrapperConfiguration.Bind<ProjectCreateViewModel, ProjectModel>();
       wrapperConfiguration.Bind<ProjectUpdateViewModel, ProjectModel>();
       
       wrapperConfiguration.Bind<ProjectTaskModel, ProjectTaskViewModel>();
       wrapperConfiguration.Bind<ProjectTaskCreateViewModel, ProjectTaskModel>();
       wrapperConfiguration.Bind<ProjectTaskUpdateViewModel, ProjectTaskModel>();
    }
}