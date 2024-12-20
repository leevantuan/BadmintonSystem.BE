using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.PaymentMethod;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.PaymentMethod;

public sealed class GetPaymentMethodByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.PaymentMethod, Guid> paymentMethodRepository)
    : IQueryHandler<Query.GetPaymentMethodByIdQuery, Response.PaymentMethodDetailResponse>
{
    public async Task<Result<Response.PaymentMethodDetailResponse>> Handle
        (Query.GetPaymentMethodByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.PaymentMethod paymentMethod =
            await paymentMethodRepository.FindByIdAsync(request.PaymentMethodId, cancellationToken)
            ?? throw new PaymentMethodException.PaymentMethodNotFoundException(request.PaymentMethodId);

        Response.PaymentMethodDetailResponse? result = mapper.Map<Response.PaymentMethodDetailResponse>(paymentMethod);

        return Result.Success(result);
    }
}
