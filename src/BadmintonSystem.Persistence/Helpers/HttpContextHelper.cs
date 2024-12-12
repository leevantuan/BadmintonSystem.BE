using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BadmintonSystem.Persistence.Helpers;
public static class HttpContextHelper
{
    public static Guid GetCurrentUserId(this HttpContext httpContext)
    {
        var currentUserId = httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        Guid.TryParse(currentUserId, out var userId);

        return userId;
    }

    public static string? GetCurrentUserName(this HttpContext httpContext)
    {
        var currentUserName = httpContext?.User.FindFirstValue(ClaimTypes.Name);

        return currentUserName;
    }

    public static string? GetCurrentUserEmail(this HttpContext httpContext)
    {
        var currentUserEmail = httpContext?.User.FindFirstValue(ClaimTypes.Email);

        return currentUserEmail;
    }
}
