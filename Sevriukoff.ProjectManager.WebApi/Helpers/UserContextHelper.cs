using System.Security.Claims;
using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.WebApi.Helpers;

public class UserContextHelper
{
    public static UserContext GetUserContext(ClaimsPrincipal user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        var roleClaim = user.FindFirst(ClaimTypes.Role);

        if (userIdClaim == null || roleClaim == null)
            throw new InvalidOperationException("User context cannot be determined.");

        return new UserContext
        {
            UserId = Guid.Parse(userIdClaim.Value),
            Role = Enum.Parse<UserRole>(roleClaim.Value)
        };
    }
}