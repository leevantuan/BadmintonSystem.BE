using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BadmintonSystem.Application.Attributes;
public sealed class CookieAuthorizeAttribute : TypeFilterAttribute
{
    public string RoleName { get; set; }

    public string ActionValue { get; set; }

    public CookieAuthorizeAttribute(string roleName, string actionValue) : base(typeof(CookieAuthorizedFilter))
    {
        RoleName = roleName;
        ActionValue = actionValue;
        Arguments = new object[] { RoleName, ActionValue };
    }
}

public class CookieAuthorizedFilter : IAuthorizationFilter
{
    public string RoleName { get; set; }

    public string ActionValue { get; set; }

    public CookieAuthorizedFilter(string roleName, string actionValue)
    {
        RoleName = roleName;
        ActionValue = actionValue;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!CanAccessToAction(context.HttpContext))
            context.Result = new ForbidResult();
    }

    private bool CanAccessToAction(HttpContext httpContext)
    {
        var roles = httpContext.User.FindFirstValue(ClaimTypes.Role);

        if (roles.Equals(RoleName))
        {
            return true;
        }

        return false;
    }
}
