using System.Security.Claims;
using BadmintonSystem.Application.Extensions;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BadmintonSystem.Application.Attributes;

public sealed class JwtAuthorizeAttribute : TypeFilterAttribute
{
    public JwtAuthorizeAttribute(string functionKey, int actionValue)
        : base(typeof(JwtAuthorizeFilter))
    {
        FunctionKey = functionKey;
        ActionValue = actionValue;
        Arguments = new object[] { FunctionKey, ActionValue };
    }

    public string FunctionKey { get; set; }

    public int ActionValue { get; set; }
}

public sealed class JwtAuthorizeFilter : IAuthorizationFilter
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public JwtAuthorizeFilter
        (string functionKey, int actionValue, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        FunctionKey = functionKey;
        ActionValue = actionValue;
    }

    public string FunctionKey { get; set; }

    public int ActionValue { get; set; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool isAccess = CanAccessToAction(context.HttpContext).GetAwaiter().GetResult();
        if (!isAccess)
        {
            context.Result = new ForbidResult();
        }
    }

    public async Task<bool> CanAccessToAction(HttpContext httpContext)
    {
        // get user
        string? userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return false; // user not authenticated
        }

        AppUser? userById = await _userManager.FindByIdAsync(userId);

        if (userById == null)
        {
            return false; // user not found
        }

        // get user claim
        IList<Claim> userClaims = await _userManager.GetClaimsAsync(userById);

        var resultClaims = userClaims.ToDictionary(item => item.Type, item => item.Value);

        IList<string> roles = await _userManager.GetRolesAsync(userById);

        // ROLE CLAIMS
        foreach (string roleName in roles)
        {
            string roleNameCapitalize = StringExtension.Uppercase(roleName);

            AppRole? role = await _roleManager.FindByNameAsync(roleNameCapitalize)
                            ?? throw new IdentityException.AppRoleException(roleNameCapitalize);

            IList<Claim> roleClaim = await _roleManager.GetClaimsAsync(role);

            // MERGE ROLE
            resultClaims = AuthenticationExtension.MergeClaims(resultClaims, roleClaim);
        }

        userClaims = resultClaims
            .Select(e => new Claim(e.Key, e.Value))
            .ToList();

        Claim? userFunctionClaim = userClaims.FirstOrDefault(c => c.Type == FunctionKey);

        if (userFunctionClaim == null)
        {
            return false; // No permission claim found for the function
        }

        if (int.TryParse(userFunctionClaim.Value, out int userActionValue))
        {
            ActionValue = 1 << ActionValue; // dich bit operator
            return (userActionValue & ActionValue) == ActionValue; // and operator
        }

        return false;
    }
}
