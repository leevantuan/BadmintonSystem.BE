using System.Security.Claims;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;

public sealed class UpdateAppRoleClaimCommandHandler : ICommandHandler<Command.UpdateAppRoleClaimCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public UpdateAppRoleClaimCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Result> Handle(Command.UpdateAppRoleClaimCommand request, CancellationToken cancellationToken)
    {
        AppRole role = await _roleManager.FindByNameAsync(StringExtension.CapitalizeFirstLetter(request.Data.RoleCode))
                       ?? throw new IdentityException.AppRoleNotFoundException(request.Data.RoleCode);

        IList<Claim> roleClaims = await _roleManager.GetClaimsAsync(role);

        var requestDict =
            request.Data?.ListFunctions?.ToDictionary(item => item.FunctionKey.Trim().ToUpper(),
                item => item.ActionValues);

        foreach (Claim item in roleClaims.ToList())
        {
            if (requestDict.TryGetValue(item.Type, out string value))
            {
                string newValue = ConvertBinary(value);

                await _roleManager.RemoveClaimAsync(role, item);

                var newClaim = new Claim(item.Type, newValue);
                await _roleManager.AddClaimAsync(role, newClaim);

                requestDict.Remove(item.Type);
            }
        }

        foreach (KeyValuePair<string, string> res in requestDict)
        {
            string newValue = ConvertBinary(res.Value);

            var newClaim = new Claim(res.Key.ToUpper(), newValue);
            await _roleManager.AddClaimAsync(role, newClaim);
        }

        return Result.Success();
    }

    private static string ConvertBinary(string value)
    {
        var listAction = value.Trim().Split(',').ToList();

        int newValue = 0;
        foreach (string action in listAction)
        {
            newValue += 1 << Convert.ToInt32(action);
        }

        return newValue.ToString();
    }
}
