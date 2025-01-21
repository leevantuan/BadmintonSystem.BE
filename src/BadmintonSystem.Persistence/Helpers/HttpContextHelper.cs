using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BadmintonSystem.Persistence.Helpers;

public static class HttpContextHelper
{
    public static Guid GetCurrentUserId(this HttpContext httpContext)
    {
        string? currentUserId = httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        Guid.TryParse(currentUserId, out Guid userId);

        return userId;
    }

    public static string? GetCurrentUserName(this HttpContext httpContext)
    {
        string? currentUserName = httpContext?.User.FindFirstValue(ClaimTypes.Name);

        return currentUserName;
    }

    public static string? GetCurrentUserEmail(this HttpContext httpContext)
    {
        string? currentUserEmail = httpContext?.User.FindFirstValue(ClaimTypes.Email);

        return currentUserEmail;
    }

    public static List<string>? GetCurrentRoles(this HttpContext httpContext)
    {
        string? serializedRoles = httpContext?.User.FindFirstValue(ClaimTypes.Role);

        return JsonConvert.DeserializeObject<List<string>>(serializedRoles ?? string.Empty);
    }
}
