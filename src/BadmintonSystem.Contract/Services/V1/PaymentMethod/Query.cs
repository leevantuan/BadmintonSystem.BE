using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.PaymentMethod;

public static class Query
{
    public record GetPaymentMethodByIdQuery(Guid PaymentMethodId)
        : IQuery<Response.PaymentMethodDetailResponse>;
}
