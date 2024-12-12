using System.Security.Claims;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;

public sealed class UpdateAppUserClaimCommandHandler : ICommandHandler<Command.UpdateAppUserClaimCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public UpdateAppUserClaimCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Result> Handle(Command.UpdateAppUserClaimCommand request, CancellationToken cancellationToken)
    {
        AppUser user = await _userManager.FindByEmailAsync(request.Data.Email)
                       ?? throw new IdentityException.AppUserNotFoundException(request.Data.Email);

        IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);

        var requestDict =
            request.Data?.ListFunctions?.ToDictionary(item => item.FunctionKey.Trim().ToUpper(),
                item => item.ActionValues);

        foreach (Claim item in userClaims.ToList())
        {
            if (requestDict.TryGetValue(item.Type, out string value))
            {
                string newValue = ConvertBinary(value);

                await _userManager.RemoveClaimAsync(user, item);

                var newClaim = new Claim(item.Type, newValue);
                await _userManager.AddClaimAsync(user, newClaim);

                requestDict.Remove(item.Type);
            }
        }

        foreach (KeyValuePair<string, string> res in requestDict)
        {
            string newValue = ConvertBinary(res.Value);

            var newClaim = new Claim(res.Key.ToUpper(), newValue);
            await _userManager.AddClaimAsync(user, newClaim);
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
