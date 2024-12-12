using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.PaymentMethod;

public static class Query
{
    public record GetPaymentMethodsQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.PaymentMethodResponse>>;

    public record GetPaymentMethodByIdQuery(Guid Id)
        : IQuery<Response.PaymentMethodResponse>;
}
