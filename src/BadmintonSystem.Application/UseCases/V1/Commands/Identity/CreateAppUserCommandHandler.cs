using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;

public sealed class CreateAppUserCommandHandler : ICommandHandler<Command.CreateAppUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public CreateAppUserCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Result> Handle(Command.CreateAppUserCommand request, CancellationToken cancellationToken)
    {
        var userByEmail = await _userManager.FindByEmailAsync(request.Data.Email);

        if (userByEmail != null)
        {
            throw new IdentityException.AppUserAlreadyExistException(request.Data.Email);
        }

        var newUser = new AppUser
        {
            UserName = request.Data.UserName.Trim(),
            Email = request.Data.Email.Trim(),
            FirstName = request.Data.FirstName.Trim(),
            LastName = request.Data.LastName.Trim(),
            FullName = GetFullName(request.Data.FirstName, request.Data.LastName),
            SecurityStamp = Guid.NewGuid().ToString() // Set a unique security stamp
        };

        var createUserResult = await _userManager.CreateAsync(newUser, request.Data.Password);

        if (!createUserResult.Succeeded)
        {
            var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppUserException(errors);
        }

        var roleNameRequest = StringExtension.CapitalizeFirstLetter(request.Data.RoleCode.ToString());
        var addDefaultRoleOfUserResult = await _userManager.AddToRoleAsync(newUser, roleNameRequest);

        if (!addDefaultRoleOfUserResult.Succeeded)
        {
            var errors = string.Join(", ", addDefaultRoleOfUserResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppRoleException(errors);
        }

        await SetUserClaimWithRoleClaim(roleNameRequest, newUser);

        return Result.Success();
    }

    private static string GetFullName(string firstName, string lastName)
    {
        string fullName = StringExtension.CapitalizeFirstLetter(firstName) + " " + StringExtension.CapitalizeFirstLetter(lastName);
        return fullName;
    }

    private async Task SetUserClaimWithRoleClaim(string roleNameRequest, AppUser newUser)
    {
        var role = await _roleManager.FindByNameAsync(roleNameRequest)
            ?? throw new IdentityException.AppRoleNotFoundException(roleNameRequest);

        var roleClaims = await _roleManager.GetClaimsAsync(role);

        await _userManager.AddClaimsAsync(newUser, roleClaims);
    }
}
