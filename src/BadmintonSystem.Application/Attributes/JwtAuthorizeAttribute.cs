using System.Security.Claims;
using BadmintonSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BadmintonSystem.Application.Attributes;
public sealed class JwtAuthorizeAttribute : TypeFilterAttribute
{
    public string FunctionKey { get; set; }

    public int ActionValue { get; set; }

    public JwtAuthorizeAttribute(string functionKey, int actionValue) : base(typeof(JwtAuthorizeFilter))
    {
        FunctionKey = functionKey;
        ActionValue = actionValue;
        Arguments = new object[] { FunctionKey, ActionValue };
    }
}

public sealed class JwtAuthorizeFilter : IAuthorizationFilter
{
    private readonly UserManager<AppUser> _userManager;

    public string FunctionKey { get; set; }

    public int ActionValue { get; set; }

    public JwtAuthorizeFilter(string functionKey, int actionValue, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        FunctionKey = functionKey;
        ActionValue = actionValue;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isAccess = CanAccessToAction(context.HttpContext).GetAwaiter().GetResult();
        if (!isAccess)
            context.Result = new ForbidResult();
    }

    public async Task<bool> CanAccessToAction(HttpContext httpContext)
    {
        // get user
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return false; // user not authenticated
        }

        var userById = await _userManager.FindByIdAsync(userId);

        if (userById == null)
        {
            return false; // user not found
        }

        // get user claim
        var userClaims = await _userManager.GetClaimsAsync(userById);

        var userFunctionClaim = userClaims.FirstOrDefault(c => c.Type == FunctionKey);

        if (userFunctionClaim == null)
        {
            return false; // No permission claim found for the function
        }

        if (int.TryParse(userFunctionClaim.Value, out var userActionValue))
        {
            ActionValue = 1 << ActionValue; // dich bit operator
            return (userActionValue & ActionValue) == ActionValue; // and operator
        }

        return false;
    }
}
