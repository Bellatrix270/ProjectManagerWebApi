namespace Sevriukoff.ProjectManager.Application.Models;

public class UserContext
{
    public Guid UserId { get; set; }
    public UserRole Role { get; set; }
}