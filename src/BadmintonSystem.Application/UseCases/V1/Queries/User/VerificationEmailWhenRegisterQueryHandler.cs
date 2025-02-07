using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Command = BadmintonSystem.Contract.Services.V1.ChatRoom.Command;
using Request = BadmintonSystem.Contract.Services.V1.User.Request;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class VerificationEmailWhenRegisterQueryHandler(
    IRedisService redisService,
    UserManager<AppUser> userManager,
    IRegisterHub registerHub,
    ISender sender,
    ApplicationDbContext context)
    : IQueryHandler<Query.VerificationEmailWhenRegisterQuery>
{
    public async Task<Result> Handle
        (Query.VerificationEmailWhenRegisterQuery request, CancellationToken cancellationToken)
    {
        string userRequestJson = await redisService.GetAsync(request.UserId.ToString());
        if (string.IsNullOrEmpty(userRequestJson))
        {
            throw new ApplicationException("Đã quá hạn thời gian Verification Email");
        }

        Request.CreateUserAndAddress? userRequest =
            JsonConvert.DeserializeObject<Request.CreateUserAndAddress>(userRequestJson);

        // check exist
        AppUser? userByEmail = await userManager.FindByEmailAsync(userRequest.Email);

        if (userByEmail != null)
        {
            throw new IdentityException.AppUserAlreadyExistException(userRequest.Email);
        }

        // valid user => add new user
        var newUser = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = userRequest.UserName.Trim(),
            Email = userRequest.Email.Trim(),
            FirstName = userRequest.FirstName.Trim() ?? string.Empty,
            LastName = userRequest.LastName.Trim() ?? string.Empty,
            PhoneNumber = userRequest.PhoneNumber.Trim() ?? string.Empty,
            Gender = (GenderEnum)userRequest.Gender,
            DateOfBirth = userRequest.DateOfBirth ?? DateTime.Now,
            FullName = StringExtension.GetFullNameFromFirstNameAndLastName(userRequest.FirstName,
                userRequest.LastName),
            SecurityStamp = Guid.NewGuid().ToString() // Set a unique security stamp
        };

        var newAddress = new Domain.Entities.Address
        {
            Id = request.UserId,
            AddressLine1 = userRequest.AddressLine1 ?? string.Empty,
            AddressLine2 = userRequest.AddressLine2 ?? string.Empty,
            Street = userRequest.Street ?? string.Empty,
            City = userRequest.City ?? string.Empty,
            Unit = userRequest.Unit ?? string.Empty,
            Province = userRequest.Province ?? string.Empty
        };

        var newAppUserAddress = new UserAddress
        {
            AddressId = newAddress.Id,
            UserId = newUser.Id,
            IsDefault = DefaultEnum.TRUE
        };

        IdentityResult createUserResult = await userManager.CreateAsync(newUser, userRequest.Password);

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

        context.Address.Add(newAddress);

        context.UserAddress.Add(newAppUserAddress);

        await sender.Send(new Command.CreateChatRoomCommand(newUser.Id), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        // SIGNALR
        await registerHub.VerificationEmailAsync(new Response.VerifyResponseHub
        {
            Email = newUser.Email,
            IsVerified = (int)VerifyEnum.Success
        });

        return Result.Success();
    }
}
