namespace Sevriukoff.ProjectManager.Application.Exception;

public class AccessDeniedException : System.Exception
{
    public AccessDeniedException(string message) : base(message) { }
}