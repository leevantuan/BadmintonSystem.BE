using System.Text.RegularExpressions;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetRegisterByCustomerQueryHandler(
    IGmailService mailService,
    UserManager<AppUser> userManager,
    IRedisService redisService)
    : IQueryHandler<Query.RegisterByCustomerQuery>
{
    public async Task<Result> Handle(Query.RegisterByCustomerQuery request, CancellationToken cancellationToken)
    {
        AppUser? userByEmail = await userManager.FindByEmailAsync(request.Data.Email);

        if (userByEmail != null)
        {
            throw new IdentityException.AppUserAlreadyExistException(request.Data.Email);
        }

        var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
        bool passwordCondition = regex.IsMatch(request.Data.Password);
        if (!passwordCondition)
        {
            throw new IdentityException.AppUserPasswordConditionException(
                "Password must be at least 8 characters long and include uppercase letters, lowercase letters, numbers, and special characters.");
        }

        await redisService.SetAsync(request.Data.Email, request.Data, TimeSpan.FromMinutes(10));

        string verificationLink = $"https://bookingweb.shop/api/v1/users/verify-email?email={request.Data.Email}";

        // Gửi email xác thực
        await mailService.SendVerificationEmailAsync(request.Data.UserName, request.Data.Email, verificationLink);

        return Result.Success();
    }
}
