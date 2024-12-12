using System.Security.Claims;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;
public sealed class CreateAppRoleClaimCommandHandler : ICommandHandler<Command.CreateAppRoleClaimCommand>
{
    private readonly RoleManager<AppRole> _roleManager;

    public CreateAppRoleClaimCommandHandler(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(Command.CreateAppRoleClaimCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByNameAsync(request.Data.RoleName.ToString())
            ?? throw new IdentityException.AppRoleNotFoundException(request.Data.RoleName);

        var claim = new Claim(request.Data.FunctionKey.Trim().ToUpper(), request.Data.ActionValue.ToString());

        await _roleManager.AddClaimAsync(role, claim);

        return Result.Success();
    }
}
