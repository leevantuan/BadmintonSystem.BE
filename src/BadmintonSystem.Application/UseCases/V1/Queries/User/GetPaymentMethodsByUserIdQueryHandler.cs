using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetPaymentMethodsByUserIdQueryHandler(
    ApplicationDbContext context,
    IRepositoryBase<PaymentMethod, Guid> paymentMethodRepository,
    IMapper mapper)
    : IQueryHandler<Query.GetPaymentMethodsByUserIdQuery, PagedResult<Response.PaymentMethodByUserResponse>>
{
    public async Task<Result<PagedResult<Response.PaymentMethodByUserResponse>>> Handle
        (Query.GetPaymentMethodsByUserIdQuery request, CancellationToken cancellationToken)
    {
        AppUser? userById = context.AppUsers.FirstOrDefault(x => x.Id == request.UserId)
                            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        List<Response.PaymentMethodByUserResponse>? paymentMethods =
            mapper.Map<List<Response.PaymentMethodByUserResponse>>(userById.PaymentMethods.ToList());

        var result =
            PagedResult<Response.PaymentMethodByUserResponse>.Create(
                paymentMethods,
                1,
                10,
                paymentMethods.Count());

        return Result.Success(result);
    }
}
