using System.Security.Claims;
using BadmintonSystem.Application.Extensions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;

public sealed class UpdateAppUserClaimCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    ApplicationDbContext context)
    : ICommandHandler<Command.UpdateAppUserClaimCommand>
{
    public async Task<Result> Handle(Command.UpdateAppUserClaimCommand request, CancellationToken cancellationToken)
    {
        AppUser user = await userManager.FindByEmailAsync(request.Data.Email)
                       ?? throw new IdentityException.AppUserNotFoundException(request.Data.Email);

        IList<Claim> userClaims = await userManager.GetClaimsAsync(user);

        var requestDict =
            request.Data?.ListFunctions?.ToDictionary(item => item.FunctionKey.Trim().ToUpper(),
                item => item.ActionValues);

        foreach (Claim item in userClaims.ToList())
        {
            if (requestDict.TryGetValue(item.Type, out string value))
            {
                string newValue = AuthenticationExtension.ConvertToBinary(value);

                await userManager.RemoveClaimAsync(user, item);

                var newClaim = new Claim(item.Type, newValue);
                await userManager.AddClaimAsync(user, newClaim);

                requestDict.Remove(item.Type);
            }
        }

        foreach (KeyValuePair<string, string> res in requestDict)
        {
            string newValue = AuthenticationExtension.ConvertToBinary(res.Value);

            var newClaim = new Claim(res.Key.ToUpper(), newValue);
            await userManager.AddClaimAsync(user, newClaim);
        }

        return Result.Success();
    }
}
