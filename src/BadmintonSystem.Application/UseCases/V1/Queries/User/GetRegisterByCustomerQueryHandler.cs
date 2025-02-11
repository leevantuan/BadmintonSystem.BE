using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetRegisterByCustomerQueryHandler(
    IGmailService mailService,
    IRedisService redisService)
    : IQueryHandler<Query.RegisterByCustomerQuery>
{
    public async Task<Result> Handle(Query.RegisterByCustomerQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();

        await redisService.SetAsync(userId.ToString(), request.Data, TimeSpan.FromMinutes(10));

        string verificationLink = $"https://bookingweb.shop/api/v1/users/verify-email?userId={userId}";

        // Gửi email xác thực
        await mailService.SendVerificationEmailAsync(request.Data.UserName, request.Data.Email, verificationLink);

        return Result.Success();
    }
}
