using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Identity;
public sealed class GetRegisterQueryHandler : IQueryHandler<Query.RegisterQuery>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public GetRegisterQueryHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Result> Handle(Query.RegisterQuery request, CancellationToken cancellationToken)
    {
        // check exist
        var userByEmail = await _userManager.FindByEmailAsync(request.Data.Email);

        if (userByEmail != null)
        {
            throw new IdentityException.AppUserAlreadyExistException(request.Data.Email);
        }

        // valid user => add new user
        var newUser = new AppUser()
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

        // initial role
        await InitialAppRoleAsync();

        // add default role => CUSTOMER
        var addDefaultRoleOfUserResult = await _userManager.AddToRoleAsync(newUser, StringExtension.CapitalizeFirstLetter(AppRoleEnum.CUSTOMER.ToString()));

        if (!addDefaultRoleOfUserResult.Succeeded)
        {
            var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppRoleException(errors);
        }

        // add default user claim
        //var role = await _roleManager.FindByNameAsync(AppRoleEnum.CUSTOMER.ToString())
        //    ?? throw new IdentityException.AppRoleNotFoundException(AppRoleEnum.CUSTOMER.ToString());

        // get list role claim
        //var roleClaims = role.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue))
        //                            .ToList();

        //await _userManager.AddClaimsAsync(newUser, roleClaims);

        return Result.Success();
    }

    private static string GetFullName(string firstName, string lastName)
    {
        var fullName = $"{lastName.Trim()} {firstName.Trim()}";

        return fullName;
    }

    private async Task InitialAppRoleAsync()
    {
        if (!_roleManager.Roles.Any())
        {
            foreach (AppRoleEnum roleEnum in Enum.GetValues(typeof(AppRoleEnum)))
            {
                var roleName = roleEnum.ToString();

                // check if the role already exists
                if (await _roleManager.RoleExistsAsync(roleName))
                {
                    continue; // Skip creation if the role already exists
                }

                // Create the new role
                var role = new AppRole
                {
                    Name = StringExtension.CapitalizeFirstLetter(roleName),
                    RoleCode = roleName,
                    Description = roleName
                };

                var createRoleResult = await _roleManager.CreateAsync(role);

                if (!createRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", createRoleResult.Errors.Select(e => e.Description));
                    throw new IdentityException.AppRoleException(errors);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
