using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetRegisterByCustomerQueryHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    ApplicationDbContext context)
    : IQueryHandler<Query.RegisterByCustomerQuery>
{
    public async Task<Result> Handle(Query.RegisterByCustomerQuery request, CancellationToken cancellationToken)
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
            Id = Guid.NewGuid(),
            UserName = request.Data.UserName.Trim(),
            Email = request.Data.Email.Trim(),
            FirstName = request.Data.FirstName.Trim(),
            LastName = request.Data.LastName.Trim(),
            PhoneNumber = request.Data.PhoneNumber.Trim(),
            Gender = (GenderEnum)request.Data.Gender,
            DateOfBirth = request.Data.DateOfBirth,
            FullName = StringExtension.GetFullNameFromFirstNameAndLastName(request.Data.FirstName,
                request.Data.LastName),
            SecurityStamp = Guid.NewGuid().ToString() // Set a unique security stamp
        };

        var newAddress = new Address
        {
            Id = Guid.NewGuid(),
            AddressLine1 = request.Data.AddressLine1,
            AddressLine2 = request.Data.AddressLine2,
            Street = request.Data.Street,
            City = request.Data.City,
            Unit = request.Data.Unit,
            Province = request.Data.Province
        };

        var newAppUserAddress = new UserAddress
        {
            AddressId = newAddress.Id,
            UserId = newUser.Id,
            IsDefault = DefaultEnum.TRUE
        };

        IdentityResult createUserResult = await userManager.CreateAsync(newUser, request.Data.Password);

        if (!createUserResult.Succeeded)
        {
            string errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppUserException(errors);
        }

        // add default role => CUSTOMER
        IdentityResult addDefaultRoleOfUserResult = await userManager.AddToRoleAsync(newUser,
            StringExtension.CapitalizeFirstLetter(AppRoleEnum.CUSTOMER.ToString()));

        if (!addDefaultRoleOfUserResult.Succeeded)
        {
            string errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppRoleException(errors);
        }

        // add default user claim
        // AppRole role = await roleManager.FindByNameAsync(AppRoleEnum.CUSTOMER.ToString())
        //                ?? throw new IdentityException.AppRoleNotFoundException(AppRoleEnum.CUSTOMER.ToString());
        //
        // // get list role claim
        // IList<Claim> claims = await roleManager.GetClaimsAsync(role);
        //
        // await userManager.AddClaimsAsync(newUser, claims);

        context.Address.Add(newAddress);

        context.UserAddress.Add(newAppUserAddress);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
