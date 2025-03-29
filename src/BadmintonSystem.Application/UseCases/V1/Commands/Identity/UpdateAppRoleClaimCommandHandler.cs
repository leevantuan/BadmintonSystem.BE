using System.Security.Claims;
using BadmintonSystem.Application.Extensions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;

public sealed class UpdateAppRoleClaimCommandHandler(
    RoleManager<AppRole> roleManager)
    : ICommandHandler<Command.UpdateAppRoleClaimCommand>
{
    public async Task<Result> Handle(Command.UpdateAppRoleClaimCommand request, CancellationToken cancellationToken)
    {
        AppRole role = await roleManager.FindByNameAsync(StringExtension.CapitalizeFirstLetter(request.Data.RoleCode))
                       ?? throw new IdentityException.AppRoleNotFoundException(request.Data.RoleCode);

        IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role);

        var requestDict =
            request.Data?.ListFunctions?.ToDictionary(item => item.FunctionKey.Trim().ToUpper(),
                item => item.ActionValues);

        foreach (Claim item in roleClaims.ToList())
        {
            if (requestDict.TryGetValue(item.Type, out string value))
            {
                string newValue = AuthenticationExtension.ConvertToBinary(value);

                await roleManager.RemoveClaimAsync(role, item);

                var newClaim = new Claim(item.Type, newValue);
                await roleManager.AddClaimAsync(role, newClaim);

                requestDict.Remove(item.Type);
            }
        }

        foreach (KeyValuePair<string, string> res in requestDict)
        {
            string newValue = AuthenticationExtension.ConvertToBinary(res.Value);

            var newClaim = new Claim(res.Key.ToUpper(), newValue);
            await roleManager.AddClaimAsync(role, newClaim);
        }

        return Result.Success();
    }
}
