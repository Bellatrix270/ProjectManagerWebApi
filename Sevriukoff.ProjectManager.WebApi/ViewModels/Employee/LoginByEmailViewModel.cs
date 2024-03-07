#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;

public class LoginByEmailViewModel : LoginViewModel
{
    public required string Email { get; set; }
}