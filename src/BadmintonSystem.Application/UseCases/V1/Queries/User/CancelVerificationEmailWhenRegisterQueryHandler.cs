using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class CancelVerificationEmailWhenRegisterQueryHandler(
    IRedisService redisService)
    : IQueryHandler<Query.CancelVerificationEmailWhenRegisterQuery>
{
    public async Task<Result> Handle
        (Query.CancelVerificationEmailWhenRegisterQuery request, CancellationToken cancellationToken)
    {
        string userRequestJson = await redisService.GetAsync(request.Email);
        if (!string.IsNullOrEmpty(userRequestJson))
        {
            await redisService.DeleteByKeyAsync(request.Email);
        }

        return Result.Success();
    }
}
