using System.Text.Json.Serialization;
using Sevriukoff.ProjectManager.WebApi.Mapping;
#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;

[JsonConverter(typeof(EmployeeLoginConverter))]
public class LoginViewModel
{
    public required string Password { get; set; }
}