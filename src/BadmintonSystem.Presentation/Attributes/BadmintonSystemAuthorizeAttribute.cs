using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BadmintonSystem.Presentation.Attributes;
public sealed class BadmintonSystemAuthorizeAttribute : TypeFilterAttribute
{
    public string RoleName { get; set; }

    public string ActionValue { get; set; }

    // base: truyền vaò typeof ==> nó kế thừa từ AuthorizeFilter
    public BadmintonSystemAuthorizeAttribute(string roleName, string actionValue)
        : base(typeof(BadmintonSystemAuthorizeFilter))
    {
        RoleName = roleName;
        ActionValue = actionValue;
        Arguments = new object[] { RoleName, ActionValue };
    }
}

public class BadmintonSystemAuthorizeFilter : IAuthorizationFilter
{
    public string RoleName { get; set; }

    public string ActionValue { get; set; }

    public BadmintonSystemAuthorizeFilter(string roleName, string actionValue)
    {
        RoleName = roleName;
        ActionValue = actionValue;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!CanAccessToAction(context.HttpContext))
            context.Result = new ForbidResult();
    }

    // Func check cos quyền hay không
    private bool CanAccessToAction(HttpContext httpContext)
    {
        var roles = httpContext.User.FindFirstValue(ClaimTypes.Role);

        if (roles.Equals(RoleName))
            return true;

        return false;
    }

}
