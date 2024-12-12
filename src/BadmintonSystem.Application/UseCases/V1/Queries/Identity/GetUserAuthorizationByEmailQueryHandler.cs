using System.Security.Claims;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Identity;

public sealed class GetUserAuthorizationByEmailQueryHandler
    : IQueryHandler<Query.GetUserAuthorizationByEmailQuery, List<Response.UserAuthorization>>
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public GetUserAuthorizationByEmailQueryHandler(UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Result<List<Response.UserAuthorization>>> Handle(
        Query.GetUserAuthorizationByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var results = new List<Response.UserAuthorization>();

        AppUser user = await _userManager.FindByEmailAsync(request.Email)
                       ?? throw new IdentityException.AppUserException(request.Email);

        IList<Claim>? claims = await _userManager.GetClaimsAsync(user);

        if (claims == null || !claims.Any())
        {
            return Result.Success(results);
        }

        var functionKeys = Enum.GetValues<FunctionEnum>()
            .Select(e => e.ToString())
            .ToList();

        foreach (string function in functionKeys)
        {
            try
            {
                string? value = claims.FirstOrDefault(x => x.Type == function)?.Value;
                Response.UserAuthorization? result = ActionHandler(function, value);

                if (result != null)
                {
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return Result.Success(results);
    }

    private static Response.UserAuthorization ActionHandler(string? function, string? value)
    {
        if (string.IsNullOrEmpty(function))
        {
            return null;
        }

        var result = new Response.UserAuthorization
        {
            FunctionKey = function,
            Action = new List<string>()
        };

        if (string.IsNullOrEmpty(value) || !int.TryParse(value, out int actionUserValue))
        {
            return result;
        }

        var actionsEnum = Enum.GetValues<ActionEnum>()
            .Select(e => (int)e)
            .ToList();

        foreach (int action in actionsEnum)
        {
            int actionValueInitial = action;
            int actionValue = 1 << actionValueInitial;

            if ((actionUserValue & actionValue) == actionValue)
            {
                result.Action.Add(actionValueInitial.ToString());
            }
        }

        return result;
    }
}
