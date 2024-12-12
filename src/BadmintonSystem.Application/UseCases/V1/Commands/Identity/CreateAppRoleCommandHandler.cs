using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;
public sealed class CreateAppRoleCommandHandler : ICommandHandler<Command.CreateAppRoleCommand>
{
    private readonly RoleManager<AppRole> _roleManager;

    public CreateAppRoleCommandHandler(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(Command.CreateAppRoleCommand request, CancellationToken cancellationToken)
    {
        // check exist
        var roleByRoleCode = await _roleManager.Roles.FirstOrDefaultAsync(r => r.RoleCode == request.Data.RoleCode, cancellationToken);

        if (roleByRoleCode != null)
        {
            throw new IdentityException.AppRoleAlreadyExistException(roleByRoleCode.RoleCode);
        }

        var role = new AppRole()
        {
            Name = request.Data.RoleCode.Trim(),
            RoleCode = request.Data.RoleCode.Trim().ToUpper(),
            Description = request.Data.Description.Trim()
        };

        await _roleManager.CreateAsync(role);

        return Result.Success();
    }
}
