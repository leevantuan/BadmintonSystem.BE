using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Identity;

public sealed class GetRegisterQueryHandler(
    UserManager<AppUser> userManager)
    : IQueryHandler<Query.RegisterQuery>
{
    public async Task<Result> Handle(Query.RegisterQuery request, CancellationToken cancellationToken)
    {
        // check exist
        AppUser? userByEmail = await userManager.FindByEmailAsync(request.Data.Email);

        if (userByEmail != null)
        {
            throw new IdentityException.AppUserAlreadyExistException(request.Data.Email);
        }

        // valid user => add new user
        var newUser = new AppUser
        {
            UserName = request.Data.UserName.Trim(),
            Email = request.Data.Email.Trim(),
            FirstName = request.Data.FirstName.Trim(),
            LastName = request.Data.LastName.Trim(),
            FullName = StringExtension.GetFullNameFromFirstNameAndLastName(request.Data.FirstName,
                request.Data.LastName),
            SecurityStamp = Guid.NewGuid().ToString() // Set a unique security stamp
        };

        IdentityResult createUserResult = await userManager.CreateAsync(newUser, request.Data.Password);

        if (!createUserResult.Succeeded)
        {
            string errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppUserException(errors);
        }

        // add default role => Manager
        IdentityResult addDefaultRoleOfUserResult = await userManager.AddToRoleAsync(newUser,
            StringExtension.CapitalizeFirstLetter(AppRoleEnum.MANAGER.ToString()));

        if (!addDefaultRoleOfUserResult.Succeeded)
        {
            string errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppRoleException(errors);
        }

        // // add default user claim
        // AppRole role = await roleManager.FindByNameAsync(AppRoleEnum.MANAGER.ToString())
        //                ?? throw new IdentityException.AppRoleNotFoundException(AppRoleEnum.MANAGER.ToString());
        //
        // // get list role claim
        // IList<Claim> claims = await roleManager.GetClaimsAsync(role);
        //
        // await userManager.AddClaimsAsync(newUser, claims);

        return Result.Success();
    }
}
